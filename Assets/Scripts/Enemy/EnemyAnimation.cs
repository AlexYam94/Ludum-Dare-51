using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
    [SerializeField] Animator _anim;


    public void Move(float value)
    {
        _anim.SetFloat("speed", value);
    }
}
