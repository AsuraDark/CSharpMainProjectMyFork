using Model;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.YamlDotNet.Core.Tokens;
using UnityEngine;
using Utilities;

namespace UnitBrains.Pathfinding
{
    public class AStarUnitPath : BaseUnitPath
    {
        public int Cost = 10;
        private int[] dx = { -1, 0, 1, 0 };
        private int[] dy = { 0, 1, 0, -1 };
        public AStarUnitPath(IReadOnlyRuntimeModel runtimeModel, Vector2Int startPoint, Vector2Int endPoint) : base(runtimeModel, startPoint, endPoint)
        {
        }
        protected override void Calculate()
        {
            var currentPoint = startPoint;
            var result = new List<Vector2Int> { startPoint };

            while (currentPoint != endPoint)
            {
                var nextStep = CalcNextStepTowards(currentPoint, endPoint);
                var hasLoop = result.Contains(nextStep);
                result.Add(nextStep);
                if (hasLoop)
                    break;
                currentPoint = nextStep;
            }

            path = result.ToArray();
        }

        private Vector2Int CalcNextStepTowards(Vector2Int fromPos, Vector2Int toPos)
        {


            Vector2Int current = new Vector2Int();
            List<Vector2Int> steps = new List<Vector2Int>();

            for (int i = 0; i < 4; i++)
            {
                Vector2Int newStep = new Vector2Int(fromPos.x + dx[i], fromPos.y + dy[i]);

                if (runtimeModel.IsTileWalkable(newStep))
                {
                    steps.Add(newStep);
                }
            }
            foreach (var step in steps)
            {
                if (CalculateValue(step, toPos) < CalculateValue(fromPos, toPos))
                {
                    current = step;
                }


            }


            return current;

        }
        public int CalculateEstimate(Vector2Int current, Vector2Int target)
        {
            return Math.Abs(current.x - target.x) + Math.Abs(current.y - target.y);
        }

        public int CalculateValue(Vector2Int current, Vector2Int target)
        {
            return Cost + CalculateEstimate(current, target);
        }
    }
}
