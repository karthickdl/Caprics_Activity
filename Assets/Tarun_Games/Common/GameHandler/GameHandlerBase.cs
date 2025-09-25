using UnityEngine;

namespace DLearners
{
    public class GameHandlerBase : Singleton<GameHandlerBase>
    {
        public bool isTesting;
        public PlatformType platformType;
        protected override void Awake()
        {
            base.Awake();
        }

        public virtual void InitGameHandlerImmersiveGame(DataSO _dataSO)
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

            dataSO = _dataSO;
        }


        public virtual void TriggerDemo()
        {
        }


        [Header("Sound SO")]
        public GameAudioDataSO gameAudioDataSO;
        [Header("Game Data")]
        public DataSO dataSO;

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
    }
}