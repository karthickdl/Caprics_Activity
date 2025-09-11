
namespace DLearners
{
    public class GameManagerBase : Singleton<GameManagerBase>
    {
        protected bool isInputUnLocked;


        protected Data currentData = new Data();
        protected int currentOptionCount;
        protected InstructionData currentInstructionData = new InstructionData();
        protected UserData currentUserData = new UserData();
        protected override void Awake()
        {
            base.Awake();            
        }

        public virtual void UpdateQuestion()
        {

        }
    }
}