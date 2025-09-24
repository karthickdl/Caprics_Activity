using DLearners;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

namespace DLearnersApplication
{
    public class ApplicationManager : Singleton<ApplicationManager>
    {
        


        public void OnCloseGameAndBackToHomePage(string sceneNameToLoad)
        {
            SetScreenOrientation(ScreenOrientation.Portrait);
            Destroy(currentOpenGame.gameObject);
            currentOpenGame = null;

            DLearnersAudioManager.Instance.CleanUp();
            Time.timeScale = 1;


            // StartCoroutine(LoadScene("Application_Menu"));
        }




        [SerializeField] private GamePFLocationSO gamePFLocationSO;
        [SerializeField] private GameHandlerBase currentOpenGame;
        [SerializeField] private bool isOfflineTesting;
        [SerializeField] private string json;
        [SerializeField] private string jsonUrl;
        [SerializeField] private GameLaunchData currentGameLaunchData;
        [SerializeField] private string sceneNameToLoad;


        bool dataReady;
        private IEnumerator Start()
        {
            if (isOfflineTesting)
            {
                GetOfflineData();
            }
            else
            {
                yield return StartCoroutine(GetJsonData(jsonUrl));
            }
            yield return new WaitUntil(() => dataReady); // resumes only when ready

            switch (currentGameLaunchData.gameType)
            {
                case GameType.LWS:
                    sceneNameToLoad = "";
                    break;
                case GameType.Game:
                    sceneNameToLoad = "Game";
                    break;
                default:
                    break;
            }

            //VaultDatabaseManager.Instance.Initialize();
#if UNITY_ANDROID
#endif


            // AudioManager.Instance.Initialize();

            StartCoroutine(LoadingMenuManager.Instance.LoadScene(sceneNameToLoad, Json_Test));
            yield return null;
        }

       





       

        
        #region Json
        private IEnumerator GetJsonData(string jsonURL)
        {
            dataReady = false;
            using (UnityWebRequest webRequest = UnityWebRequest.Get(jsonURL))
            {
                // Send the request and wait for a response
                yield return webRequest.SendWebRequest();

                if (webRequest.result == UnityWebRequest.Result.ConnectionError ||
                    webRequest.result == UnityWebRequest.Result.ProtocolError)
                {
                    Debug.LogError("Error: " + webRequest.error);
                }
                else
                {
                    // Get the downloaded JSON text
                    string jsonText = webRequest.downloadHandler.text;
                    Debug.Log("Received JSON: " + jsonText);

                    // Parse the JSON into your C# class
                    var Data = JsonUtility.FromJson<GameLaunchData>(jsonText);
                    currentGameLaunchData = Data;
                    // Now you can access the data
                    Debug.Log("ID: " + currentGameLaunchData.gameID);
                    Debug.Log("Name: " + currentGameLaunchData.gameSOID);
                    Debug.Log("GameType: " + currentGameLaunchData.gameType);
                    dataReady = true;
                }
            }
        }
        private void GetOfflineData()
        {
            var Data = JsonUtility.FromJson<GameLaunchData>(json);
            currentGameLaunchData = Data;

            // Now you can access the data
            Debug.Log("ID: " + currentGameLaunchData.gameID);
            Debug.Log("Name: " + currentGameLaunchData.gameSOID);
            Debug.Log("GameType: " + currentGameLaunchData.gameType);
            dataReady = true;
        }
        #endregion


        #region Get
        public void Json_Test()
        {
            GamePFLocationData[] gamePFLocationData = { };

            gamePFLocationData = gamePFLocationSO.gamePFLocationDatas;

            int cashLoop = gamePFLocationData.Length;
            for (int i = 0; i < cashLoop; i++)
            {
                if (gamePFLocationData[i].gameID == currentGameLaunchData.gameID)
                {
                    LaunchGame(i);
                }
            }
        }
        private void LaunchGame(int ID)
        {
            SetScreenOrientation(ScreenOrientation.LandscapeLeft);

            currentOpenGame = Instantiate(gamePFLocationSO.GetGamePF(ID));
            currentOpenGame.InitGameHandlerImmersiveGame();
        }
        #endregion



        private void SetScreenOrientation(ScreenOrientation screenOrientation)
        {
            Screen.orientation = screenOrientation;
        }
    }
    [Serializable]
    public class GameLaunchData
    {
        public string gameID;
        public string gameSOID;
        public GameType gameType;
    }
}