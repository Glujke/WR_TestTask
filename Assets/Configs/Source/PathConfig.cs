using System;
using UnityEngine;



namespace WR.Configs
{
    [Serializable]
    public class PathConfig
    {
        [SerializeField]
        public string PathToInputCanvas;
        [SerializeField]
        public string PathToFlagPrefab;
        [SerializeField]
        public string PathToMiniGamePrefab;
        [SerializeField]
        public string PathToCaptureTimerPrefab;
        [SerializeField]
        public string PathToBlockTimerPrefab;
        [SerializeField]
        public string PathToChatPrefab;

    }
}
