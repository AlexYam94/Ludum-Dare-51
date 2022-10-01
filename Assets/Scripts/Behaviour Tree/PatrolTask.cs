using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public class PatrolTask : Node
    {
        private Transform _transform;
        private Transform[] _patrolPoints;
        private Rigidbody2D _rb;

        float _moveSpeed;
        float _waitAtPoint;
        float _jumpForce;
        EnemyAnimation _enemyAnimation;
        int _currentXPoint = 0;
        float _waitCounter;

        public PatrolTask(Transform transform, Transform[] wayPoints, float moveSpeed, float waitAtPoint, float jumpForce)
        {
            _transform = transform;
            _patrolPoints = wayPoints;
            _rb = transform.GetComponent<Rigidbody2D>();
            _enemyAnimation = transform.GetComponent<EnemyAnimation>();
            _moveSpeed = moveSpeed;
            _waitAtPoint = waitAtPoint;
            _jumpForce = jumpForce;
        }

        public override NodeState Evaluate()
        {

            if (Mathf.Abs(_transform.position.x - _patrolPoints[_currentXPoint].position.x) > .2f)
            {
                GoToPoint();
            }
            else
            {
                FindNextPoint();
            }
            _enemyAnimation.Move(Mathf.Abs(_rb.velocity.x));

            return NodeState.SUCCESS;
        }
        private void FindNextPoint()
        {
            _rb.velocity = new Vector2(0, _rb.velocity.y);
            _waitCounter -= Time.deltaTime;
            if (_waitCounter <= 0)
            {
                _waitCounter = _waitAtPoint;
                _currentXPoint++;
                if (_currentXPoint >= _patrolPoints.Length)
                {
                    _currentXPoint = 0;
                }
            }
        }

        private void GoToPoint()
        {
            if (_transform.position.x < _patrolPoints[_currentXPoint].position.x)
            {
                _rb.velocity = new Vector2(_moveSpeed, _rb.velocity.y);
                flipX(false);
            }
            else
            {
                _rb.velocity = new Vector2(-_moveSpeed, _rb.velocity.y);
                flipX(true);
            }
            if (_transform.position.y < _patrolPoints[_currentXPoint].position.y - .5f && _rb.velocity.y < .1f)
            {
                _rb.velocity = new Vector2(_rb.velocity.x, _jumpForce);
            }
        }

        private void flipX(bool facingRight)
        {
            Vector3 scale = _transform.localScale;
            scale.x = facingRight ? 1 : -1;
            _transform.localScale = scale;
        }
    }
}