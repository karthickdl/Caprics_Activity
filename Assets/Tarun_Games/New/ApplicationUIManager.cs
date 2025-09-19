using DLearners;
using UnityEngine;

public class ApplicationUIManager : Singleton<ApplicationUIManager>
{
    [SerializeField] GameIconButton gameIconButtonPF;
    [SerializeField] Transform buttonSpawn;

    private void Start()
    {

        for (int i = 0; i < 2; i++)
        {
            GameIconButton cash = Instantiate(gameIconButtonPF, buttonSpawn);
            cash.InitGameIconButton(i);
        }
    }
}
