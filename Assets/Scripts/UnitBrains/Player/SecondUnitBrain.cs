using System.Collections.Generic;
using System.Linq;
using Codice.CM.Triggers;
using Model;
using Model.Runtime.Projectiles;
using UnityEngine;
using Utilities;


namespace UnitBrains.Player
{
    public class SecondUnitBrain : DefaultPlayerUnitBrain
    {
        public override string TargetUnitName => "Cobra Commando";
        private const float OverheatTemperature = 3f;
        private const float OverheatCooldown = 2f;
        private float _temperature = 0f;
        private float _cooldownTime = 0f;
        private bool _overheated;

        private List<Vector2Int> _outOfReachTargets = new List<Vector2Int>();
        protected override void GenerateProjectiles(Vector2Int forTarget, List<BaseProjectile> intoList)
        {
            float overheatTemperature = OverheatTemperature;
            ///////////////////////////////////////
            // Homework 1.3 (1st block, 3rd module)
            ///////////////////////////////////////
            int temp = GetTemperature();
            if (temp < overheatTemperature)
            {
                IncreaseTemperature();
                for (int i = 0; i <= temp; i++)
                {
                    var projectile = CreateProjectile(forTarget);
                    AddProjectileToList(projectile, intoList);
                }
            }
            ///////////////////////////////////////
        }

        public override Vector2Int GetNextStep()
        {
            if(_outOfReachTargets.Count == 0 || IsTargetInRange(_outOfReachTargets[0]))
            {
                return unit.Pos;
            }
            
            return IsTargetInRange(_outOfReachTargets[0]) ? unit.Pos : unit.Pos.CalcNextStepTowards(_outOfReachTargets[0]);
        }

        protected override List<Vector2Int> SelectTargets()
        {
            ///////////////////////////////////////
            // Homework 1.4 (1st block, 4rd module)
            ///////////////////////////////////////
            List<Vector2Int> result = new List<Vector2Int>(GetAllTargets());
            _outOfReachTargets.Clear();
            if (result.Count > 0)
            {
                Vector2Int target = new Vector2Int();

                float MinValue = float.MaxValue;
                float j = 0;



                foreach (Vector2Int i in result)
                {
                    j = DistanceToOwnBase(i);
                    if (j < MinValue)
                    {
                        MinValue = j;
                        target = i;
                    }
                }
                

                if (IsTargetInRange(target))
                {
                    result.Clear();
                    result.Add(target);
                }
                else
                {
                    result.Clear();
                    _outOfReachTargets.Add(target);
                }



            }
            else
            {
                if (IsPlayerUnitBrain)
                {
                    _outOfReachTargets.Add(runtimeModel.RoMap.Bases[RuntimeModel.BotPlayerId]);
                }
                else
                {
                    _outOfReachTargets.Add(runtimeModel.RoMap.Bases[RuntimeModel.PlayerId]);
                }
                
            }



            return result;
            ///////////////////////////////////////
        }

        public override void Update(float deltaTime, float time)
        {
            if (_overheated)
            {
                _cooldownTime += Time.deltaTime;
                float t = _cooldownTime / (OverheatCooldown / 10);
                _temperature = Mathf.Lerp(OverheatTemperature, 0, t);
                if (t >= 1)
                {
                    _cooldownTime = 0;
                    _overheated = false;
                }
            }
        }

        private int GetTemperature()
        {
            if (_overheated) return (int)OverheatTemperature;
            else return (int)_temperature;
        }

        private void IncreaseTemperature()
        {
            _temperature += 1f;
            if (_temperature >= OverheatTemperature) _overheated = true;
        }
    }
}