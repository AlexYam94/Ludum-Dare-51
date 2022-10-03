using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    [SerializeField] int _extraHealthAmount;
    [SerializeField] GameObject _pickupEffect;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            PlayerHealthController.GetInstance().GetHealth(_extraHealthAmount);
            if(_pickupEffect != null)
            {
                Instantiate(_pickupEffect, transform.position, Quaternion.identity);
            }
            Destroy(gameObject);
        }
    }
}
