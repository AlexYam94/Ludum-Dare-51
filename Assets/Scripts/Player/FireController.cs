using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    [SerializeField] AudioSource _audioSource;
    [SerializeField] TextMeshProUGUI _ammoText;

    public Weapon _currentWeapon;

    public int poolCount = 0;

    private int bulletCount;
    //ObjectPool<Bullet> _bulletPool;
    PlayerAnimation _playerAnimation;
    Transform _whereToFire;
    FirePattern _pattern;
    float _fireCounter = 0;
    //IEnumerator _currentReloadCoroutine;
    
    // Transform of the GameObject you want to shake
    private Transform transform;

    // Desired duration of the shake effect
    private float shakeDuration = 0f;

    // A measure of magnitude for the shake. Tweak based on your preference
    private float shakeMagnitude = 0.7f;

    // A measure of how quickly the shake effect should evaporate
    private float dampingSpeed = 1.0f;

    // The initial position of the GameObject
    Vector3 initialPosition;




    private bool _canFire = true;

    public bool _isStanding { get; set; } = true;

    private void Start()
    {
        _playerAnimation = GetComponent<PlayerAnimation>();
        _whereToFire = _firePosition;
        //_bulletPool = new ObjectPool<Bullet>(CreateBullet, _bullectPrefab, _firePosition, _bulletPoolInitialCount);
        bulletCount = _currentWeapon.capacity;
        initialPosition = Camera.main.transform.localPosition;
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
        _ammoText.text = bulletCount + "/" +_currentWeapon.capacity;
        //poolCount = _bulletPool.GetCount();
        //DecideFirePosition();
        if (Input.GetKeyDown(KeyCode.J) || Input.GetKeyDown(KeyCode.Mouse0) || (Input.GetKey(KeyCode.Mouse0) && bulletCount > 0))
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
                        ShakeCamera();
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

    private void ShakeCamera()
    {
        if (shakeDuration > 0)
        {
            Camera.main.transform.localPosition = initialPosition + UnityEngine.Random.insideUnitSphere * shakeMagnitude;

            shakeDuration -= Time.deltaTime * dampingSpeed;
        }
        else
        {
            shakeDuration = 0f;
            Camera.main.transform.localPosition = initialPosition;
        }
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
        shakeDuration = .2f;

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
        _fireCounter = 0;
    }

    public void Reload(float percentage)
    {
        bulletCount += (int)Mathf.Ceil(_currentWeapon.capacity * percentage);
    }
}
