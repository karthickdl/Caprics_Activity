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

        protected int currentWrongAnsCount;
        protected int[] wrongAnsLifeCounts = { 3, 2 };


        
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
    }
}