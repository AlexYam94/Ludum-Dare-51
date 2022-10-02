using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthController : MonoBehaviour
{
    [SerializeField] int _totalHealth = 2;
    [SerializeField] GameObject _deathEffect;
    [SerializeField] int _score = 100;
    
    [SerializeField] GameObject _deathSound;

    public Action onDeath;

    public void Damage(int damageAmount)
    {
        if((_totalHealth -= damageAmount) <= 0)
        {
            if (_deathEffect != null)
            {
                GameObject.Instantiate(_deathSound,transform.position, Quaternion.identity).transform.SetParent(null);
                Instantiate(_deathEffect, transform.position, transform.rotation);
                GetComponent<DropitemController>()?.DropItem();
                ScoreController.GetInstance().Add(_score);
            }
            onDeath?.Invoke();
            Destroy(gameObject);
        }
    }
}
