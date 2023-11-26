using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyFollowPlayerPolicy : EnemyPolicy
{
    [SerializeField]
    public float minTargetDistance = 0;
    [SerializeField]
    public float maxTargetDistance = 1;
    [SerializeField]
    public float movementSpeed = 1;

    private BoxCollider2D _targetCollider, _selfCollider;

    protected override void OnPolicyStart()
    {
        _targetCollider = target.GetComponent<BoxCollider2D>();
        _selfCollider = GetComponent<BoxCollider2D>();
    }

    protected override bool PolicyShouldDecide()
    {
        float halfSqrt2 = Mathf.Sqrt(2) * 0.5f;
        Vector3 targetPosition = target.transform.position;
        Vector2 targetScaleVec = _targetCollider.size;
        float targetScale = (targetScaleVec.x + targetScaleVec.y) * halfSqrt2 / 2;
        Vector3 selfPosition = transform.position;
        Vector2 selfScaleVec = _selfCollider.size;
        float selfScale = (selfScaleVec.x + selfScaleVec.y) * halfSqrt2 / 2;

        var distanceToPlayer = Mathf.Abs(targetPosition.x - selfPosition.x) - targetScale - selfScale;
        return distanceToPlayer >= minTargetDistance && distanceToPlayer <= maxTargetDistance;
    }

    protected override void ExecutePolicy()
    {
        //Move enemy towards player horizontally, with constant speed
        Vector3 dirVector = Vector3.Normalize(target.transform.position - transform.position);
        float scaledMovementSpeed = movementSpeed * Time.deltaTime;
        transform.Translate(dirVector.x * scaledMovementSpeed, 0, 0);
    }
}
