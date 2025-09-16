using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace DLearners
{
    public class DemoController : MonoBehaviour
    {
        [SerializeField] private AudioSource demoAudioSource;
        [SerializeField] private Image bg;

        [Header("Skip Button")]
        [SerializeField] private Button skipButton;

        [Header("Debug")]
        [SerializeField] private DemoControllInfo[] demoControllInfos;

        public bool isDone;

        private int id;

        private DemoController_N demoController_N;

        public void InitDemoController(DemoControllerDataSO _demoControllerDataSO)
        {
            id = 0;
            bg.sprite = _demoControllerDataSO.bgSprite;
            demoController_N = Instantiate(_demoControllerDataSO.demoController_N, this.transform);
            skipButton.onClick.AddListener(() =>
            {
                OnSkipButton();
            });
            demoControllInfos = _demoControllerDataSO.demoControllInfos;
            PlaySequence(demoControllInfos[id]);
        }

        private float cashWaitTime;
        private IEnumerator Test()
        {
            yield return new WaitForSeconds(cashWaitTime);

            id++;
            if (id < demoControllInfos.Length)
            {
                PlaySequence(demoControllInfos[id]);
            }
            else
            {
                OnSkipButton();
            }

            yield return null;
        }

        private void PlaySequence(DemoControllInfo demoControllInfo)
        {
            demoController_N.text.text = demoControllInfo.text;
            demoAudioSource.clip = demoControllInfo.clip;
            cashWaitTime = demoControllInfo.clip.length;
            demoAudioSource.Play();
            StartCoroutine(Test());
        }

        private void OnSkipButton()
        {
            HUDManager.Instance.SetHUDOnOff(true);
            isDone = true;
            Time.timeScale = 1;
            Destroy(this.gameObject, 0.1f);
        }
    }
}