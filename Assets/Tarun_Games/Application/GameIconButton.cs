using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DLearnersApplication
{
    public class GameIconButton : MonoBehaviour
    {
        [SerializeField] Button button;
        [SerializeField] TextMeshProUGUI buttonText;
        [SerializeField] Image gameIconIMG;

        [SerializeField] int id;
        [SerializeField] string loadSceneName;
        public void InitGameIconButton(int _id, GamePFLocationData gamePFLocationData)
        {
            id = _id;
            buttonText.SetText(gamePFLocationData.gameName);
            gameIconIMG.sprite = gamePFLocationData.gameIcon;
            loadSceneName = gamePFLocationData.gameType.ToString();
            button.onClick.AddListener(() =>
            {
                OnButtonClick();
            });
        }

        private void OnButtonClick()
        {
            Debug.Log("OnButtonClick");
            ApplicationManager.Instance.OnOpenSelectedGame(id, loadSceneName);
            
        }
    }
}