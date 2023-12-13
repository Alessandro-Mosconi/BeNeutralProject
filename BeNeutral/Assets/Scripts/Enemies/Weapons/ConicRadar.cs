using System;
using UnityEngine;

namespace Enemies.Weapons
{
    public class ConicRadar : MonoBehaviour
    {
        private float _baseScale = 1.0f / 3.0f;
        private Vector3 _rotationPivot;
        private Vector2 _untransformedRotationPivot;
        private Quaternion _baseRotation;
        private Vector2 _currentAxis;
        private float _currentDepth;
        private float _currentApertureAngle;
        private Collider2D _targetCollider;
        private void Start()
        {
            // Get the sprite renderer and its vertices
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            Vector2[] vertices = spriteRenderer.sprite.vertices;

            // Get the vertex to rotate around
            _untransformedRotationPivot = vertices[1];
            _rotationPivot = transform.TransformPoint(_untransformedRotationPivot);
            _baseRotation = transform.localRotation;
            _baseScale = transform.localScale.x;
        }

        public void SetConeAperture(float apertureAngleDeg)
        {
            float baseScaleMultiplier = apertureAngleDeg / 60.0f;
            transform.localScale = new Vector3(baseScaleMultiplier * (transform.localScale.y / _baseScale), transform.localScale.y, transform.localScale.z);
            _currentApertureAngle = apertureAngleDeg;
        }
        
        public void SetConeDepth(float depth)
        {
            Vector3 topVertexPosition = transform.TransformPoint(_untransformedRotationPivot);
            float modified_depth = depth / (Mathf.Sqrt(3) * 0.5f);

            transform.localScale = new Vector3(transform.localScale.x * (modified_depth * _baseScale / transform.localScale.y), modified_depth * _baseScale, transform.localScale.z);
            _currentDepth = depth;
            
            // Calculate the new position of the pivot vertex after scaling
            Vector3 newTopVertexPosition = transform.TransformPoint(_untransformedRotationPivot);

            // Calculate the difference in position and adjust the overall position to keep the top vertex fixed
            Vector3 positionAdjustment = topVertexPosition - newTopVertexPosition;
            transform.position += positionAdjustment;
        }

        public void ResetRotation()
        {
            RotateToDir(Vector2.right);
        }

        public void RotateToDir(Vector2 coneAxisDirection)
        {
            // Rotate the sprite around the chosen vertex
            transform.localRotation = Quaternion.Euler(transform.localRotation.eulerAngles.x, transform.localRotation.eulerAngles.y, 90 + (Mathf.Rad2Deg * Mathf.Atan2(coneAxisDirection.y, coneAxisDirection.x)));
            //Bring the pivot to 0
            Vector3 topVertexPosition = transform.TransformPoint(_untransformedRotationPivot);
            Vector3 parentPosition = transform.parent.position;
            transform.position += parentPosition - topVertexPosition;
            _currentAxis = coneAxisDirection;
        }

        public bool CheckRadarIntersections(GameObject target)
        {
            //Sphere cast, then discard intersections outside the cone
            //RaycastHit2D hit = Physics2D.CircleCast(_rotationPivot, _currentDepth, _currentAxis, _currentDepth, targetLayerMask);
            Vector3 closestPoint = target.transform.position;

            Collider2D collider;
            if (_targetCollider != null)
            {
                collider = _targetCollider;
            }
            else
            {
                collider = target.GetComponent<Collider2D>();
            }
            
            Bounds targetBounds = collider.bounds;
            closestPoint = targetBounds.ClosestPoint(transform.parent.position);
            if (Vector2.SqrMagnitude(transform.parent.position - closestPoint) <= (_currentDepth * _currentDepth))
            {
                return Vector2.Angle(_currentAxis, target.transform.position - _rotationPivot) <= (0.5f * _currentApertureAngle);
            }

            return false;
        }
    }
}