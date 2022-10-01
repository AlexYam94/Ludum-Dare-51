using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportToPlayer : MonoBehaviour
{
    [SerializeField] string _playerTag;
    [SerializeField] GameObject _virtualCamera;

    GameObject _player;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag(_playerTag);
        if (Vector3.Distance(transform.position, _player.transform.position) > 50)
        {
            transform.position = _player.transform.position;
        }
        _virtualCamera.SetActive(true);
    }

    private void Update()
    {    
    }
}
