using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DLearners
{
    public class DemoController : MonoBehaviour
    {
        [SerializeField] private AudioSource demoAudioSource;
        [Header("Skip Button")]
        [SerializeField] private Button skipButton;
        [SerializeField] private Image skipIMG;

        [Header("Text Panel")]
        [SerializeField] private Transform textPanel;
        [SerializeField] private TextMeshProUGUI text;

        [Header("Debug")]
        [SerializeField] private DemoControllInfo[] demoControllInfos;

        public bool isDone;

        private int id;


        public void InitDemoController(DemoControllerDataSO _demoControllerDataSO)
        {
            Instantiate(_demoControllerDataSO.demoController_N, this.transform);
            skipButton.onClick.AddListener(() =>
            {
                OnSkipButton();
            });
            skipIMG.sprite = null;
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
            text.text = demoControllInfo.text;
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