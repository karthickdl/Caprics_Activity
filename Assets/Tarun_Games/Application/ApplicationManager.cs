using DLearners;
using UnityEngine;

namespace DLearnersApplication
{
    public class ApplicationManager : Singleton<ApplicationManager>
    {
        public GamePFLocationSO gamePFLocationSO;

        public GameHandlerBase currentOpenGame;

        public void OnOpenSelectedGame(int ID)
        {
            VaultPopUpsManager.Instance.ShowTransition(TransitionType.Fade2);
            SetScreenOrientation(ScreenOrientation.LandscapeLeft);

            currentOpenGame = Instantiate(GetGame(ID));
            currentOpenGame.InitGameHandlerImmersiveGame();
            ApplicationUIManager.Instance.SetApplicationUIOnOff(false);
        }
        public void OnCloseGameAndBackToHomePage()
        {
            VaultPopUpsManager.Instance.ShowTransition(TransitionType.Fade2);
            SetScreenOrientation(ScreenOrientation.Portrait);
            Destroy(currentOpenGame.gameObject);
            currentOpenGame = null;

            DLearnersAudioManager.Instance.CleanUp();
            Time.timeScale = 1;

            ApplicationUIManager.Instance.SetApplicationUIOnOff(true);
        }

        private GameHandlerBase GetGame(int gameID)
        {
            GameObject currentLevelGObj = Resources.Load<GameHandlerBase>(gamePFLocationSO.fileLocation + gamePFLocationSO.gamePFLocationDatas[gameID].pFLocation).gameObject;
            return currentLevelGObj.GetComponent<GameHandlerBase>();
        }

        public void SetScreenOrientation(ScreenOrientation screenOrientation)
        {
            Screen.orientation = screenOrientation;
        }
    }
}