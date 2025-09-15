using UnityEngine;
using UnityEngine.UI;

public class CoverPage : MonoBehaviour
{
    [SerializeField] public Image bgIMG;
    [SerializeField] private Button startButton;
    [SerializeField] private Image startButtonIMG;

    public bool isDone;

    public void InitCoverPage(Sprite coverPageSprit)
    {
        bgIMG.sprite = coverPageSprit;
        startButton.onClick.AddListener(() => { OnStartButton();});
        OnButtonAnim();
    }

    private void OnStartButton()
    {
        isDone = true;
        Destroy(this.gameObject, 0.1f);
    }

    private void OnButtonAnim()
    {
        Fading.OnBreathingFX(startButton.transform,1.25f,0.35f);
    }
}
