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

        protected int currentQuestionID;
        protected int I_Collect_count;
        protected override void Awake()
        {
            base.Awake();            
        }

        /// <summary>
        /// This will Trigger from tap to play screen.
        /// </summary>
        public virtual void OnPlayButton()
        {
        }

        /// <summary>
        /// We are initialising the game after all the tutorial thing is completed. 
        /// </summary>
        public virtual void InitGame()
        {
        }

        /// <summary>
        /// Seting up level data from SO (per level)
        /// </summary>
        protected virtual void GetSetCurrentLevelData()
        {
            currentData = new Data();
            currentInstructionData = new InstructionData();
            DataSO cashDataSO = GameHandlerImmersiveGame.Instance.dataSO;
            GameHandlerImmersiveGame.Instance.I_TotalQuestions = cashDataSO.datas.Count;
            currentDifficultyLevelType = cashDataSO.difficultyLevelType;

            currentData = cashDataSO.GetData(currentQuestionID);
            currentOptionCount = currentData.options.Count;
            currentInstructionData = cashDataSO.instructionData;

            STR_currentQuestionAnswer = currentData.correctOptions;
        }

        /// <summary>
        /// For Checking the answer if it is right or wrong.
        /// </summary>
        public virtual void CheckAnswer()
        {
        }
        public virtual void CheckAnswer(Transform transform)
        {
        }
        public string GetCurrentQuestionAnswer()
        {
            return STR_currentQuestionAnswer;
        }

        public virtual void UpdateQuestion()
        {

        }

        public virtual void THI_WrongEffect()
        {
        }

        public virtual void THI_Correct()
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
            GameHandlerImmersiveGame.Instance.I_TotalPoints = GameHandlerImmersiveGame.Instance.dataSO.GetCorrectAnswerPoint();
            VaultPopUpsManager.Instance.ShowPopup(NormalPopUpTypes.LevelCompletePOPUP,null);
        }

        
    }
}