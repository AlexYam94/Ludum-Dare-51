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


    // Start is called before the first frame update
    void Start()
    {
        _fireController = GetComponent<FireController>();
    }

    // Update is called once per frame
    void Update()
    {
        _loopCounter -= Time.deltaTime;
        if (_loopCounter < 0)
        {
            _loopCounter = _loopTime;
            Weapon nextWeapon = GetRandomWeapon();
            _fireController._currentWeapon = nextWeapon;
            //TODO:
            //Change arm sprite and override animator
            _armSprite.sprite = nextWeapon.weaponSprite;

        }
    }

    private Weapon GetRandomWeapon()
    {
        System.Random random = new System.Random();
        return _weapons[random.Next(0, _weapons.Length)];
    }
}
