using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemies
{
    public abstract class EnemyPolicy : MonoBehaviour
    {

        [SerializeField] public GameObject target;
        [SerializeField] public float updateInterval = 1;

        private float _cumulatedTimeInterval = 0;
        private bool _policyEnabled = true;
        private bool _policyActiveInRunLoop = true;
        private bool _isInitialPolicyUpdate = true;

        // Start is called before the first frame update
        protected void Start()
        {
            if (target == null)
            {
                _policyEnabled = false;
            }
            else
            {
                OnPolicyStart();
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (!_policyEnabled)
            {
                return;
            }

            _cumulatedTimeInterval += Time.deltaTime;
            if (_cumulatedTimeInterval >= updateInterval || _isInitialPolicyUpdate)
            {
                _policyActiveInRunLoop = PolicyShouldDecide();
                _cumulatedTimeInterval -= updateInterval;
                _isInitialPolicyUpdate = false;
            }

            if (_policyActiveInRunLoop)
            {
                ExecutePolicy();
            }
        }

        protected void ResetPolicyExecution()
        {
            _cumulatedTimeInterval = 0;
            _policyActiveInRunLoop = false;
        }

        abstract protected void OnPolicyStart();
        abstract protected bool PolicyShouldDecide();
        abstract protected void ExecutePolicy();
    }
}
