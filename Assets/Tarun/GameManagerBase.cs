
namespace DLearners
{
    public class GameManagerBase : Singleton<GameManagerBase>
    {
        public string gg;
        protected override void Awake()
        {
            base.Awake();
        }

        public virtual void THI_ShowQuestion()
        {

        }
    }
}