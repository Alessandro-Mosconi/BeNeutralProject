using System;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class HammeringEnemy : EnemyPolicy
{

    public float maxHammeringDistance = 10;
    public float hammerActionProbability = 0.6f;
    public HammerComponent hammer;
    
    private EnemyFollowPlayerPolicy _followPlayerPolicy;
    private bool _canMove;
    
    protected override void OnPolicyStart()
    {
        _followPlayerPolicy = GetComponent<EnemyFollowPlayerPolicy>();
        _canMove = _followPlayerPolicy != null;
        hammer.enabled = false;
        hammer.OnHammeringStart = () =>
        {
            if (_canMove)
            {
                _followPlayerPolicy.enabled = false;
            }
        };
        hammer.OnHammeringEnd = () =>
        {
            if (_canMove)
            {
                _followPlayerPolicy.enabled = true;
                //Reset hammer timer
                ResetPolicyExecution();
            }
        };
    }

    protected override bool PolicyShouldDecide()
    {
        float distanceToPlayer = Vector3.Distance(target.transform.position, transform.position);
        return distanceToPlayer <= maxHammeringDistance && Random.Range(0, 1) <= hammerActionProbability;
    }

    protected override void ExecutePolicy()
    {
        hammer.enabled = true;
    }
}