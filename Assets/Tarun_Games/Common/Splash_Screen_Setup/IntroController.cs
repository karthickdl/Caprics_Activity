using DG.Tweening;
using UnityEngine;

public class IntroController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private AnimationClip AC_introAnim;
    public bool isDone;
    public void InitIntroController()
    {
        animator.ResetTrigger("IsStart");
        animator.SetTrigger("IsStart");
        DOVirtual.DelayedCall(AC_introAnim.length,() =>
        {
            isDone = true;
            Destroy(gameObject);
        }).SetLink(gameObject);
    }
}