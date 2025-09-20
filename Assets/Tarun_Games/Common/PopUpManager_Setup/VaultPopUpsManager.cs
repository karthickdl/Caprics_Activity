using DG.Tweening;
using DLearners;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VaultPopUpsManager : Singleton<VaultPopUpsManager>
{
    public PopUpsListSO _popUpsListSO;
    public Image popupBG;

    public Transform popUpHolder;
    public Transform flyPopUpHolder;
    public Transform inGamePopUpHolder;
    private bool isFlyPopUpInUse;
    RectTransform canvasRectTransform;
    private bool isTransition;

    #region Unity
    protected override void Awake()
    {
        base.Awake();

        popUpHolder.gameObject.SetActive(false);
        flyPopUpHolder.gameObject.SetActive(false);

        isFlyPopUpInUse = false;
        canvasRectTransform = GetComponent<RectTransform>();
        UpdatePopupBG(false);
    }

    public void UpdatePopupBG(bool setOn)
    {
        if (setOn)
        {
            popupBG.DOFade(0.95f, 0.5f);
        }
        else
        {
            popupBG.DOFade(0f, 0f);
        }
        popupBG.gameObject.SetActive(setOn);
    }


    public void DestroyAllPopups()
    {

        foreach (Transform item1 in popUpHolder)
        {
            if (item1 != null)
            {

                Destroy(item1.gameObject);
            }
        }
        foreach (Transform item2 in flyPopUpHolder)
        {
            if (item2 != null)
            {

                Destroy(item2.gameObject);
            }
        }
        foreach (Transform item3 in inGamePopUpHolder)
        {
            if (item3 != null)
            {

                Destroy(item3.gameObject);
            }
        }

        popUpHolder.gameObject.SetActive(false);
        flyPopUpHolder.gameObject.SetActive(false);
        inGamePopUpHolder.gameObject.SetActive(false);
        UpdatePopupBG(false);
    }

    #endregion

    public void ShowTransition(TransitionType _transitionType)
    {
        if (isTransition)
        {
            return;
        }
        isTransition = true;
        popUpHolder.gameObject.SetActive(true);
        Transition tempTransition = Instantiate(_popUpsListSO.transition, popUpHolder);
        tempTransition.Init(_transitionType);

        DOVirtual.DelayedCall(1f,() =>
        {
            Destroy(tempTransition.gameObject,0.1f);
            popUpHolder.gameObject.SetActive(false);
            isTransition = false;
        }).SetLink(tempTransition.gameObject);        
    }
    public void ShowTransition(TransitionType _transitionType, Action action)
    {
        if (isTransition)
        {
            return;
        }
        isTransition = true;
        popUpHolder.gameObject.SetActive(true);
        Transition tempTransition = Instantiate(_popUpsListSO.transition, popUpHolder);
        tempTransition.Init(_transitionType, action);

        DOVirtual.DelayedCall(1f, () =>
        {
            Destroy(tempTransition.gameObject, 0.1f);
            popUpHolder.gameObject.SetActive(false);
            isTransition = false;
        }).SetLink(tempTransition.gameObject);

    }

    #region FlyPopUp
    public void CreateFlyPopUpToast(string text, int ID)
    {
        if (isFlyPopUpInUse)
        {
            return;
        }
        isFlyPopUpInUse = true;
        flyPopUpHolder.gameObject.SetActive(true);
        ToastPopUp tempToastPopUp = Instantiate(_popUpsListSO.flyPopUps[ID], flyPopUpHolder);
        tempToastPopUp.SetText(text);

        RectTransform rectTransform = tempToastPopUp.panelTransform;
        rectTransform.localScale = Vector3.one * 0.3f;
        rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, -50);
        Sequence toastSeq = DOTween.Sequence(rectTransform);
        toastSeq.Append(rectTransform.DOScale(1f, 0.3f).SetEase(Ease.OutBack));
        toastSeq.Insert(0.1f, rectTransform.DOAnchorPosY(350, 0.5f).SetEase(Ease.OutBack));
        toastSeq.AppendInterval(2f);
        toastSeq.Append(rectTransform.DOScale(0.3f, 0.3f).SetEase(Ease.InBack));
        toastSeq.Insert(2.4f, rectTransform.DOAnchorPosY(-50, 0.5f).SetEase(Ease.InBack));
        toastSeq.AppendCallback(() =>
        {
            Destroy(tempToastPopUp.gameObject);
            flyPopUpHolder.gameObject.SetActive(false);
            isFlyPopUpInUse = false;
        });
    }
    public bool CheckForPopUpReady(int totalLevelsCount, int afterEveryNum, int currentLevelNum)
    {
        int tempNum = afterEveryNum;
        for (int i = 1; i < totalLevelsCount; i++)
        {
            if (totalLevelsCount >= tempNum)
            {
                tempNum = tempNum * i;
                if (tempNum == currentLevelNum)
                {
                    return true;
                }
            }
            else
            {
                return false;
            }
        }
        return false;
    }
    #endregion

    #region In Game PopUp
    public void CallInGamePopUp(InGamePopUps inGamePopUps, float speed = 0.5f, Ease ease = Ease.Linear)
    {
        inGamePopUpHolder.gameObject.SetActive(true);

    }
    #endregion

    #region Helper
    public Vector2 WorldToCanvasPosition(Vector3 position)
    {

        Vector2 temp = Camera.main.WorldToViewportPoint(position);


        temp.x *= canvasRectTransform.sizeDelta.x;
        temp.y *= canvasRectTransform.sizeDelta.y;


        temp.x -= canvasRectTransform.sizeDelta.x * canvasRectTransform.pivot.x;
        temp.y -= canvasRectTransform.sizeDelta.y * canvasRectTransform.pivot.y;

        return temp;
    }
    #endregion


    private List<PopUpsBase> popupList = new List<PopUpsBase>();

    public void ShowPopup(NormalPopUpTypes normalPopupType, Action OnCloseCallBack = null)
    {
        UpdatePopupBG(true);
        popUpHolder.gameObject.SetActive(true);
        PopUpsBase popupInstance = Instantiate(_popUpsListSO.popUps[(int)normalPopupType], popUpHolder);
        popupList.Add(popupInstance);
        popupInstance.SetOnHiddenCallback(() =>
        {
            ClosePopup(popupInstance);
            OnCloseCallBack?.Invoke();
        });
        popupInstance.Show();
    }

    private void ClosePopup(PopUpsBase popup)
    {
        if (popupList.Contains(popup))
        {
            popupList.Remove(popup);
            Destroy(popup.gameObject);

        }
        if (popupList.Count == 0)
        {
            TurnOffPopUpHolder();
        }
    }
    private void TurnOffPopUpHolder()
    {
        UpdatePopupBG(false);
        popUpHolder.gameObject.SetActive(false);
    }

    public void CloseAllPopups()
    {
        foreach (var popup in popupList)
        {
            popup.Hide();
        }
        popupList.Clear();
    }

    public T GetPopup<T>() where T : PopUpsBase
    {
        foreach (var popup in popupList)
        {
            if (popup is T typedPopup)
            {
                return typedPopup;
            }
        }
        return null;
    }

    public void CloseMostRecentPopup()
    {
        if (popupList.Count > 0)
        {
            var mostRecentPopup = popupList[popupList.Count - 1];
            mostRecentPopup.Hide();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            ShowPopup(NormalPopUpTypes.SpinWheelPOPUP);
        }
    }
}
public enum NormalPopUpTypes
{
    RateUsPOPUP,
    SettingPOPUP,
    LevelCompletePOPUP,
    NoInternetPOPUP,
    NoAdsPOPUP,
    DailyRewardPOPUP,
    SpinWheelPOPUP,
    None,
}
public enum InGamePopUps
{
    Combo,
    Great,
    Superb,
    Awesome,
    Random
}