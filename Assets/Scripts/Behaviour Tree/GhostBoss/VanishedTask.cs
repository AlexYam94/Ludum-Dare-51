using BehaviourTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VanishedTask : Node
{
    private Transform _boss;
    private Transform[] _spawnPoints;

    private float _activeTime;
    private float _fireRate;
    private float _inactiveCounter;
    public VanishedTask(Transform boss, Transform[] spawnPoints, float activeTime, float fireRate)
    {
        _boss = boss;
        _spawnPoints = spawnPoints;
        _activeTime = activeTime;
        _fireRate = fireRate;
    }

    public override NodeState Evaluate()
    {
        if (BossHealthController.instance.GetCurrentHealth() <= 0)
        {
            SetData(GhostBossBT.BattleEndTag, true);
            return NodeState.FAILURE;
        }
        _inactiveCounter = GetData<float>(GhostBossBT.InactiveCounterTag);

        if (_inactiveCounter > 0)
        {
            _inactiveCounter -= Time.deltaTime;
            SetData(GhostBossBT.InactiveCounterTag, _inactiveCounter);
            if (_inactiveCounter <= 0)
            {
                Vector3 lastPos = _boss.position;
                do
                {
                    _boss.position = _spawnPoints[Random.Range(0, _spawnPoints.Length)].position;
                } while (_spawnPoints.Length > 1 && lastPos == _boss.position);
                _boss.gameObject.SetActive(true);

                SetData(GhostBossBT.ActiveCounterTag, _activeTime);
                SetData(GhostBossBT.ShotCounterTag, 1 / _fireRate) ;

                return NodeState.SUCCESS;
            }
            return NodeState.RUNNING;
        }
        return NodeState.FAILURE;
    }
}
