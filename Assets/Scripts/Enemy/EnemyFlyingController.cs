using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFlyingController : MonoBehaviour
{
    [SerializeField] float _rangeToStartChase;
    [SerializeField] float _flySpeed;
    [SerializeField] float _turnSpeed;

    private Animator _anim;
    private Transform _playerLocation;
    private bool _isChasing;

    // Start is called before the first frame update
    void Start()
    {
        _playerLocation = PlayerHealthController.GetInstance().transform;
        _anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!_playerLocation.gameObject.GetComponent<PlayerController>().IsVisible()) return;
        if (!_isChasing)
        {
            if(Vector3.Distance(transform.position, _playerLocation.position) < _rangeToStartChase)
            {
                _isChasing = true;
                _anim.SetBool("isChasing", _isChasing);
            }
        }
        else
        {
            if (_playerLocation.gameObject.activeSelf)
            {
                Vector3 direction = transform.position - _playerLocation.position;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _turnSpeed * Time.deltaTime);

                //transform.position = Vector3.MoveTowards(transform.position, _playerLocation.position, _flySpeed* Time.deltaTime);
                transform.position += -transform.right * _flySpeed * Time.deltaTime;
            }
        }
        
    }
}
