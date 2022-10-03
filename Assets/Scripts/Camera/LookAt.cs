using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAt : MonoBehaviour
{
    [SerializeField] string _playerTag;
    [SerializeField] Transform _follow;

    CinemachineVirtualCamera _virtualCam;
    GameObject _player;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag(_playerTag);
        _virtualCam = GetComponent<CinemachineVirtualCamera>();
        //transform.position = _player.transform.position;
        _virtualCam.ForceCameraPosition(_player.transform.position, transform.rotation);
    }

    // Update is called once per frame
    void Update()
    {
        if(_player == null)
        {
            _player = PlayerHealthController.GetInstance().gameObject;
        }
        if(_virtualCam.Follow == null)
        {
            if (_follow != null)
            {
                _virtualCam.Follow = _follow;
                _virtualCam.ForceCameraPosition(new Vector3(_follow.position.x, _follow.position.y, transform.position.z), transform.rotation);
            }
            else
            {
                _virtualCam.Follow = _player.transform;
                _virtualCam.ForceCameraPosition(new Vector3(_player.transform.position.x, _player.transform.position.y, transform.position.z), transform.rotation);
            }
        }
    }
}
