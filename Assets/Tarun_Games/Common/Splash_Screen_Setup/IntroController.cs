using UnityEngine;

public class IntroController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private AnimationClip AC_introAnim;

    public void Init()
    {
        animator.SetTrigger("IsStart");

    }
    public float InitIntroController()
    {
        Destroy(gameObject, AC_introAnim.length);
        return AC_introAnim.length;
    }
}