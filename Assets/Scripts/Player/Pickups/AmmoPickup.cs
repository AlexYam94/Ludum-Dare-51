using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPickup : MonoBehaviour
{
    [SerializeField] float _reloadPercentage;
    [SerializeField] GameObject _pickupEffect;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.gameObject.GetComponentInParent<FireController>().Reload(_reloadPercentage);
            if (_pickupEffect != null)
            {
                Instantiate(_pickupEffect, transform.position, Quaternion.identity);
            }
            Destroy(gameObject);
        }
    }
}
