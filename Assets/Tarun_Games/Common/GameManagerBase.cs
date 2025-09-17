using UnityEngine;
using UnityEngine.SceneManagement;

namespace DLearners
{
    public class GameManagerBase : Singleton<GameManagerBase>
    {
        protected bool isInputUnLocked;

        [SerializeField] protected GameObject mainGameOBJ;
        protected Data currentData = new Data();
        protected int currentOptionCount;
        protected DifficultyLevelType currentDifficultyLevelType;
        protected InstructionData currentInstructionData = new InstructionData();
        protected UserData currentUserData = new UserData();


        protected string STR_currentQuestionAnswer;
        protected string STR_currentSelectedAnswer;

        protected int currentWrongAnsCount;
        protected int[] wrongAnsLifeCounts = { 3, 2 };

        protected int I_currentQuestionCount;
        protected int I_Collect_count;
        protected override void Awake()
        {
            base.Awake();            
        }
        

        public virtual void UpdateQuestion()
        {

        }

        public virtual void THI_WrongEffect()
        {
        }

        public virtual void OnPlayButton()
        {
        }

        public virtual void InitGame()
        {
        }

        public virtual void SetGameOBJOnOff(bool isOn)
        {
            mainGameOBJ.SetActive(isOn);
        }

        public virtual void OnPlayAgainButton()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public virtual void OnLevelCompleted()
        {
            DLearners.GameHandlerImmersiveGame.Instance.I_TotalPoints = GameHandlerImmersiveGame.Instance.dataSO.GetCorrectAnswerPoint();
            VaultPopUpsManager.Instance.ShowPopup(NormalPopUpTypes.LevelCompletePOPUP,null);
        }

        /// <summary>
        /// Seting up level data from SO (per level)
        /// </summary>
        protected virtual void Tarun()
        {
            currentData = new Data();
            currentInstructionData = new InstructionData();
            DataSO cashDataSO = GameHandlerImmersiveGame.Instance.dataSO;
            currentDifficultyLevelType = cashDataSO.difficultyLevelType;

            currentData = cashDataSO.GetData(I_currentQuestionCount);
            currentOptionCount = currentData.options.Count;
            currentInstructionData = cashDataSO.instructionData;

            STR_currentQuestionAnswer = currentData.correctOptions;
        }
    }
}