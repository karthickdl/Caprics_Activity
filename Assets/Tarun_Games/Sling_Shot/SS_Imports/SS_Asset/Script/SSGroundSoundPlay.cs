using DLearners;
using UnityEngine;

public class SSGroundSoundPlay : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        DLearnersAudioManager.Instance.PlayGameSpecificSound("Fall_Ground");
    }
}
