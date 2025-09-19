using DLearners;
using UnityEngine;

public class ApplicationManager : Singleton<ApplicationManager>
{
    public GameHandlerBase[] gg;

    public GameHandlerBase hg;
    private void Start()
    {
        HUDManager.Instance.gameObject.SetActive(false);
    }
    protected override void Awake()
    {
        base.Awake();
    }
    public void tarun(int ID)
    {
        Tarun2(ScreenOrientation.LandscapeLeft);
        hg = Instantiate(gg[ID]);
        ApplicationUIManager.Instance.gameObject.SetActive(false);

    }


    public void Tarun2(ScreenOrientation screenOrientation)
    {
        Screen.orientation = screenOrientation;
    }


    public void Tarun3()
    {
        Tarun2(ScreenOrientation.Portrait);
        ApplicationUIManager.Instance.gameObject.SetActive(true);
        HUDManager.Instance.gameObject.SetActive(false);


            Destroy(hg.gameObject);
        hg = null;
        DLearnersAudioManager.Instance.CleanUp();
    }
}
