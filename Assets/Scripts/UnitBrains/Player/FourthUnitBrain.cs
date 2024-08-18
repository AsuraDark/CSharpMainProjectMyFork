using System.Collections.Generic;
using System.Linq;
using Codice.CM.Triggers;
using Model;
using Model.Runtime;
using Model.Runtime.Projectiles;
using UnitBrains.Pathfinding;
using UnityEngine;
using Utilities;
using View;


namespace UnitBrains.Player
{
    public class FourthUnitBrain : DefaultPlayerUnitBrain
    {
        public override string TargetUnitName => "Buffer";
        private float _cooldownTime = 0f;
        private float timeBetweenBuffs = 0.7f;
        
        VFXView _vfx = ServiceLocator.Get<VFXView>();
        BuffSystem _buffSystem = ServiceLocator.Get<BuffSystem>();
       
        private List<Vector2Int> _outOfReachTargets = new List<Vector2Int>();
        protected override void GenerateProjectiles(Vector2Int forTarget, List<BaseProjectile> intoList)
        {



        }

        public override Vector2Int GetNextStep()
        {
            if (timeBetweenBuffs - _cooldownTime <= 0.5f) 
            {
                return unit.Pos;
            }


            var target = runtimeModel.RoMap.Bases[
                IsPlayerUnitBrain ? RuntimeModel.BotPlayerId : RuntimeModel.PlayerId];

            _activePath = new AStarUnitPath(runtimeModel, unit.Pos, target);
            return _activePath.GetNextStepFrom(unit.Pos);
        }

        

        public override void Update(float deltaTime, float time)
        {
            _cooldownTime += Time.deltaTime;
            Debug.Log(_cooldownTime);
            if(_cooldownTime > timeBetweenBuffs)
            {
                foreach (Unit target in runtimeModel.RoPlayerUnits)
                {
                    if (unit == target)
                        continue;
                    if(_buffSystem.Buffs.ContainsKey(target))
                        continue;
                    if(HasPlayerTargetInRange(target))
                    {
                        _buffSystem.AddBuff(target, new Buff(1.0f, 10, 1));
                        _vfx.PlayVFX(target.Pos, VFXView.VFXType.BuffApplied);
                        break;
                    }
                    
                }
                _cooldownTime = 0;
            }

        }

        protected bool HasPlayerTargetInRange(Unit target)
        {
            var attackRangeSqr = unit.Config.AttackRange * unit.Config.AttackRange;

            var diff = target.Pos - unit.Pos;
            if (diff.sqrMagnitude < attackRangeSqr)
                return true;

            return false;
        }



    }
}
