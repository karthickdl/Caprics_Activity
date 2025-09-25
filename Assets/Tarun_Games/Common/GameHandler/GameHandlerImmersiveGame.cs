using System.Collections;
using UnityEngine;

namespace DLearners
{
    public class GameHandlerImmersiveGame : GameHandlerBase
    {
        public bool isFastingTesting;
        
        

        [Header("Splash Screen")]
        [SerializeField] private IntroController introControllerPF;

        [Header("Cover Page")]
        [SerializeField] private CoverPage coverPagePF;

        [Header("Demo Page")]
        [SerializeField] private DemoController demoControllerPF;
        [SerializeField] private DemoControllerDataSO _demoControllerDataSO;

       

        [SerializeField] private Transform canv;

        

        //#if UNITYEDITOR
        private void Start()
        {
           if (isTesting)
            {
                ///InitGameHandlerImmersiveGame();      
            }
       }
        //#endif
        public override void InitGameHandlerImmersiveGame(DataSO _dataSO)
        {
            base.InitGameHandlerImmersiveGame(_dataSO);
          


            StartCoroutine(Test());
        }


        private IEnumerator Test()
        {
            //DownloadManager.Instance.SetURLData(dataSO.GetURLData());


            HUDManager.Instance.SetHUDOnOff(false);
            if (!isFastingTesting)
            {
                IntroController cashIntroController = Instantiate(introControllerPF, canv);
                cashIntroController.InitIntroController();
                yield return new WaitUntil(() => cashIntroController.isDone);

                CoverPage cashCoverPage = Instantiate(coverPagePF, canv);
                cashCoverPage.InitCoverPage(dataSO.GetCoverPageSprit());

                yield return new WaitUntil(() => cashCoverPage.isDone);

                DemoController cashDemoController = Instantiate(demoControllerPF, canv);

                cashDemoController.InitDemoController(_demoControllerDataSO);

                yield return new WaitUntil(() => cashDemoController.isDone);
            }
            GameManagerBase.Instance.SetGameOBJOnOff(true);
            GameManagerBase.Instance.InitGame();
            HUDManager.Instance.SetTapToPlayOnAndOff(true);
            HUDManager.Instance.SetHUDOnOff(true);
            HUDManager.Instance.InitHUD(dataSO);



            yield return null;
        }

        public override void TriggerDemo()
        {
            base.TriggerDemo();
            DemoController cashDemoController = Instantiate(demoControllerPF, canv);
            cashDemoController.InitDemoController(_demoControllerDataSO);
        }





        
        
        public void JS_getID(string val)
        {
            STR_IDjson = val;
            Debug.Log("json string from javascript : " + val);
            MyJSON myjson = new MyJSON();
            myjson.FetchIDs();
            mode = "live";
        }

        public void JS_getMode(string val)
        {
            mode = "preview";
            STR_previewJsonAPI = val;
        }
    }
    public enum PlatformType
    {
        WEB,
        MOBILE
    }
}

