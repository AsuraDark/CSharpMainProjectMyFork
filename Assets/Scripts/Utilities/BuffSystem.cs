using Model.Runtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffSystem 
{
    private Dictionary<Unit,List<Buff>> _buffs = new Dictionary<Unit,List<Buff>>();
    public Dictionary<Unit, List<Buff>> Buffs => _buffs;
    public void AddBuff(Unit unit, Buff buff)
    {
        if(!_buffs.ContainsKey(unit))
        {
            _buffs[unit] = new List<Buff>();
        }
        _buffs[unit].Add(buff);
    }

    public void Update()
    {
        foreach(var buff in _buffs)
        {
            for (int i = 0; i < buff.Value.Count; i++) 
            {
                buff.Value[i].DecreaseDuration(Time.deltaTime);
                if (buff.Value[i].Duration <= 0) 
                {
                    buff.Value.RemoveAt(i);
                }
            }
        }
    }
    public float GetAttackSpeedModifier(Unit unit)
    {
        float modifier = 1.0f;
        if(_buffs.ContainsKey(unit))
        {
            foreach(var buff in _buffs[unit])
            {
                modifier *= buff.AttackSpeedModifier;
            } 
        }
        return modifier;
    }
    public float GetMoveSpeedModifier(Unit unit)
    {
        float modifier = 1.0f;
        if (_buffs.ContainsKey(unit))
        {
            foreach (var buff in _buffs[unit])
            {
                modifier *= buff.MoveSpeedModifier;
            }
        }
        return modifier;
    }
    
}
