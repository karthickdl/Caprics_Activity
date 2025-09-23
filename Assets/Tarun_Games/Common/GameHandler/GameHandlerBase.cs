using UnityEngine;

namespace DLearners
{
    public class GameHandlerBase : Singleton<GameHandlerBase>
    {
        public bool isTesting;

        protected override void Awake()
        {
            base.Awake();
        }

        public virtual void InitGameHandlerImmersiveGame()
        {

        }

        public virtual void gg()
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