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
        private PlayerManager _target;

        private void Start()
        {
            _activeBehaviors = GetComponents<EnemyBehavior>();
            //Initialize the hash map
            for (int i = 0; i < _activeBehaviors.Length; i++)
            {
                _behaviorToIndexMap[(int)_activeBehaviors[i].Type()] = i;
            }
            foreach (int type in Enum.GetValues(typeof(EnemyBehaviorType)))
            {
                _behaviorToIndexMap.TryAdd(type, -1);
            }

            bool addedInitialState = false;
            foreach (EnemyBehaviorType behaviorType in Enum.GetValues(typeof(EnemyBehaviorType)))
            {
                int index = _behaviorToIndexMap[(int)behaviorType];
                if (index != -1 && (!addedInitialState || behaviorType == EnemyBehaviorType.AbsorbsCharge))
                {
                    _currentStates.Add(index);
                    addedInitialState = true;
                }
            }

            List<EnemyBehaviorType> currentBehaviors = new List<EnemyBehaviorType>(_currentStates.Count);
            foreach (var state in _currentStates)
            {
                currentBehaviors.Add(_activeBehaviors[state].Type());
            }
            foreach (EnemyBehavior activeBehavior in _activeBehaviors)
            {
                activeBehavior.LinkToController(this);
                activeBehavior.ResetBehavior(transform);
                if (!currentBehaviors.Contains(activeBehavior.Type()))
                {
                    activeBehavior.DidAbandonState();
                }
            }

            _target = target.GetComponent<PlayerManager>();
            String states = "";
            _currentStates.ForEach(s => states += _activeBehaviors[s].Type() + ", ");
            print("STATE MACHINE INIT: Initial states are " + states);
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
                bool shouldMoveToNextState = behavior.PerformStep(_target, Time.deltaTime);
                int signalCode = behavior.switchSignalCode;
                if (shouldMoveToNextState)
                {
                    switch (behavior.Type())
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
                        case EnemyBehaviorType.Radar:
                            //Move to Shoot
                            MoveToState(i, EnemyBehaviorType.Shoot, ref sank);
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
                            //Move to Radar
                            bool movedToRadar = MoveToState(i, EnemyBehaviorType.Radar, ref sank);
                            if (!movedToRadar)
                            {
                                //Attempt to move to Patrol
                                AddActiveStateIfPresent(EnemyBehaviorType.Patrol);
                            }
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