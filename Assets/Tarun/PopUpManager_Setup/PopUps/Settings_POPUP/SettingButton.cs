using UnityEngine;

    public class SettingButton : MonoBehaviour
    {
        public void OnSettingsOpenButton()
        {
           // VaultAudioManager.Instance.PlaySound("Button_Click");
          //  HapticTouchManager.PlayHaptics(HapticTypes.MediumImpact);
            VaultPopUpsManager.Instance.ShowPopup(NormalPopUpTypes.SettingPOPUP);
        }
    }