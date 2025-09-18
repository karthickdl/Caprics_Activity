using UnityEngine;

public class SSGroundSoundPlay : MonoBehaviour
{
    public AudioSource AS_fall;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        AS_fall.Play();
    }
}
