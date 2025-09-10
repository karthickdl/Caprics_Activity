using UnityEngine;
using UnityEngine.UI;

public class CoverPage : MonoBehaviour
{
    [SerializeField] public Image bgIMG;
    [SerializeField] private Button startButton;
    [SerializeField] private Image startButtonIMG;

    public bool ggd;

    public Sprite gg;
    public void InitCoverPage()
    {
        bgIMG.sprite = gg;
        startButton.onClick.AddListener(() => { OnStartButton();});
    }

    private void OnStartButton()
    {
        ggd = true;
        Destroy(this.gameObject, 0.1f);
    }
}
