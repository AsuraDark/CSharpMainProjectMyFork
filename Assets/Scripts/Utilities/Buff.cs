using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.YamlDotNet.Core.Tokens;
using UnityEngine;

public class Buff 
{
    private float _duration;
    private float _attackSpeedModifier;
    private float _moveSpeedModifier;
    
    public Buff(float duration, float attackSpeedModifier, float moveSpeedModifier)
    {
        _duration = duration;
        _attackSpeedModifier = attackSpeedModifier;
        _moveSpeedModifier = moveSpeedModifier;
    }

    public float Duration { get; private set; }
    public float AttackSpeedModifier { get; private set; }
    public float MoveSpeedModifier { get; private set; }
    public void DecreaseDuration(float time)
    {
        _duration -= time;
        if(_duration <= 0)
        {
            ActionAfterDuration();
        }
    }

    public virtual void ActionAfterDuration()
    {

    }
}
