using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DLearnersApplication
{
    public class GameIconButton : MonoBehaviour
    {
        [SerializeField] Button button;
        [SerializeField] TextMeshProUGUI buttonText;

        [SerializeField] int id;

        public void InitGameIconButton(int _id)
        {
            id = _id;
            button.onClick.AddListener(() =>
            {
                OnButtonClick();
            });
        }

        private void OnButtonClick()
        {
            Debug.Log("OnButtonClick");
            ApplicationManager.Instance.OnOpenSelectedGame(id);
        }
    }
}