
using UnityEngine.UI;

public class NoInternetPopUP : PopUpsBase
{
    public Button retryButton;

    protected override void OnEnable()
    {
        base.OnEnable();
        retryButton.onClick.AddListener(OnRetry);
    }
    protected override void OnDisable()
    {
        base.OnDisable();
    }

    private void OnRetry()
    {
       /* VaultAudioManager.Instance.PlaySound("Button_Click");
        HapticTouchManager.PlayHaptics(HapticTypes.MediumImpact);*/
    }
}