using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossActivator : MonoBehaviour
{
    [SerializeField] GameObject _bossToActivate;

    private void Start()
    {
        _bossToActivate.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            _bossToActivate.SetActive(true);

            gameObject.SetActive(false);
        }
    }
}
