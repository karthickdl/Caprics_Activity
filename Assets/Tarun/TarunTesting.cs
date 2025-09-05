using DLearners;
using UnityEngine;

public class TarunTesting : Singleton<TarunTesting>
{
    public GameAudioDataSO gameAudioDataSO;
    public IntroController introControllerPF;
    public CoverPage coverPagePF;
    public Transform canv;
    private void Start()
    {
        ShowIntro();
    }
    public void Update()
    {
    }

    private void ShowIntro()
    {
        Instantiate(introControllerPF, canv);
    }
}
