using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace WR.Game.Player
{
    public class PlayerNotifier : MonoBehaviour
    {
        private const float COEFFICIENT_TIME = 0.25f;
        [SerializeField]
        public TMP_Text message;

        public void SendMessageAllPlayers(string msg)
        {
            StartCoroutine(ShowMessageRoutine(msg));
        }

        public void ShowWinMessage(string msg)
        {
            StartCoroutine(ShowWinMessageRoutine(msg));
        }
        private IEnumerator ShowMessageRoutine(string message)
        {
            this.message.text = this.message.alpha != 1.0f ? this.message.text + message + "\n" : message + "\n";
            this.message.alpha = 1.0f;
            yield return StartCoroutine(HideTextRoutine());
            this.message.text = "";
        }
        private IEnumerator ShowWinMessageRoutine(string message)
        {
            this.message.text = message;
            this.message.alpha = 1.0f;
            yield return null;
            Time.timeScale = 0;
        }
        private IEnumerator HideTextRoutine()
        {
            while (message.alpha > 0)
            {
                message.alpha -= Time.deltaTime * COEFFICIENT_TIME;
                if (message.alpha <= 0)
                {
                    message.alpha = 0;
                    break;
                }
                yield return null;
            }
            yield return null;
        }
    }
}
