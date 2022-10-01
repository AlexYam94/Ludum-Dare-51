using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBullet : MonoBehaviour
{
    [SerializeField] float _moveSpeed;
    [SerializeField] Rigidbody2D _rb;
    [SerializeField] int _damageAmount;
    [SerializeField] GameObject _imapctEffect;
    // Start is called before the first frame update
    void Start()
    {
        Vector3 direction = transform.position - PlayerHealthController.GetInstance().gameObject.transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    // Update is called once per frame
    void Update()
    {
        _rb.velocity = -transform.right * _moveSpeed;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            PlayerHealthController.GetInstance().Damage(_damageAmount);
        }
        if (_imapctEffect != null)
        {
            Instantiate(_imapctEffect, transform.position, transform.rotation);
        }
        Destroy(gameObject);
    }
}
