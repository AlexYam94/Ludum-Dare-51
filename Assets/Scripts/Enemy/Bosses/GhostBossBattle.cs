using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class GhostBossBattle : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera _virtualCamera;
    [SerializeField] Transform _camPosition;
    [SerializeField] float _camSpeed;
    [SerializeField] float _moveSpeed;
    [SerializeField] float _phase3moveSpeedMultiplier;
    [SerializeField] Transform[] _spawnPoints;
    [SerializeField] float _activeTime;
    [SerializeField] float _fadeoutTime;
    [SerializeField] float _inactiveTime;
    [SerializeField] int _phase1Threshold;
    [SerializeField] int _phase2Threshold;
    [SerializeField] float _phase2FadeoutTimeMultiplier = 1;
    [SerializeField] Animator anim;
    [SerializeField] Transform _boss;
    [SerializeField] Collider2D _collider;
    [SerializeField] GameObject _bullet;
    [SerializeField] Transform _shotPoint;
    [SerializeField] float _fireRate;
    [SerializeField] float _phase2FireRateMultiplier;
    [SerializeField] float _phase3FireRateMultiplier;
    [SerializeField] GameObject _winObj;

    private bool _battelEnded;
    private Transform _targetPoint;
    private GameObject _player;
    private float _activeCounter, _fadeCounter, _inactiveCounter, _shotCounter;

    // Start is called before the first frame update
    void Start()
    {
        _player = PlayerHealthController.GetInstance().gameObject;
        _activeCounter = _activeTime;
        _shotCounter = _fireRate;
    }
    private void OnEnable()
    {
        _virtualCamera.GetComponent<LookAt>().enabled = false;
        _virtualCamera.Follow = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_battelEnded)
        {
            _virtualCamera.transform.position = Vector3.Lerp(_virtualCamera.transform.position, _camPosition.position, _camSpeed * Time.deltaTime);
        }
        ChangeBossState();
    }

    private void ChangeBossState()
    {
        if (_battelEnded)
        {
            _fadeCounter -= Time.deltaTime;
            if(_fadeCounter < 0)
            {
                _winObj?.SetActive(true);
                _winObj.transform.SetParent(null);

                if (Vector2.Distance(_virtualCamera.transform.position, _player.transform.position) > 0.5)
                {
                    Vector3 target = _player.transform.position;
                    target.z = -10;
                    _virtualCamera.transform.position = Vector3.Lerp(_virtualCamera.transform.position, target, _camSpeed * Time.deltaTime);
                }
                else
                {
                    _virtualCamera.GetComponent<LookAt>().enabled = true;

                    gameObject.SetActive(false);
                }
                
            }
        }else if (BossHealthController.instance.GetCurrentHealth() >= _phase1Threshold)
        {
            PhaseOneBehaviour();
        }
        else if(BossHealthController.instance.GetCurrentHealth() >= _phase2Threshold)
        {
            PhaseTwoBehaviour();
        }
        else
        {
            PhaseThreeBehaviour();
        }
    }
    private void PhaseThreeBehaviour()
    {
        if (_targetPoint == null)
        {
            _targetPoint = _boss;
            _fadeCounter = _fadeoutTime;
            anim.SetTrigger("vanished");
        }
        else
        {
            if (Vector3.Distance(_boss.transform.position, _targetPoint.position) > .02f && _collider.enabled)
            {
                _boss.position = Vector3.MoveTowards(_boss.position, _targetPoint.position, _moveSpeed * _phase3moveSpeedMultiplier * Time.deltaTime);
                Fire(_phase3FireRateMultiplier);
            }
            else if (_activeCounter > 0 && Vector3.Distance(_boss.transform.position, _targetPoint.position) <= .02f)
            {
                _activeCounter -= Time.deltaTime;
                if (_activeCounter < 0)
                {
                    _fadeCounter = _fadeoutTime * _phase2FadeoutTimeMultiplier;
                }

                Fire(_phase2FireRateMultiplier);
                //anim.SetTrigger("vanished");
            }
            else if (_fadeCounter > 0)
            {
                _fadeCounter -= Time.deltaTime;
                if (_fadeCounter < _fadeoutTime / 1.5)
                {
                    anim.SetTrigger("vanished");
                }
                if (_fadeCounter <= 0)
                {
                    _boss.gameObject.SetActive(false);
                    _inactiveCounter = _inactiveTime;
                }
            }
            else if (_inactiveCounter > 0)
            {
                _inactiveCounter -= Time.deltaTime;
                if (_inactiveCounter <= 0)
                {
                    _boss.position = _spawnPoints[Random.Range(0, _spawnPoints.Length)].position;


                    do
                    {
                        _targetPoint = _spawnPoints[Random.Range(0, _spawnPoints.Length)];
                    } while (_spawnPoints.Length > 1 && _targetPoint.position == _boss.position);

                    _boss.gameObject.SetActive(true);
                    _activeCounter = _activeTime;
                    _shotCounter = 1 / _fireRate;
                }
            }

        }
    }

    private void PhaseTwoBehaviour()
    {
        if (_targetPoint == null)
        {
            _targetPoint = _boss;
            _fadeCounter = _fadeoutTime;
            anim.SetTrigger("vanished");
        }
        else
        {
            if (Vector3.Distance(_boss.transform.position, _targetPoint.position) > .02f && _collider.enabled)
            {
                _boss.position = Vector3.MoveTowards(_boss.position, _targetPoint.position, _moveSpeed * Time.deltaTime);

                //if (Vector3.Distance(_boss.transform.position, _targetPoint.position) <= .02f)
                //{
                //    _fadeCounter = _fadeoutTime * _phase2FadeoutTimeMultiplier;
                //    //anim.SetTrigger("vanished");
                //}
                Fire(_phase2FireRateMultiplier);
            }else if (_activeCounter>0 && Vector3.Distance(_boss.transform.position, _targetPoint.position) <= .02f)
            {
                _activeCounter -= Time.deltaTime;
                if (_activeCounter < 0)
                {
                    _fadeCounter = _fadeoutTime * _phase2FadeoutTimeMultiplier;
                }

                Fire(_phase2FireRateMultiplier);
                //anim.SetTrigger("vanished");
            }
            else if (_fadeCounter > 0)
            {
                _fadeCounter -= Time.deltaTime;
                if(_fadeCounter < _fadeoutTime / 1.5)
                {
                    anim.SetTrigger("vanished"); 
                }
                if (_fadeCounter <= 0)
                {
                    _boss.gameObject.SetActive(false);
                    _inactiveCounter = _inactiveTime;
                }
            }
            else if (_inactiveCounter > 0)
            {
                _inactiveCounter -= Time.deltaTime;
                if (_inactiveCounter <= 0)
                {
                    _boss.position = _spawnPoints[Random.Range(0, _spawnPoints.Length)].position;


                    do
                    {
                        _targetPoint = _spawnPoints[Random.Range(0, _spawnPoints.Length)];
                    } while (_spawnPoints.Length > 1 && _targetPoint.position == _boss.position);

                    _boss.gameObject.SetActive(true);
                    _activeCounter = _activeTime;
                    _shotCounter = 1 / _fireRate;
                }
            }

        }
    }

    private void PhaseOneBehaviour()
    {
        if (_activeCounter > 0)
        {
            _activeCounter -= Time.deltaTime;
            if (_activeCounter <= 0)
            {
                _fadeCounter = _fadeoutTime;
                anim.SetTrigger("vanished");

            }
            Fire(1);
        }
        else if (_fadeCounter > 0)
        {
            _fadeCounter -= Time.deltaTime;
            if (_fadeCounter <= 0)
            {
                _boss.gameObject.SetActive(false);
                _inactiveCounter = _inactiveTime;
            }
        }
        else if (_inactiveCounter > 0)
        {
            _inactiveCounter -= Time.deltaTime;
            if (_inactiveCounter <= 0)
            {
                Vector3 lastPos = _boss.position;
                do
                {
                    _boss.position = _spawnPoints[Random.Range(0, _spawnPoints.Length)].position;
                } while (_spawnPoints.Length > 1 && lastPos == _boss.position);
                _boss.gameObject.SetActive(true);
                _activeCounter = _activeTime;

                _shotCounter = 1/_fireRate;
            }
        }
    }

    private void Fire(float multiplier)
    {
        _shotCounter -= Time.deltaTime;
        if (_shotCounter <= 0)
        {
            _shotCounter = (1/_fireRate) *(1/multiplier);

            Instantiate(_bullet, _shotPoint.position, Quaternion.identity);
        }
    }

    public void EndBattle()
    {
        _battelEnded = true;

        _fadeCounter = _fadeoutTime;
        anim.SetTrigger("vanished");
        _collider.enabled = false;
        BossBullet[] bullets = FindObjectsOfType<BossBullet>();
        foreach(var bullet in bullets){
            bullet.gameObject.SetActive(false);
        }
    }
}
