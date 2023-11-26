using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShootPlayerPolicy : EnemyPolicy
{
    
    [SerializeField]
    public float minDirectionalError = 0;
    [SerializeField]
    public float maxDirectionalError = 0;
    [SerializeField]
    public float shootingSpeed = 2;
    public GameObject bulletPrefab;

    private float _cumulatedShootingDelay = 0;
    private Vector2 _shootingBaseDirection;
    
    protected override void OnPolicyStart()
    {
        
    }

    protected override bool PolicyShouldDecide()
    {
        Vector3 dirTowardsPlayer = (target.transform.position - transform.position);
        _shootingBaseDirection = new Vector2(dirTowardsPlayer.x, dirTowardsPlayer.y);
        return true; //TODO: Implement radar!
    }

    protected override void ExecutePolicy()
    {
        _cumulatedShootingDelay += Time.deltaTime;
        if (_cumulatedShootingDelay >= 1 / shootingSpeed)
        {
            float randomDirectionalErrorRad = Random.Range(minDirectionalError, maxDirectionalError);
            float errorCos = Mathf.Cos(randomDirectionalErrorRad);
            float errorSin = Mathf.Sin(randomDirectionalErrorRad);
            Vector2 rotatedBulletDirection = new Vector2(_shootingBaseDirection.x * errorCos - _shootingBaseDirection.y * errorSin, _shootingBaseDirection.x * errorSin + _shootingBaseDirection.y * errorCos);
            //Instantiate a Bullet object
            Instantiate(bulletPrefab, transform.position, Quaternion.AngleAxis(Mathf.Rad2Deg * Mathf.Atan2(rotatedBulletDirection.y, rotatedBulletDirection.x), Vector3.forward));
            _cumulatedShootingDelay -= 1 / shootingSpeed;
        }
    }
}
