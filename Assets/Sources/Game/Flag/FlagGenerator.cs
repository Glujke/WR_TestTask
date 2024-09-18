using System.Collections.Generic;
using UnityEngine;
using WR.Configs;
using WR.Game.Interfaces;

namespace WR.Game.Flag
{
    public class FlagGenerator : IFlagPositionGenerating
    {
        private readonly FlagConfig flagConfig;

        private List<Vector3> vector3s;
        private List<Vector3> createdObjects;

        private float radiusFlag;
        private Vector3 startPos;
        private Vector3 endPos;

        public FlagGenerator(FlagConfig flagConfig)
        {
            this.flagConfig = flagConfig;   
            startPos = new Vector3(flagConfig.minHorizontalPosition, flagConfig.minVerticalPosition);
            endPos = new Vector3(flagConfig.maxHorizontalPosition, flagConfig.maxVerticalPosition);
            radiusFlag = flagConfig.Radius;
            Initialize();
        }

        private void Initialize()
        {
            vector3s = new List<Vector3>();
            createdObjects = new List<Vector3>();
            for (var x = flagConfig.minHorizontalPosition + radiusFlag; x < flagConfig.maxHorizontalPosition - radiusFlag; x += radiusFlag * 2)
            {
                for (var z = flagConfig.minVerticalPosition + radiusFlag; z < flagConfig.maxVerticalPosition - radiusFlag; z += radiusFlag * 2)
                {
                    vector3s.Add(new Vector3(x, 0.5f, z));
                }
            }
        }

        public Vector3 GetRandomPosition()
        {
            var random = Random.Range(0, vector3s.Count);
            if (createdObjects.Contains(vector3s[random]))
            {
                random++;
                if (random > vector3s.Count) random = 0;
            }
            createdObjects.Add(vector3s[random]);
            return vector3s[random];
        }
    }
}
