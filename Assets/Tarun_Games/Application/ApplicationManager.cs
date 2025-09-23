using DLearners;
using System.Collections;
using UnityEngine;

namespace DLearnersApplication
{
    public class ApplicationManager : Singleton<ApplicationManager>
    {
        public GamePFLocationSO gamePFLocationSO;

        public GameHandlerBase currentOpenGame;

        public void OnOpenSelectedGame(int ID, string sceneNameToLoad)
        {
            
            SetScreenOrientation(ScreenOrientation.LandscapeLeft);

            StartCoroutine(LoadScene(sceneNameToLoad));
            currentOpenGame = Instantiate(GetGame(ID));
            currentOpenGame.InitGameHandlerImmersiveGame();
            ApplicationUIManager.Instance.SetApplicationUIOnOff(false);
        }
        public void OnCloseGameAndBackToHomePage(string sceneNameToLoad)
        {
            SetScreenOrientation(ScreenOrientation.Portrait);
            Destroy(currentOpenGame.gameObject);
            currentOpenGame = null;

            DLearnersAudioManager.Instance.CleanUp();
            Time.timeScale = 1;

            ApplicationUIManager.Instance.SetApplicationUIOnOff(true);

            StartCoroutine(LoadScene("Application_Menu"));
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

        public IEnumerator LoadScene(string sceneNameToLoad)
        {
            VaultPopUpsManager.Instance.ShowTransition(TransitionType.Fade2);

            LoadScene(sceneNameToLoad);

            /* AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneNameToLoad);
             asyncOperation.allowSceneActivation = false;
             while (!asyncOperation.isDone)
             {
                 if (asyncOperation.progress >= 1f)
                 {
                     asyncOperation.allowSceneActivation = true;
                     // SaveDataHandler.Instance.GameSceneLoaded = true;
                 }
                 yield return null;
             }*/
            yield return null;
        }
    }
}