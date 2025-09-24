using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace DLearnersApplication
{
    public class LoadingMenuManager : Singleton<LoadingMenuManager>
    {
        public Transform logo;

        public float loadingSpeedOffSet = 1f;

        private void Start()
        {
            //VaultAudioManager.Instance.PlaySound("BGM");
            LoadingAnimation();
#if VAULT_Plugin_Manager
            return;
#endif
           // StartCoroutine(LoadScene());
        }

        public IEnumerator LoadScene(string sceneNameToLoad,Action gg)
        {
            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneNameToLoad);
            asyncOperation.allowSceneActivation = false;
            while (!asyncOperation.isDone)
            {
                LoadingProgress(Mathf.Lerp(sliderValue, asyncOperation.progress + 0.11f, Time.deltaTime * loadingSpeedOffSet));

                if (sliderValue >= 1f)
                {
                    asyncOperation.allowSceneActivation = true;
                    gg?.Invoke();
                    // SaveDataHandler.Instance.GameSceneLoaded = true;
                }
                yield return null;
            }
        }

        private void LoadingAnimation()
        {
            Fading.OnBubleFX(logo.gameObject, 0.8f, Vector3.zero, Vector3.one);
        }

        [SerializeField]
        private Slider loadingBar;
        public float sliderValue => loadingBar.value;
        private void LoadingProgress(float percentage)
        {
            loadingBar.value = percentage;
        }
    }
}