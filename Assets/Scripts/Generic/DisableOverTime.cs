using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableOverTime : MonoBehaviour
{
    [SerializeField] float _lifeTime = 2f;
    float _timeToDisable = 0;

    private void Update()
    {
        if (_timeToDisable < _lifeTime)
        {
            _timeToDisable += Time.deltaTime;
        }
        else
        {
            _timeToDisable = 0;
            gameObject.SetActive(false);
        }
    }
}
