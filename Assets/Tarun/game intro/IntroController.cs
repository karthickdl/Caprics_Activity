using UnityEngine;

public class IntroController : MonoBehaviour
{
    [SerializeField] private AnimationClip AC_introAnim;

    private void Start()
    {
        Destroy(gameObject, AC_introAnim.length);
    }
}