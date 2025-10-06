using DG.Tweening;
using UnityEngine;

public class CoinPF : MonoBehaviour
{
    public void CoinMoveToPoint(Vector3 endPoint,float speed)
    {
        transform.DOMove(endPoint, speed).OnComplete(()=> 
        {
            Destroy(gameObject,0.1f);
        }).SetLink(this.gameObject).SetEase(Ease.Linear);
    }
}
