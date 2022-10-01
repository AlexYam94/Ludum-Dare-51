using BehaviourTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Phase2ActivateTask : Node
{
    private Transform _boss;
    private Transform _shotPoint;
    private GameObject _bullet;
    private float _fadeoutTime;
    private float _fireRate;
    private float _fireRateMultiplier;
    private int _HealthThreshHold;
    private float _fadeoutTimeMultiplier;
    private Collider2D _collider;
    private float _moveSpeed;
    Transform[] _spawnPoints;

    private Transform _targetPoint;
    private Animator _anim;
    private float _activeCounter;
    private float _shotCounter;
    private bool _targetSet;

    public Phase2ActivateTask(Transform boss, Transform shotPoint, GameObject bullet, float fadeoutTime, float fireRate, float fireRateMultiplier, int HealthThreshHold, float fadeoutTimeMultiplier, Collider2D collider, float moveSpeed, Transform[] spawnPoints)
    {
        _boss = boss;
        _bullet = bullet;
        _shotPoint = shotPoint;
        _fadeoutTime = fadeoutTime;
        _fireRate = fireRate;
        _fireRateMultiplier = fireRateMultiplier;
        _anim = _boss.GetComponent<Animator>();
        _HealthThreshHold = HealthThreshHold;
        _fadeoutTimeMultiplier = fadeoutTimeMultiplier;
        _collider = collider;
        _moveSpeed = moveSpeed;
        _spawnPoints = spawnPoints;
        _targetSet = false;
    }

    public override NodeState Evaluate()
    {
        if (BossHealthController.instance.GetCurrentHealth() <= 0)
        {
            SetData(GhostBossBT.BattleEndTag, true);
            return NodeState.FAILURE;
        }

        if (BossHealthController.instance.GetCurrentHealth() < _HealthThreshHold) return NodeState.FAILURE;

        if (_targetPoint == null)
        {
            _targetPoint = _boss;
            SetData(GhostBossBT.FadeCounterTag, _fadeoutTime);
            _anim.SetTrigger("vanished");
        }

        _activeCounter = GetData<float>(GhostBossBT.ActiveCounterTag);
        if (!_targetSet) {
            do
            {
                _targetPoint = _spawnPoints[Random.Range(0, _spawnPoints.Length)];
            } while (_spawnPoints.Length > 1 && _targetPoint.position == _boss.position);
            _targetSet = true;
        }

         if (Vector3.Distance(_boss.transform.position, _targetPoint.position) > .5f && _collider.enabled)
        {
            _boss.position = Vector3.MoveTowards(_boss.position, _targetPoint.position, _moveSpeed * Time.deltaTime);

            Fire(_fireRateMultiplier);
        }
        else if (_activeCounter > 0 && Vector3.Distance(_boss.transform.position, _targetPoint.position) <= .5f)
        {
            _activeCounter -= Time.deltaTime;
            SetData(GhostBossBT.ActiveCounterTag, _activeCounter);
            if (_activeCounter < 0)
            {
                SetData(GhostBossBT.FadeCounterTag, _fadeoutTime);
                //_anim.SetTrigger("vanished");
                _targetSet = false;
            }


            Fire(_fireRateMultiplier);
            return NodeState.SUCCESS;
            //anim.SetTrigger("vanished");
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
