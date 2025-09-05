using DLearners;
using UnityEngine;

public class TarunTesting : Singleton<TarunTesting>
{
    public GameAudioDataSO gameAudioDataSO;

    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.H))
        {
            DLearnersAudioManager.Instance.PlaySound2("Play",2f);
        }
    }
}
