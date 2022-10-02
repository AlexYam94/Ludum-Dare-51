using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatroller : MonoBehaviour
{
    [SerializeField] Transform[] _patrolPoints;
    [SerializeField] float _moveSpeed;
    [SerializeField] float _waitAtPoint;
    [SerializeField] float _jumpForce;
    [SerializeField] Rigidbody2D _rb;
    
    EnemyAnimation _enemyAnimation;
    int _currentXPoint = 0;
    float _waitCounter;



    // Start is called before the first frame update
    void Start()
    {
        _enemyAnimation = GetComponent<EnemyAnimation>();
        _waitCounter = _waitAtPoint;

        foreach(Transform pPoint in _patrolPoints)
        {
            pPoint.SetParent(null);
        }
        GetComponent<EnemyHealthController>().onDeath = Cleanup;
    }

    // Update is called once per frame
    void Update()
    {
        if (Mathf.Abs(transform.position.x - _patrolPoints[_currentXPoint].position.x) > .2f)
        {
            GoToPoint();
        }
        else
        {
            FindNextPoint();
        }
        _enemyAnimation.Move(Mathf.Abs(_rb.velocity.x));
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
        if (transform.position.x < _patrolPoints[_currentXPoint].position.x)
        {
            _rb.velocity = new Vector2(_moveSpeed, _rb.velocity.y);
            flipX(false);
        }
        else
        {
            _rb.velocity = new Vector2(-_moveSpeed, _rb.velocity.y);
            flipX(true);
        }
        //if(transform.position.y < _patrolPoints[_currentXPoint].position.y - .5f && _rb.velocity.y < .1f)
        //{
        //    _rb.velocity = new Vector2(_rb.velocity.x, _jumpForce);
        //}
    }

    private void Cleanup()
    {
        foreach(var point in _patrolPoints)
        {
            Destroy(point.gameObject);
        }
    }

    private void flipX(bool facingRight)
    {
        Vector3 scale = transform.localScale;
        scale.x = facingRight ? 1 : -1;
        transform.localScale = scale;
    }
}
