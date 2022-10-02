using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Bullet;
using static FirePattern;

public class FireController : MonoBehaviour
{
    //[SerializeField] Bullet _bullectPrefab;
    [SerializeField] Transform _firePosition;
    [SerializeField] Transform _fireUpPosition;
    [SerializeField] GameObject _bomb;
    [SerializeField] Transform _bombPosition;
    [SerializeField] int _bulletPoolInitialCount = 10;
    [SerializeField] AudioClip _emptyGunShot;
    
    public Weapon _currentWeapon;

    public int poolCount = 0;

    private int bulletCount;
    AudioSource _audioSource;
    //ObjectPool<Bullet> _bulletPool;
    PlayerAnimation _playerAnimation;
    Transform _whereToFire;
    FirePattern _pattern;
    float _fireCounter = 0;
    //IEnumerator _currentReloadCoroutine;




    private bool _canFire = true;

    public bool _isStanding { get; set; } = true;

    private void Start()
    {
        _playerAnimation = GetComponent<PlayerAnimation>();
        _whereToFire = _firePosition;
        //_bulletPool = new ObjectPool<Bullet>(CreateBullet, _bullectPrefab, _firePosition, _bulletPoolInitialCount);
        _audioSource = GetComponent<AudioSource>();
        bulletCount = _currentWeapon.capacity;
    }

    private void OnLevelWasLoaded()
    {
        //_bulletPool = new ObjectPool<Bullet>(CreateBullet, _bullectPrefab, _firePosition, _bulletPoolInitialCount);
    }


    private void OnDisable()
    {
        //_bulletPool.ClearPool();
    }

    private void Update()
    {
        //poolCount = _bulletPool.GetCount();
        //DecideFirePosition();
        if (Input.GetKeyDown(KeyCode.J) || Input.GetKeyDown(KeyCode.Mouse0) || (Input.GetKey(KeyCode.Mouse0) && bulletCount > 0 ))
        {
            if (_fireCounter <= 0)
            {
                _fireCounter = _currentWeapon.fireRate;
                if (bulletCount <= 0)
                {
                    _audioSource.PlayOneShot(_emptyGunShot);
                }
                else
                {
                    if (_isStanding && _canFire)
                    {
                        Shoot();
                    }
                }
            }
        }
        _fireCounter -= Time.deltaTime;
        
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            _canFire = true;
        }
        //if (bulletCount <= 0)
        //{
        //    _currentReloadCoroutine = Reload();
        //    StartCoroutine(_currentReloadCoroutine);
        //}
    }

    private IEnumerator Reload()
    {
        //TODO: Show reload progress bar
        //TODO: Play reload animation
        yield return new WaitForSeconds(_currentWeapon.reloadTime);
        bulletCount = _currentWeapon.capacity;
        //TODO: Reset reload progress bar
        //TODO: Hide reload progress bar
        //TODO: Stop reload animation
    }

    private void Shoot()
    {
        /*
            FirePatternDirection[] firePatternDirections = _pattern.directions;
            foreach(FirePatternDirection pattern in firePatternDirections)
            {
                Bullet bullet = _bulletPool.Dequeue();
                //bullet.SetDirection(ConvertFirePatternDirectionToBulletDirection(pattern));
                bullet.SetDirection(_firePosition.transform.right);
                bullet.transform.position = _whereToFire.position;
                if (bullet.GetObjectPool() == null)
                {
                    bullet.SetObjectPool(_bulletPool);
                }

            }
        */
        if (!_currentWeapon.autoMode)
        {
            _canFire = false;
        }
        _currentWeapon.Fire().Invoke(_firePosition,_firePosition.transform.right);
        _playerAnimation.Shoot();
        _audioSource.PlayOneShot(_currentWeapon.gunshot);
        bulletCount -= 1;
    }

    private void DecideFirePosition()
    {
        if (Input.GetKey(KeyCode.W))
        {
            _whereToFire = _fireUpPosition;
        }
        else
        {
            _whereToFire = _firePosition;
        }
    }

    private Bullet CreateBullet(Bullet gameObject, Transform transform)
    {
        Bullet bullet = GameObject.Instantiate(gameObject, transform.position, transform.rotation);
        bullet.gameObject.SetActive(false);
        //bullet.SetObjectPool(_bulletPool);
        //DontDestroyOnLoad(bullet);
        return bullet;
    }

    public void DisableFire()
    {
        _canFire = false;

    }

    public void EnableFire()
    {
        _canFire = true;
    }

    public void ChangeWeapon(Weapon nextWeapon)
    {
        bulletCount = nextWeapon.capacity;
        _currentWeapon = nextWeapon;
        //StopCoroutine(_currentReloadCoroutine);
        _playerAnimation.OverrideArmController(nextWeapon.weaponAnimatorController);
    }
}
