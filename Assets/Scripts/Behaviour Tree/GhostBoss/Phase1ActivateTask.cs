using BehaviourTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Phase1ActivateTask : Node
{
    private Transform _boss;
    private Transform _shotPoint;
    private GameObject _bullet;
    private float _fadeoutTime;
    private float _inactiveTime;
    private float _fireRate;
    private float _fireRateMultiplier;
    private int _phase1HealthThreshHold;

    private Animator _anim;
    private float _activeCounter;
    private float _fadeCounter;
    private float _shotCounter;

    public Phase1ActivateTask(Transform boss, Transform shotPoint, GameObject bullet, float fadeoutTime, float inactiveTime, float fireRate, float fireRateMultiplier, int phase1HealthThreshHold)
    {
        _boss = boss;
        _bullet = bullet;
        _shotPoint = shotPoint;
        _fadeoutTime = fadeoutTime;
        _inactiveTime = inactiveTime;
        _fireRate = fireRate;
        _fireRateMultiplier = fireRateMultiplier;
        _anim = _boss.GetComponent<Animator>();
        _phase1HealthThreshHold = phase1HealthThreshHold;
    }

    public override NodeState Evaluate()
    {
        if (BossHealthController.instance.GetCurrentHealth() <= 0)
        {
            SetData(GhostBossBT.BattleEndTag, true);
            return NodeState.FAILURE;
        }

        if (BossHealthController.instance.GetCurrentHealth() < _phase1HealthThreshHold) return NodeState.FAILURE;

        _activeCounter = GetData<float>(GhostBossBT.ActiveCounterTag);

        if (_activeCounter > 0)
        {
            _activeCounter -= Time.deltaTime;
            SetData(GhostBossBT.ActiveCounterTag, _activeCounter);
            if (_activeCounter <= 0)
            {
                //_fadeCounter = _fadeoutTime;
                SetData(GhostBossBT.FadeCounterTag, _fadeoutTime);
                //_anim.SetTrigger("vanished");

            }
            Fire(_fireRateMultiplier);
            return NodeState.RUNNING;
        }
        return NodeState.FAILURE;
    }

    private void Fire(float multiplier)
    {
        _shotCounter = GetData<float>(GhostBossBT.ShotCounterTag);
        _shotCounter -= Time.deltaTime;
        if (_shotCounter <= 0)
        {
            _shotCounter = (1 / _fireRate) * (1 / multiplier);

            GameObject.Instantiate(_bullet, _shotPoint.position, Quaternion.identity);
        }
        SetData(GhostBossBT.ShotCounterTag, _shotCounter);
    }
}
