using System;
using System.Collections.Generic;
using UnityEngine;

namespace Enemies.Behaviors
{
    /*
     * A state machine simulator to have a common behavior for all enemies, customizable with a list of active behaviors for the enemy
     */
    public class EnemyController : MonoBehaviour
    {
        public GameObject target;

        private EnemyBehavior[] _activeBehaviors;
        private readonly List<int> _currentStates = new List<int>();
        private readonly Dictionary<int, int> _behaviorToIndexMap = new Dictionary<int, int>();
        private int _stateToRestoreAfterAction = -1;

        private void Start()
        {
            _activeBehaviors = GetComponents<EnemyBehavior>();
            //Initialize the hash map
            for (int i = 0; i < _activeBehaviors.Length; i++)
            {
                _behaviorToIndexMap[(int)_activeBehaviors[i].Type] = i;
            }
            foreach (int type in Enum.GetValues(typeof(EnemyBehaviorType)))
            {
                _behaviorToIndexMap.TryAdd(type, -1);
            }
            
            //Initialize the machine
            int patrolIndex = _behaviorToIndexMap[(int)EnemyBehaviorType.Patrol];
            if (patrolIndex != -1)
            {
                _currentStates.Add(patrolIndex);
            }
            else
            {
                int followIndex = _behaviorToIndexMap[(int)EnemyBehaviorType.Chase];
                if (followIndex != -1)
                {
                    _currentStates.Add(followIndex);
                }
                else
                {
                    //The enemy neither follows nor patrol. Choose one of the actions (hammer, shoot etc)
                    int hammerIndex = _behaviorToIndexMap[(int)EnemyBehaviorType.Hammer];
                    int shootIndex = _behaviorToIndexMap[(int)EnemyBehaviorType.Shoot];
                    if (shootIndex != -1)
                    {
                        _currentStates.Add(shootIndex);
                    } else if (hammerIndex != -1)
                    {
                        _currentStates.Add(hammerIndex);
                    }
                }
            }

            foreach (EnemyBehavior activeBehavior in _activeBehaviors)
            {
                activeBehavior.LinkToController(this);
                activeBehavior.ResetBehavior(transform);
                if (activeBehavior.Type != _activeBehaviors[_currentStates[0]].Type)
                {
                    activeBehavior.DidAbandonState();
                }
            }
            print("STATE MACHINE INIT: Initial state is " + _activeBehaviors[_currentStates[0]].Type);
        }

        private void Update()
        {
            //Simulate the state machine, one step at a time
            bool sank = false;
            bool switchedToHammer = false;
            for (int i = 0; i < _currentStates.Count; i++)
            {
                int currentState = _currentStates[i];
                EnemyBehavior behavior = _activeBehaviors[currentState];
                bool shouldMoveToNextState = behavior.PerformStep(target, Time.deltaTime);
                int signalCode = behavior.switchSignalCode;
                if (shouldMoveToNextState)
                {
                    switch (behavior.Type)
                    {
                        case EnemyBehaviorType.Patrol:
                            //Move to Follow
                            MoveToState(i, EnemyBehaviorType.Chase, ref sank);
                            bool addedShieldAction = AddActiveStateIfPresent(EnemyBehaviorType.Shield);
                            if (!addedShieldAction)
                            {
                                AddActiveStateIfPresent(EnemyBehaviorType.Shoot);
                            }
                            break;
                        case EnemyBehaviorType.Chase:
                            //Move to Patrol OR Hammer (then Chase) dep. on signal code
                            if (signalCode == 0)
                            {
                                MoveToState(i, EnemyBehaviorType.Patrol, ref sank);
                            }
                            else
                            {
                                switchedToHammer = MoveToState(i, EnemyBehaviorType.Hammer, ref sank);
                                RemoveStateIfPresent(EnemyBehaviorType.Shield);
                                if (switchedToHammer)
                                {
                                    _stateToRestoreAfterAction = (int)EnemyBehaviorType.Chase;
                                }
                            }
                            break;
                        case EnemyBehaviorType.Hammer:
                            //Move to Shield
                            MoveToState(i, EnemyBehaviorType.Shield, ref sank);
                            if (_stateToRestoreAfterAction != -1)
                            {
                                AddActiveStateIfPresent((EnemyBehaviorType)_stateToRestoreAfterAction);
                                _stateToRestoreAfterAction = -1;
                            }
                            break;
                        case EnemyBehaviorType.Shield:
                            //Move to Hammer
                            MoveToState(i, EnemyBehaviorType.Hammer, ref sank);
                            break;
                        case EnemyBehaviorType.Shoot:
                            //Move to Patrol
                            MoveToState(i, EnemyBehaviorType.Patrol, ref sank);
                            break;
                    }
                }
            }
            //Clean all transitions to the Sink state
            if (sank)
            {
                _currentStates.RemoveAll((a) => a == -1);
            }
        }

        private bool MoveToState(int currentStateIndex, EnemyBehaviorType nextState, ref bool sank)
        {
            int newStateIndex = _behaviorToIndexMap[(int) nextState];
            
            _activeBehaviors[_currentStates[currentStateIndex]].DidAbandonState();
            if (newStateIndex != -1)
            {
                _currentStates[currentStateIndex] = newStateIndex;
                _activeBehaviors[newStateIndex].ResetBehavior(transform);
                print("MOVING to state " + nextState);
                return true;
            }
            else
            {
                _currentStates[currentStateIndex] = -1;
                sank = true;
                return false;
            }
        }
        
        private bool AddActiveStateIfPresent(EnemyBehaviorType newState)
        {
            int newStateIndex = _behaviorToIndexMap[(int) newState];
            if (newStateIndex != -1)
            {
                if (!_currentStates.Contains(newStateIndex))
                {
                    _currentStates.Add(newStateIndex);
                }
                _activeBehaviors[newStateIndex].ResetBehavior(transform);
                print("ADDING state " + newState);
                return true;
            }

            return false;
        }
        
        private bool RemoveStateIfPresent(EnemyBehaviorType state)
        {
            int stateIndex = _behaviorToIndexMap[(int) state];
            if (stateIndex != -1)
            {
                _activeBehaviors[stateIndex].DidAbandonState();
                _currentStates.Remove(stateIndex);
                print("REMOVING state " + state);
                return true;
            }

            return false;
        }
    }
}