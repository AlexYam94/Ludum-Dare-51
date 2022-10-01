using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    [SerializeField] public float bulletSpeed;
    [SerializeField] ParticleSystem _impactEffect;
    [SerializeField] float _lifeTime = 10f;
    [SerializeField] int _damageAmount = 1;


    ObjectPool<Bullet> _pool;

    Rigidbody2D _rb;
    //Direction _direction;
    Vector3 _direction;
    Vector2 _moveDir = Vector2.right;

    public void SetObjectPool(ObjectPool<Bullet> pool)
    {
        _pool = pool;
    }

    public ObjectPool<Bullet> GetObjectPool()
    {
        return _pool;
    }

    //public void SetDirection(Direction direction)
    //{
    //    _direction = direction;
    //}
    public void SetDirection(Vector3 direction)
    {
        _direction = direction;
    }

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _rb.velocity = _direction * bulletSpeed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Enemy")
        {
            other.GetComponent<EnemyHealthController>()?.Damage(_damageAmount);
        }
        if (other.tag == "Boss")
        {
            other.GetComponentInParent<BossHealthController>().TakeDamage(_damageAmount);
        }
        //_pool.Enqueue(this);
        //gameObject.SetActive(false);

        Destroy(gameObject);
        if (_impactEffect != null)
        {
            //TODO: Play hit sound
            Instantiate(_impactEffect, transform.position, Quaternion.identity);
        }
    }

    private void OnBecameInvisible()
    {
        //_pool.Enqueue(this);
        //gameObject.SetActive(false);
        Destroy(gameObject);
    }
}
