using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DLearners
{
    public class HUDManager : Singleton<HUDManager>
    {
        [Header ("Tap To Play")]
        [SerializeField] private Button tapToPlayButton;
        [SerializeField] private TextMeshProUGUI tapToPlayText;
        [SerializeField] private string[] tapToPlayTexts;

        [Header("Score Update System")]
        [SerializeField] private Text pointsText;//TEX_points
        [SerializeField] private Text TEX_questionCount;
        [SerializeField] private TextMeshProUGUI cashPointFX;//TM_pointFx;
        [SerializeField] private Button instructionButton;


        [SerializeField] private int correctAnswerPoint;
        [SerializeField] private int wrongAnswerPoint;
        [SerializeField] private int totalQuestionsCount;
        protected int currentQuestionsID { get; private set; }
        protected int score { get; private set; }

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
           // instructionButton.onClick.AddListener(() => { });//Popup
        }
        private void OnResetData()
        {
            pointsText.text = "0";
            TEXM_instruction.text = "";
            TEXM_instruction2.text = "";
            pointsText.text = "";
            TEX_questionCount.text = "";
            cashPointFX.text = "";
            currentQuestionsID = 0;
            score = 0;
        }
        #endregion

        #region Score Update System
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

        #region Score Update System
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



        [SerializeField] private TextMeshProUGUI TEXM_instruction;
        [SerializeField] private TextMeshProUGUI TEXM_instruction2;
        [SerializeField] private Button button;
        private AudioClip audioClip;
        public void InitInstruction(InstructionData _instructionData)
        {
            int cashLoop = _instructionData.instruction.Count;
            for (int i = 0; i < cashLoop; i++)
            {
                TEXM_instruction.text = _instructionData.instruction[i];
                audioClip = _instructionData.instructionAudioClip[i];

                button.onClick.AddListener(() =>
                {
                    DLearners.DLearnersAudioManager.Instance.PlaySound3(audioClip);
                });
            }           
        }

        public void BUT_instructionPage()
        {
            StopAllCoroutines();
            Time.timeScale = 0;
            //InitInstruction();
        }

        public void BUT_closeInstruction()
        {
            Time.timeScale = 1;
        }

       
    }
}