using System.Collections;
using UnityEngine;

namespace DLearners
{
    public class GameHandlerImmersiveGame : GameHandlerBase
    {
        public PlatformType platformType;
        [Header ("Sound SO")]
        public GameAudioDataSO gameAudioDataSO;

        [Header("Splash Screen")]
        [SerializeField] private IntroController introControllerPF;

        [Header("Cover Page")]
        [SerializeField] private CoverPage coverPagePF;

        [Header("Demo Page")]
        [SerializeField] private DemoController demoControllerPF;
        [SerializeField] private DemoControllerDataSO _demoControllerDataSO;

        [Header("Game Data")]
        public DataSO dataSO;

        [SerializeField] private Transform canv;
        private void Start()
        {
            Init();
            StartCoroutine(Test());
        }
        private IEnumerator Test()
        {
            DownloadManager.Instance.SetURLData(dataSO.GetURLData());
            HUDManager.Instance.SetHUDOnOff(false);
            IntroController cashIntroController = Instantiate(introControllerPF, canv);
            yield return new WaitForSeconds(cashIntroController.InitIntroController());

            CoverPage cashCoverPage = Instantiate(coverPagePF, canv);
            cashCoverPage.InitCoverPage(dataSO.GetCoverPageSprit());

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


        


        private void Init()
        {
            Application.ExternalEval("OnAppReady()");
#if UNITY_ANDROID || UNITY_IOS
            platformType = PlatformType.MOBILE;
#elif UNITY_WEBGL
             platformType = PlatformType.WEB;
#endif


            switch (platformType)
            {
                case PlatformType.WEB:
                    break;
                case PlatformType.MOBILE:
                    break;
                default:
                    break;
            }
        }
        [Header("ID")]
        public string STR_IDjson;//
        public string STR_childID;
        public string STR_GameID;
        public string STR_responseSerial;//

        [Header("SCORE")]
        public int I_TotalPoints;
        public int I_TotalQuestions;
        public int I_correctPoints => dataSO.GetCorrectAnswerPoint();

       [Header("MODE")]
        public string mode;
        [Header("PREVIEW MODE")]
        public string STR_previewJsonAPI;
        public void JS_getID(string val)
        {
            STR_IDjson = val;
            Debug.Log("json string from javascript : " + val);
            MyJSON myjson = new MyJSON();
            myjson.FetchIDs();
            DLearners.GameHandlerImmersiveGame.Instance.mode = "live";
        }

        public void JS_getMode(string val)
        {
            DLearners.GameHandlerImmersiveGame.Instance.mode = "preview";
            STR_previewJsonAPI = val;
        }
    }
    public enum PlatformType
    {
        WEB,
        MOBILE
    }
}

