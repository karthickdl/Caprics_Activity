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

    public DataSO dataSO;
    public CoverPage coverPage;
    private void Start()
    {
        StartCoroutine(Test());
    }
    private IEnumerator Test()
    {
        DownloadManager.Instance.SetURLData(dataSO.GetURLData());
         HUDManager.Instance.SetHUDOnOff(false);
         IntroController cashIntroController = Instantiate(introControllerPF, canv);
         yield return new WaitForSeconds(cashIntroController.InitIntroController());        

         CoverPage cashCoverPage = Instantiate(coverPagePF, canv);
         coverPage = cashCoverPage;
         cashCoverPage.InitCoverPage(dataSO.GetCoverPageSprit());

        // RB_Runner_Main rB_Runner_Main = (RB_Runner_Main)RB_Runner_Main.Instance;
        // StartCoroutine(rB_Runner_Main.IN_CoverImage());

         yield return new WaitUntil(() => cashCoverPage.isDone);

        DemoController cashDemoController = Instantiate(demoControllerPF, canv);

        cashDemoController.InitDemoController(_demoControllerDataSO);

        yield return new WaitUntil(() => cashDemoController.isDone);

        GameManagerBase.Instance.SetGameOBJOnOff(true);
        GameManagerBase.Instance.InitGame();
        HUDManager.Instance.SetTapToPlayOnAndOff(true);
        HUDManager.Instance.SetHUDOnOff(true);
        HUDManager.Instance.InitHUD(dataSO);



        yield return null;
    }

    public void gg()
    {
        DemoController cashDemoController = Instantiate(demoControllerPF, canv);

        cashDemoController.InitDemoController(_demoControllerDataSO);
    }
    
}
