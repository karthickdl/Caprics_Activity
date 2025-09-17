using DLearners;
using UnityEngine;
using UnityEngine.UI;
public class LevelComplete : PopUpsBase
{
    public Text TEX_finalPoints;
    public Button replayButton, nextButton;
    public Transform[] starsOBJs;
    
    protected override void OnDisable()
    {
        base.OnDisable();
        replayButton.onClick.RemoveAllListeners();
        nextButton.onClick.RemoveAllListeners();
    }
    public override void Show()
    {
        base.Show();
        InitLevelComplet();
    }

    private void InitLevelComplet()
    {
        replayButton.onClick.AddListener(() =>
        {
            OnReplayButton();
        });
        nextButton.onClick.AddListener(() =>
        {
            OnNextButton();
        });
        SetStarts(false, 3);
        SetScore();
    }
    private void SetScore()
    {
        int totalObtainablePoints = GameHandlerImmersiveGame.Instance.I_TotalQuestions * GameHandlerImmersiveGame.Instance.I_correctPoints;
        // Debug.Log("Max points : " + totalObtainablePoints);
        // Debug.Log("Total obtainable points : " + totalObtainablePoints);
        int I_2star = totalObtainablePoints / 3;//10
        int I_3star = totalObtainablePoints / 2;//20

        TEX_finalPoints.text = GameHandlerImmersiveGame.Instance.I_TotalPoints.ToString();
        // Debug.Log("Points got : " + DLearners.TarunTesting.Instance.I_TotalPoints);

        int totalPoints = GameHandlerImmersiveGame.Instance.I_TotalPoints;
        int cashCount=0;
        if (totalPoints >= 0 && I_2star > totalPoints)
        {
            cashCount = 1;
        }
        else if (I_3star > totalPoints && totalPoints >= I_2star)
        {
            cashCount = 2;
        }
        else if (I_3star >= totalPoints)
        {
            cashCount = 3;
        }
        SetStarts(true, cashCount);
    }
    private void SetStarts(bool isOn,int count)
    {
        int cashLoop = starsOBJs.Length;
        for (int i = 0; i < cashLoop; i++)
        {
            if(count-1>= i)
            {
                starsOBJs[i].gameObject.SetActive(isOn);
                Fading.OnBubleFX(starsOBJs[i].gameObject,0.25f, Vector3.zero,Vector3.one);
            }  
        }
    }

    #region Buttons
    private void OnReplayButton()
    {
#if UNITY_ANDROID || UNITY_IOS
        Debug.Log("OnReplayButton");
#elif UNITY_WEBGL
 Debug.Log("OnNextButton");
#endif
    }
    private void OnNextButton()
    {
#if UNITY_ANDROID || UNITY_IOS
        Debug.Log("OnNextButton");
#elif UNITY_WEBGL
Application.ExternalEval("closeApplication()");
#endif
    }
    #endregion
}