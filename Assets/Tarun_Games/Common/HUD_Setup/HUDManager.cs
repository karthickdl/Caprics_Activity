using DG.Tweening;
using DLearnersApplication;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DLearners
{
    public class HUDManager : Singleton<HUDManager>
    {

        [SerializeField] private int correctAnswerPoint;
        [SerializeField] private int wrongAnswerPoint;
        [SerializeField] private int totalQuestionsCount;
        protected int currentQuestionsID { get; private set; }
        protected int score { get; private set; }

        private void Start()
        {
            SetHUDOnOff(false);
        }
        #region Init
        public void SetHUDOnOff(bool isOn)
        {
            this.gameObject.SetActive(isOn);
        }
        public void InitHUD(DataSO _dataSO)
        {
            OnResetData();
            correctAnswerPoint = _dataSO.GetCorrectAnswerPoint();
            wrongAnswerPoint = _dataSO.GetWrongAnswerPoint();
            totalQuestionsCount = _dataSO.datas.Count;
            UpdateQuestionCountText(0);
            InitInstruction(_dataSO.instructionData);
            instructionButton.onClick.AddListener(() => { OnOpenInstructionPanel(); });
            InitPauseMenu();
        }
        private void OnResetData()
        {
            pointsText.text = "0";
            TEXM_instruction.text = "";
            pointsText.text = "";
            TEX_questionCount.text = "";
            cashPointFX.text = "";
            currentQuestionsID = 0;
            score = 0;
        }
        #endregion

        #region Score Update System
        [Header("Score Update System")]
        [SerializeField] private Text pointsText;//TEX_points
        [SerializeField] private Text TEX_questionCount;
        [SerializeField] private TextMeshProUGUI cashPointFX;//TM_pointFx;
        public void UpdateScoreText(bool isAdd)
        {
            string cash = "";
            int cashScore = 0;
            if (isAdd)
            {
                cash = "+" + correctAnswerPoint + " points";
                cashScore += correctAnswerPoint;
            }
            else
            {
                cash = "-" + wrongAnswerPoint + " points";
                if(score > wrongAnswerPoint)
                {
                    cashScore -= wrongAnswerPoint;
                }
                else
                {
                    cashScore = 0;
                    score = 0;
                }
            }


            cashPointFX.text = cash;


            score += cashScore;
            pointsText.text = score.ToString();

            DOVirtual.DelayedCall(1f,() =>
            {
                cashPointFX.text = "";
            });
        }
        public void UpdateScoreText(bool isAdd,int points)
        {
            string cash = "";
            int cashScore = 0;
            if (isAdd)
            {
                cash = "+" + points + " points";
                cashScore += points;
            }
            else
            {
                cash = "-" + points + " points";
                if (score > points)
                {
                    cashScore -= points;
                }
                else
                {
                    cashScore = 0;
                    score = 0;
                }
            }


            cashPointFX.text = cash;


            score += cashScore;
            pointsText.text = score.ToString();

            DOVirtual.DelayedCall(1f, () =>
            {
                cashPointFX.text = "";
            });
        }
        #endregion

        public void UpdateQuestionCountText(int _currentQuestionsID)
        {
            TEX_questionCount.text = ((_currentQuestionsID + 1) + "/" + totalQuestionsCount).ToString();
        }

        #region Tap To Play System
        [Header("Tap To Play")]
        [SerializeField] private Button tapToPlayButton;
        [SerializeField] private TextMeshProUGUI tapToPlayText;
        [SerializeField] private string[] tapToPlayTexts;
        public void SetTapToPlayOnAndOff(bool isOn)
        {
            tapToPlayButton.gameObject.SetActive(isOn);
#if UNITY_WEBGL
            tapToPlayText.text = tapToPlayTexts[1];
#elif UNITY_ANDROID || UNITY_IOS
            tapToPlayText.text = tapToPlayTexts[0];
#endif
            tapToPlayButton.onClick.AddListener(() =>
            {                
                GameManagerBase.Instance.OnPlayButton();
                SetTapToPlayOnAndOff(false);
                tapToPlayButton.onClick.RemoveAllListeners();
            });
            // Fading.OnBreathingFX(tapToPlayText.transform,0.2f,0.35f);
        }
        #endregion

        #region Instruction System
        [Header("Instruction Panel")]
        [SerializeField] private GameObject instructionOBJ;
        [SerializeField] private Button instructionButton;
        [SerializeField] private TextMeshProUGUI TEXM_instruction;
        [SerializeField] private Button audioPlayButton;
        [SerializeField] private Button closeButton;
        [SerializeField] private Button howToPlayButton;
        private AudioClip audioClip;
        public void InitInstruction(InstructionData _instructionData)
        {
            int cashLoop = _instructionData.instruction.Count;
            for (int i = 0; i < cashLoop; i++)
            {
                TEXM_instruction.text = _instructionData.instruction[i];
                audioClip = _instructionData.instructionAudioClip[i];

                audioPlayButton.onClick.AddListener(() =>
                {
                    DLearnersAudioManager.Instance.PlaySound3(audioClip);
                });
            }
            howToPlayButton.onClick.AddListener(() =>
            {
                SetHUDOnOff(false);
                instructionOBJ.gameObject.SetActive(false);
                GameHandlerImmersiveGame.Instance.gg();
            });
            closeButton.onClick.AddListener(() =>
            {
                OnCloseInstructionPanel();
                /*audioPlayButton.onClick.RemoveAllListeners();
                closeButton.onClick.RemoveAllListeners();
                howToPlayButton.onClick.RemoveAllListeners();*/
            });
        }

        public void OnOpenInstructionPanel()
        {
           // StopAllCoroutines();
            Time.timeScale = 0;
            instructionOBJ.gameObject.SetActive(true);
            //InitInstruction();
        }

        public void OnCloseInstructionPanel()
        {
            Time.timeScale = 1;
            instructionOBJ.gameObject.SetActive(false);
        }

        #endregion

        #region Pause System
        [Header ("Pause Menu")]
        [SerializeField] private GameObject pauseMenuOBJ;
        [SerializeField] private Button pauseButton;
        [SerializeField] private Button resumeButton;
        [SerializeField] private Button homeButton;
        [SerializeField] private float F_volume;
        [SerializeField] private Slider SL_volume;

        private void InitPauseMenu()
        {
            SL_volume.value = F_volume;
            Time.timeScale = 1;
            pauseMenuOBJ.SetActive(false);
            pauseButton.onClick.AddListener(() => { OnPauseButton();});
            resumeButton.onClick.AddListener(() => { OnResumeButton();});
            homeButton.onClick.AddListener(() => { OnHomeButton();});
        }

        private void OnPauseButton()
        {
            pauseMenuOBJ.SetActive(true);
            Time.timeScale = 0;
        }
        private void OnResumeButton()
        {
            pauseMenuOBJ.SetActive(false);
            Time.timeScale = 1;
        }
        private void OnHomeButton()
        {
#if UNITY_WEBGL
Application.ExternalEval("closeApplication()");
#elif UNITY_ANDROID || UNITY_IOS
            ApplicationManager.Instance.OnCloseGameAndBackToHomePage("Application_Menu");
            SetHUDOnOff(false);
#endif
        }
        #endregion
    }
}