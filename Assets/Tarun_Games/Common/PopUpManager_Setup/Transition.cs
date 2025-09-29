using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

public class Transition : MonoBehaviour
{
    public Image fadeIMG;
    public Transform fadeTransform;
    public void Init(TransitionType _transitionType = TransitionType.Fade, Action action = null)
    {
        fadeIMG.DOFade(0, 0);
        switch (_transitionType)
        {
            case TransitionType.Fade:

                fadeIMG.DOFade(1, 0.5f).OnComplete(()=> 
                {
                    fadeIMG.DOFade(0, 0.5f).SetEase(Ease.Linear);
                }).SetEase(Ease.Linear);                
                break;
            case TransitionType.Fade2:
                fadeIMG.DOFade(1, 0);
                fadeIMG.DOFade(0, 1f).OnComplete(() =>
                {
                    action?.Invoke();
                   // fadeIMG.DOFade(0, 0.5f).SetEase(Ease.Linear);
                }).SetEase(Ease.Linear);
                break;
            case TransitionType.C_Fill:
                fadeIMG.type = Image.Type.Filled;
                fadeIMG.DOFade(1, 0);
                fadeIMG.fillMethod = Image.FillMethod.Radial360;
                fadeIMG.fillAmount = 0;
                fadeIMG.DOFillAmount(1, 0.5f);
                break;
            default:
                break;
        }
    }
}
public enum TransitionType
{
    Fade,
    Fade2,
    C_Fill
}