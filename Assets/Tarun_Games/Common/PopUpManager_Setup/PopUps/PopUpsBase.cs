using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System;
public class PopUpsBase : MonoBehaviour
{
    public Button popUpCloseButton;

    public Action turnOffHolder;


    protected virtual void OnEnable()
    {
        if (popUpCloseButton != null)
        {
            popUpCloseButton.onClick.AddListener(OnCloseButton);
        }
    }
    protected virtual void OnDisable()
    {
        if (popUpCloseButton != null)
        {
            popUpCloseButton.onClick.RemoveAllListeners();
        }
    }
    
    public virtual void OnCloseButton()
    {
       // VaultAudioManager.Instance.PlaySound("Button_Click");
       // HapticTouchManager.PlayHaptics(HapticTypes.MediumImpact);
        Hide();
    }
    //---------

    private Action onHiddenCallback;

    public virtual void Show()
    {
        transform.localScale = Vector3.zero;
        gameObject.SetActive(true);
        transform.DOScale(Vector3.one, 0.25f).SetEase(Ease.OutBack).OnComplete(() =>
        {
            PostShowAction();
        });
    }

    public virtual void Hide()
    {
        transform.DOScale(Vector3.zero, 0.25f).SetEase(Ease.InBack).OnComplete(() =>
        {
            gameObject.SetActive(false);
            onHiddenCallback?.Invoke();
        });
    }

    protected virtual void PostShowAction() { }

    public virtual void SetOnHiddenCallback(Action callback)
    {
        onHiddenCallback = callback;
    }
}