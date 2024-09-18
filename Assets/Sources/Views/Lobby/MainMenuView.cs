using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace WR.UI
{
    public class MainMenuView : MonoBehaviour
    {
        [SerializeField]
        public Button host;
        [SerializeField]
        public Button client;
        [SerializeField]
        public Button server;

        [SerializeField]
        public TMP_InputField ip;
        [SerializeField]
        public TMP_InputField port;

        [SerializeField]
        public TMP_Text errorMessage;

    }
}
