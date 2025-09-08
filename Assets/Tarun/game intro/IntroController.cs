using UnityEngine;

public class IntroController : MonoBehaviour
{
    [SerializeField] private AnimationClip AC_introAnim;

    public float InitIntroController()
    {
        Destroy(gameObject, AC_introAnim.length);
        return AC_introAnim.length;
    }
}