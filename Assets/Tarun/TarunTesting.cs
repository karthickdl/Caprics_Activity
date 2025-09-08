using DLearners;
using System.Collections;
using UnityEngine;

public class TarunTesting : Singleton<TarunTesting>
{
    public GameAudioDataSO gameAudioDataSO;
    [SerializeField] private IntroController introControllerPF;
    [SerializeField] private CoverPage coverPagePF;
    [SerializeField] private DemoController demoControllerPF;
    [SerializeField] private DemoControllerDataSO _demoControllerDataSO;
    [SerializeField] private Transform canv;
    private void Start()
    {
        StartCoroutine(Test());

        Application.ExternalEval("OnAppReady()");
    }
    private IEnumerator Test()
    {
        IntroController cashIntroController = Instantiate(introControllerPF, canv);
        yield return new WaitForSeconds(cashIntroController.InitIntroController());

        CoverPage cashCoverPage = Instantiate(coverPagePF, canv);

        cashCoverPage.InitCoverPage();

        yield return new WaitUntil(() => cashCoverPage.ggd);

        DemoController cashDemoController = Instantiate(demoControllerPF, canv);

        cashDemoController.InitDemoController(_demoControllerDataSO);


        yield return null;
    }
}
