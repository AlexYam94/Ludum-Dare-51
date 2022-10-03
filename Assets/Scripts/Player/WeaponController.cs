using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [SerializeField] Weapon[] _weapons;
    [SerializeField] float _loopTime = 10f;
    [SerializeField] SpriteRenderer _armSprite;
    [SerializeField] TextMeshProUGUI _text;

    FireController _fireController;
    float _loopCounter;
    Weapon _lastWeapon;
    float _timer = 1f;


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
        _text.text = "" + Mathf.Ceil(_loopCounter);
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
        if (_timer < 0)
        {
            _timer = 1f;
            ScoreController.GetInstance().Add(1);
        }
        _timer -= Time.deltaTime;
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
