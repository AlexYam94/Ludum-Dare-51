using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePlayer : MonoBehaviour
{
    [SerializeField] int _damageAmount = 1;
    [SerializeField] GameObject _destroyEffect;
    [SerializeField]  bool _destroyOnDamage = false;

    private void OnCollisionEnter2D(Collision2D other)
    {
        DealDamage(other.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        DealDamage(other.gameObject);
    }

    private void DealDamage(GameObject other)
    {
        if (other.tag.Equals("Player"))
        {
            PlayerHealthController.GetInstance().Damage(_damageAmount);
            if (_destroyOnDamage)
            {
                if(_destroyEffect != null)
                {
                    Instantiate(_destroyEffect, transform.position, transform.rotation);
                }
                Destroy(gameObject);
            }
        }
    }

}
