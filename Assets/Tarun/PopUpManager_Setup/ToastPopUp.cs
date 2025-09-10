using TMPro;
using UnityEngine;


public class ToastPopUp : MonoBehaviour
{
    public TextMeshProUGUI flyPopUpsText;
    public RectTransform panelTransform;

    public void SetText(string text)
    {
        flyPopUpsText.text = text;
    }
}