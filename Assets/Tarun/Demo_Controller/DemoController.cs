using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DemoController : MonoBehaviour
{
    [SerializeField] private AudioSource demoAudioSource;
    private DemoControllerDataSO _demoControllerDataSO;
    [Header ("Skip Button")]
    [SerializeField] private Button skipButton;
    [SerializeField] private Image skipIMG;

    [Header("Text Panel")]
    [SerializeField] private Transform textPanel;
    [SerializeField] private TextMeshProUGUI text;

    [Header("Debug")]
    [SerializeField] private DemoControllInfo[] demoControllInfos;

    

    private int id;
    

    public void InitDemoController(DemoControllerDataSO _demoControllerDataSO)
    {
        Instantiate(_demoControllerDataSO.demoController_N,this.transform);
        skipButton.onClick.AddListener(() => { OnSkipButton(); });
        skipIMG.sprite = null;
        demoControllInfos = _demoControllerDataSO.demoControllInfos;
        Setgg(demoControllInfos[id]);        
    }

    private float cashWaitTime;
    private IEnumerator Test()
    {
        yield return new WaitForSeconds(cashWaitTime);

        id++;
        if (id < demoControllInfos.Length)
        {
            Setgg(demoControllInfos[id]);
        }
        else
        {
            OnSkipButton();
        }

        yield return null;
    }

    private void Setgg(DemoControllInfo ggs)
    {
       
        text.text = ggs.text;
        demoAudioSource.clip = ggs.clip;
        cashWaitTime = ggs.clip.length;
        demoAudioSource.Play();
        StartCoroutine(Test());        
    }

    private void OnSkipButton()
    {
        gameObject.SetActive(false);
    }
}

