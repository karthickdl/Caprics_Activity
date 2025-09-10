using UnityEngine;
using DG.Tweening;
using TMPro;
using System.Text;
using System.IO;

public class SettingsPopUp : PopUpsBase
{
    private const string PolicyLink = "https://vaultgamesstudio.com/privacy-policy/";
    private const string DiscordLinkForVaultGameZone = "https://discord.gg/xds5QDvEwh";

    [Header("Settings Panel")]

    public TextMeshProUGUI buildVSText;
    public TextMeshProUGUI deviceIDText;

    [Header("Sounds and Vibration")]
    public GameObject soundFXOnImg;
    public GameObject soundFXOffImg;

    public GameObject soundBGOnImg;
    public GameObject soundBGOffImg;

    public GameObject vibrationOnImg;
    public GameObject vibrationOffImg;

    #region Unity
    private void Start()
    {
        buildVSText.text = Application.version;
        deviceIDText.text = "Device ID: " + SystemInfo.deviceUniqueIdentifier;
        OnStartCalls();
        CheckSoundandVibration();
    }
    #endregion

    private void OnStartCalls()
    {
        //VaultAudioManager.Instance.PlaySound("Button_Click");
       // HapticTouchManager.PlayHaptics(HapticTypes.MediumImpact);
        // settingsBG.SetActive(false);
    }

    #region Sounds and Vibration
    public void OnSoundFXButtonOff()
    {
       // VaultAudioManager.Instance.PlaySound("Button_Click");
       // HapticTouchManager.PlayHaptics(HapticTypes.MediumImpact);

       /* if (SaveDataHandler.Instance.InGameSoundFXOn)
        {
            SaveDataHandler.Instance.InGameSoundFXOn = false;
        }
        else
        {
            SaveDataHandler.Instance.InGameSoundFXOn = true;
        }*/
        CheckSoundandVibration();
    }
    public void OnSoundBGButtonOff()
    {
      /*  VaultAudioManager.Instance.PlaySound("Button_Click");
        HapticTouchManager.PlayHaptics(HapticTypes.MediumImpact);

        if (SaveDataHandler.Instance.BGSoundOn)
        {
            SaveDataHandler.Instance.BGSoundOn = false;
        }
        else
        {
            SaveDataHandler.Instance.BGSoundOn = true;
        }*/
        CheckSoundandVibration();
    }
    public void OnVibrationButtonOff()
    {
       /* VaultAudioManager.Instance.PlaySound("Button_Click");
        HapticTouchManager.PlayHaptics(HapticTypes.MediumImpact);

        if (SaveDataHandler.Instance.VibrationOn)
        {
            SaveDataHandler.Instance.VibrationOn = false;
        }
        else
        {
            SaveDataHandler.Instance.VibrationOn = true;
        }*/
        CheckSoundandVibration();
    }
    private void CheckSoundandVibration()
    {
       /* if (SaveDataHandler.Instance.InGameSoundFXOn)
        {
            soundFXOnImg.SetActive(true);
            soundFXOffImg.SetActive(false);
            VaultAudioManager.Instance.inGameMixer.SetFloat("Val", 0f);
        }
        else
        {
            soundFXOnImg.SetActive(false);
            soundFXOffImg.SetActive(true);
            VaultAudioManager.Instance.inGameMixer.SetFloat("Val", -80f);
        }

        if (SaveDataHandler.Instance.VibrationOn)
        {
            vibrationOnImg.SetActive(true);
            vibrationOffImg.SetActive(false);
            HapticTouchManager.IsHaptic = true;
        }
        else
        {
            vibrationOnImg.SetActive(false);
            vibrationOffImg.SetActive(true);
            HapticTouchManager.IsHaptic = false;
        }

        if (SaveDataHandler.Instance.BGSoundOn)
        {
            soundBGOnImg.SetActive(true);
            soundBGOffImg.SetActive(false);
            VaultAudioManager.Instance.bgMixer.SetFloat("Val", 0f);
        }
        else
        {
            soundBGOnImg.SetActive(false);
            soundBGOffImg.SetActive(true);
            VaultAudioManager.Instance.bgMixer.SetFloat("Val", -80f);
        }*/
    }
    #endregion

    #region Buttons
    public void OnCloseSettingsButton()
    {
        //GameManager.Instance.gameState = GameState.InGame;
       /* VaultAudioManager.Instance.PlaySound("Button_Click");
        HapticTouchManager.PlayHaptics(HapticTypes.MediumImpact);*/

        DOVirtual.DelayedCall(0.15f, () =>
        {
            base.OnCloseButton();
        });
    }
    public void OnContactUS()
    {
        /*VaultAudioManager.Instance.PlaySound("Button_Click");
        HapticTouchManager.PlayHaptics(HapticTypes.MediumImpact);*/

        string emailTo = "info@vaultgamesstudio.com";
        string subject = Application.productName + " : Support Mail";

        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.AppendLine("Application Version : V" + Application.version);
       // stringBuilder.AppendLine("Level : " + SaveDataHandler.Instance.CurrentLevel);
        stringBuilder.AppendLine("Device-Model : " + SystemInfo.deviceModel);
        stringBuilder.AppendLine("OS : " + SystemInfo.operatingSystem);
        stringBuilder.AppendLine("Device-ID : " + SystemInfo.deviceUniqueIdentifier);

        Application.OpenURL("mailto:" + emailTo + "?subject=" + subject + "&body=" + stringBuilder.ToString());
    }
    public void OnPolicyButton()
    {
        /*VaultAudioManager.Instance.PlaySound("Button_Click");
        HapticTouchManager.PlayHaptics(HapticTypes.MediumImpact);*/
        Application.OpenURL(PolicyLink);
    }
   
    #endregion
   
}