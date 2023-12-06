using UnityEngine;
using Enemies.Weapons;

namespace Enemies.Behaviors
{
    public class EnemyShootBehavior : EnemyBehavior
    {
        public float minDirectionalError = 0;
        public float maxDirectionalError = 0;
        public float maxTargetDistance = 10;
        public float shootingSpeed = 2;
        public GameObject bulletPrefab;

        private float _cumulatedShootingDelay = 0;
        private Vector2 _shootingBaseDirection;

        public override void ResetBehavior(Transform self)
        {
            _cumulatedShootingDelay = 0;
            ObjectPoolingManager.Instance.CreatePool(bulletPrefab, 50, 100);

            base.ResetBehavior(self);
        }

        public override bool PerformStep(GameObject target, float deltaTime)
        {
            //1. Check if the Player is not visible anymore - in this case stop shooting
            bool playerVisible = CheckTargetVisible(target);
            //2. Otherwise, keep shooting at the Player
            UpdateBaseShootingDirection(target, deltaTime);
            ShootTarget(target, deltaTime);
            return !playerVisible;
        }

        public override void DidAbandonState()
        {
            base.DidAbandonState();
        }

        private bool CheckTargetVisible(GameObject target)
        {
            if (WeakSelf.TryGetTarget(out Transform self))
            {
                return Vector3.SqrMagnitude(self.position - target.transform.position) <=
                       (maxTargetDistance * maxTargetDistance);
            }

            return true;
        }

        private void UpdateBaseShootingDirection(GameObject target, float deltaTime)
        {
            if (WeakSelf.TryGetTarget(out Transform self))
            {
                TimeSinceLastUpdate += deltaTime;
                if (TimeSinceLastUpdate >= UpdateInterval)
                {
                    Vector3 dirTowardsPlayer = Vector3.Normalize(target.transform.position - self.position);
                    _shootingBaseDirection = new Vector2(dirTowardsPlayer.x, dirTowardsPlayer.y);
                    TimeSinceLastUpdate -= UpdateInterval;
                }
            }
        }

        private void ShootTarget(GameObject target, float deltaTime)
        {
            if (WeakSelf.TryGetTarget(out Transform self))
            {
                _cumulatedShootingDelay += deltaTime;
                if (_cumulatedShootingDelay >= 1 / shootingSpeed)
                {
                    float randomDirectionalErrorRad = Random.Range(minDirectionalError, maxDirectionalError);
                    float errorCos = Mathf.Cos(randomDirectionalErrorRad);
                    float errorSin = Mathf.Sin(randomDirectionalErrorRad);
                    Vector2 rotatedBulletDirection =
                        new Vector2(_shootingBaseDirection.x * errorCos - _shootingBaseDirection.y * errorSin,
                            _shootingBaseDirection.x * errorSin + _shootingBaseDirection.y * errorCos);
                    //Instantiate a Bullet object
                    GameObject bullet = ObjectPoolingManager.Instance.GetObject(bulletPrefab.name);
                    bullet.transform.position = self.position;
                    bullet.transform.rotation =
                        Quaternion.AngleAxis(
                            Mathf.Rad2Deg * Mathf.Atan2(rotatedBulletDirection.y, rotatedBulletDirection.x),
                            Vector3.forward);
                    _cumulatedShootingDelay -= 1 / shootingSpeed;
                }
            }
        }
    }
}