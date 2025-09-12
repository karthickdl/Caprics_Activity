
using TMPro;

namespace DLearners
{
    public class GameManagerBase : Singleton<GameManagerBase>
    {
        protected bool isInputUnLocked;


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
    }
}