using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Transition : MonoBehaviour
{
    public Image fadeIMG;
    public Transform fadeTransform;
    public void Init(TransitionType _transitionType = TransitionType.Fade)
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
                break;
            default:
                break;
        }


    }
}
public enum TransitionType
{
    Fade,
    Fade2
}