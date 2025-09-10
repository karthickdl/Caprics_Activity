using DLearners;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;

public class TarunTesting : Singleton<TarunTesting>
{
    public GameAudioDataSO gameAudioDataSO;
    [SerializeField] private IntroController introControllerPF;
    [SerializeField] private CoverPage coverPagePF;
    [SerializeField] private DemoController demoControllerPF;
    [SerializeField] private DemoControllerDataSO _demoControllerDataSO;

    [SerializeField] private Transform canv;


    public CoverPage coverPage;
    private void Start()
    {
        StartCoroutine(Test());
    }
    private IEnumerator Test()
    {
        IntroController cashIntroController = Instantiate(introControllerPF, canv);
        yield return new WaitForSeconds(cashIntroController.InitIntroController());

        CoverPage cashCoverPage = Instantiate(coverPagePF, canv);
        coverPage = cashCoverPage;
        cashCoverPage.InitCoverPage();

        RB_Runner_Main rB_Runner_Main = (RB_Runner_Main)RB_Runner_Main.Instance;
        StartCoroutine(rB_Runner_Main.IN_CoverImage());
        
        yield return new WaitUntil(() => cashCoverPage.ggd);
        
        DemoController cashDemoController = Instantiate(demoControllerPF, canv);

        cashDemoController.InitDemoController(_demoControllerDataSO);


        yield return null;
    }



    public DataSO dataSO;


    public void test2()
    {
        
    }
}
