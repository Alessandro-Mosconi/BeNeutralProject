using System;
using UnityEngine;

namespace Enemies.Behaviors
{
    public class EnemyBehavior : MonoBehaviour
    {
        [SerializeField] public EnemyBehaviorType Type;
        [SerializeField] public float UpdateInterval;
        [HideInInspector] public int switchSignalCode;

        protected float TimeSinceLastUpdate = 0;
        protected WeakReference<Transform> WeakSelf;
        protected EnemyController Controller;

        public void LinkToController(EnemyController controller)
        {
            Controller = controller;
            WeakSelf = new WeakReference<Transform>(controller.transform);
        }

        public virtual void ResetBehavior(Transform self)
        {
            TimeSinceLastUpdate = 0;
        }

        public virtual bool PerformStep(PlayerManager target, float deltaTime)
        {
            return false;
        }
    
        public virtual void DidAbandonState() {}
    }
}