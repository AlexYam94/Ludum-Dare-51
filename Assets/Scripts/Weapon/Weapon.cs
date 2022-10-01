using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Weapon", fileName = "WeaponObject", order = 0)]
public class Weapon : ScriptableObject
{
    public weaponType type;
    public Sprite weaponSprite;
    public AnimatorOverrideController weaponAnimator;
    public bool autoMode = false;
    public int capacity = 10;
    public float reloadTime = 2;

    public enum weaponType{
        pistol,
        shotgun,
        smg,
        crossbow
    }

    public Action<Vector3, Vector2, Bullet> Fire()
    {
        switch (type)
        {
            case weaponType.pistol:
                return PistolFire;
            case weaponType.shotgun:
                return ShotgunFire;
            case weaponType.smg:
                return SmgFire;
            case weaponType.crossbow:
                return CrossbpwFire;
        }
        return PistolFire;
    }

    private void PistolFire(Vector3 origin, Vector2 direction, Bullet bullet)
    {
        Bullet b = GameObject.Instantiate(bullet);
        b.transform.position = origin;
        b.SetDirection(direction);
    }
    private void ShotgunFire(Vector3 origin, Vector2 direction, Bullet bullet)
    {
        Bullet b = GameObject.Instantiate(bullet);
        b.transform.position = origin;
        b.SetDirection(direction);
    }
    private void SmgFire(Vector3 origin, Vector2 direction, Bullet bullet)
    {
        Bullet b = GameObject.Instantiate(bullet);
        b.transform.position = origin;
        b.SetDirection(direction);
    }
    private void CrossbpwFire(Vector3 origin, Vector2 direction, Bullet bullet)
    {
        Bullet b = GameObject.Instantiate(bullet);
        b.transform.position = origin;
        b.SetDirection(direction);
    }
}
