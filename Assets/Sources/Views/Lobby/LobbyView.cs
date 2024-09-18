using Mirror;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace WR.UI
{
    public class LobbyView : MonoBehaviour
    {
        public TMP_Text playerId;
        public LobbyPlayerView[] players;
        public Button disconnect;
    }
}
