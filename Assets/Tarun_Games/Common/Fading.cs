using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;
using TMPro;
public static class Fading 
{   
    public static void OnFadeInAndScaleUP(GameObject obj, float time)
    {
        Image tempimg = obj.transform.GetComponent<Image>();
        tempimg.color = new Color(tempimg.color.r, tempimg.color.g, tempimg.color.b, 0f);
        tempimg.DOFade(1f, time).SetLink(obj);

        obj.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        obj.transform.DOScale(new Vector3(1f, 1f, 1f), time).SetLink(obj);
    }

    public static void OnBubleFX(GameObject obj, float duration, Vector3 fromScaleValue, Vector3 toScaleValue)
    {
        obj.transform.localScale = fromScaleValue;
        obj.transform.DOScale(toScaleValue, duration).SetEase(Ease.OutBack).SetLink(obj);
    }
    public static void UIMoveAnim(GameObject obj, Vector2 xy, float time)
    {
        (obj.transform as RectTransform).DOAnchorPos(new Vector2(xy.x, xy.y), time, false).SetLink(obj);
    }
    public static void OnScaleIn(GameObject obj, float duration, Vector3 fromScaleValue, Vector3 toScaleValue)
    {
        obj.transform.localScale = fromScaleValue;
        obj.transform.DOScale(toScaleValue, duration).SetLink(obj);
    }
    public static void OnScaleOut(GameObject obj, float duration,Vector3 toScaleValue)
    {
        obj.transform.DOScale(toScaleValue, duration).SetLink(obj);
    }
    
    public static void OnFade(GameObject obj, float duration, float fromAlphaValue, float toAlphaValue)//
    {
        Image tempimg = obj.transform.GetComponent<Image>();
        tempimg.color = new Color(tempimg.color.r, tempimg.color.g, tempimg.color.b, fromAlphaValue);
        tempimg.DOFade(toAlphaValue, duration).SetLink(obj);
    }

    public static void OnShakeFX(GameObject obj, float duration , Vector3 strengh)
    {
        obj.transform.DOShakePosition(duration, strengh, 25).SetLink(obj);
    }

    public static IEnumerator LerpScoreRoutine(int fromValue,int toValue, TextMeshProUGUI change, float duration)
    {
        bool lerpOver = false;
        int gg = fromValue;
        DOTween.To(() => gg, x => gg = x, toValue, duration).OnComplete(() =>
        {
            lerpOver = true;
        }).SetLink(change.gameObject);
        while (!lerpOver)
        {
            change.text =gg.ToString();
            yield return null;
        }
    }
    public static IEnumerator LerpScoreRoutine(int fromValue, int toValue, Text change, float duration)
    {
        bool lerpOver = false;
        int gg = fromValue;
        DOTween.To(() => gg, x => gg = x, toValue, duration).OnComplete(() =>
        {
            lerpOver = true;
        }).SetLink(change.gameObject);
        while (!lerpOver)
        {
            change.text = gg.ToString();
            yield return null;
        }
    }

    public static void OnBreathingFX(Transform transform,float size, float time)
    {
        transform.DOScale(Vector3.one * size, time)
             .SetEase(Ease.InOutSine)
             .SetLoops(-1, LoopType.Yoyo).SetLink(transform.gameObject);
    }
}