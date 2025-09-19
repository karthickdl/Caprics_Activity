using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameIconButton : MonoBehaviour
{
    [SerializeField] Button button;
    [SerializeField] TextMeshProUGUI buttonText;

    [SerializeField] int id;
    private void Start()
    {
        button.onClick.AddListener(() =>
        {
            OnButtonClick();
        });
    }
    public void InitGameIconButton(int _id)
    {
        id = _id;
    }

    private void OnButtonClick()
    {
        Debug.Log("OnButtonClick");

        Application.LoadLevel(1);

        ApplicationManager.Instance.tarun(id);
    }
}
