using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [SerializeField] Weapon[] _weapons;
    [SerializeField] float _loopTime = 10f;
    [SerializeField] SpriteRenderer _armSprite;

    FireController _fireController;
    float _loopCounter;
    Weapon _lastWeapon;


    // Start is called before the first frame update
    void Start()
    {
        _fireController = GetComponent<FireController>();
        _lastWeapon = _fireController._currentWeapon;
        _loopCounter = _loopTime;
    }

    // Update is called once per frame
    void Update()
    {
        _loopCounter -= Time.deltaTime;
        if (_loopCounter < 0)
        {
            _loopCounter = _loopTime;
            Weapon nextWeapon = GetRandomWeapon();
            _lastWeapon = nextWeapon;
            _fireController.ChangeWeapon(nextWeapon);
            //TODO:
            //Change arm sprite and override animator
            _armSprite.sprite = nextWeapon.weaponSprite;

        }
    }

    private Weapon GetRandomWeapon()
    {
        System.Random random = new System.Random();
        Weapon nextWeapon;
        do
        {
            nextWeapon = _weapons[random.Next(0, _weapons.Length)];
        } while (nextWeapon.type == _lastWeapon.type);
        _lastWeapon = nextWeapon;
        return nextWeapon;
    }
}
