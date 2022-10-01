using BehaviourTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeOutTask : Node
{
    private float _fadeCounter;
    private float _inactiveTime;
    private Transform _boss;

    private Animator _anim;
    private bool _vanished;

    public FadeOutTask(float inactiveTime, Transform boss)
    {
        _inactiveTime = inactiveTime;
        _boss = boss;
        _anim = _boss.GetComponent<Animator>();
        _vanished = false;
    }

    public override NodeState Evaluate()
    {
        _fadeCounter = GetData<float>(GhostBossBT.FadeCounterTag);
        if (_fadeCounter > 0)
        {
            if (!_vanished)
            {
                _anim.SetTrigger("vanished");
            }
            else
            {
                _vanished = false;
            }
            _fadeCounter -= Time.deltaTime;
            SetData(GhostBossBT.FadeCounterTag, _fadeCounter);
            if (_fadeCounter <= 0)
            {
                _vanished = true;
                _boss.gameObject.SetActive(false);
                SetData(GhostBossBT.InactiveCounterTag, _inactiveTime);
                //_inactiveCounter = _inactiveTime;
                return NodeState.SUCCESS;
            }
            return NodeState.RUNNING;
        }
        return NodeState.FAILURE;
    }
}
