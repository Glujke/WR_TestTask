using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace WR.Configs
{
    [Serializable]
    public class MiniGameConfig
    {
        [SerializeField]
        public float MaxTimeGame = 1.5f;
        [SerializeField]
        public float SpeedValue = 2.0f;

        [SerializeField]
        public float MinValueWin = 0.43f;
        [SerializeField]
        public float MaxValueWin = 0.64f;

        [SerializeField]
        public float intervalRollTime = 0.43f;
        [SerializeField]
        public int chanceInPercent = 25;
    }
}
