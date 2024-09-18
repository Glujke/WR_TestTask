using System;
using UnityEngine;

namespace WR.Configs
{
    [Serializable]
    public class FlagConfig
    {
        [SerializeField]
        public float Radius = 3.0f;

        [SerializeField]
        public float MaxTimeToCapture = 5.0f;
        [SerializeField]
        public float MaxTimeToBlock = 5.0f;


        [SerializeField]
        public float minHorizontalPosition = 0;
        [SerializeField]
        public float maxHorizontalPosition = 79;

        [SerializeField]
        public float minVerticalPosition = 0;
        [SerializeField]
        public float maxVerticalPosition = 79;
    }
}