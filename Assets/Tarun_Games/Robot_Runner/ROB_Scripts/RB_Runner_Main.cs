using DG.Tweening;
using DLearners;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.UI;


public class RB_Runner_Main : GameManagerBase
{
    [Header("Objects")]
    public GameObject[] GA_Question;
    public GameObject G_QuestionSpawn;
    public GameObject G_currentquestion;
    public GameObject G_Robot;
    public TextMeshProUGUI questionText;
    public GameObject G_Question;
    public GameObject G_Options;
    public GameObject[] GA_Options;
    GameObject G_Highlight;



    [SerializeField] private Sprite[] SPRA_ArrowsWebGL;
    [SerializeField] private Sprite[] SPRA_ArrowsMobile;
    [SerializeField] private Image[] IMGA_Up;
    [SerializeField] private Image[] IMGA_Down;

    #region Unity
    protected override void Awake()
    {
        base.Awake();
    }
    #endregion

    /// <summary>
    /// This will Trigger from tap to play screen.
    /// </summary>
    public override void OnPlayButton()
    {
        base.OnPlayButton();
        Robotmovement.Instance.OnPlayButton();
    }

    /// <summary>
    /// We are initialising the game after all the tutorial thing is completed. 
    /// </summary>
    public override void InitGame()
    {
        base.InitGame();
        

        currentQuestionID =0;        

        NextStep();
        #region----------Platform Checking to set sprites for controls in Demo

        /*if (MainController.instance.WEB)
        {
            // G_PlayerControls.SetActive(false);

            //setting images
            IMGA_Up[0].sprite = SPRA_ArrowsWebGL[0];
            IMGA_Up[1].sprite = SPRA_ArrowsWebGL[0];
            IMGA_Down[0].sprite = SPRA_ArrowsWebGL[1];
            IMGA_Down[1].sprite = SPRA_ArrowsWebGL[1];
        }
        else if (MainController.instance.MOBILE)
        {
            // G_PlayerControls.SetActive(true);

            //setting images
            IMGA_Up[0].sprite = SPRA_ArrowsMobile[0];
            IMGA_Up[1].sprite = SPRA_ArrowsMobile[0];
            IMGA_Down[0].sprite = SPRA_ArrowsMobile[1];
            IMGA_Down[1].sprite = SPRA_ArrowsMobile[1];
        }*/

        #endregion
    }

    /// <summary>
    /// Seting up level data from SO (per level) From base class
    /// </summary>
    protected override void GetSetCurrentLevelData()
    {
        base.GetSetCurrentLevelData();

        SetUpOptionsPanel();
    }

    /// <summary>
    /// Seting up Number of options for the game 
    /// </summary>
    private void SetUpOptionsPanel()
    {
        int cacheLoop = GA_Options.Length;
        for (int i = 0; i < cacheLoop; i++)
        {
            GA_Options[i].SetActive(false);
        }

        if (currentOptionCount == 2)
        {
            G_Options = GA_Options[0];
        }
        if (currentOptionCount == 3)
        {
            G_Options = GA_Options[1];
        }
        if (currentOptionCount == 4)
        {
            G_Options = GA_Options[2];
        }
        if (currentOptionCount == 5)
        {
            G_Options = GA_Options[3];
        }

        G_Options.SetActive(true);

        for (int i = 0; i < currentOptionCount; i++)
        {
            G_Options.transform.GetChild(i).GetChild(0).GetComponent<Button>().onClick.AddListener(() => { CheckAnswer(); });
        }
    }

    /// <summary>
    /// Showing transition and moving to next question, or checking for level complete 
    /// </summary>
    private void NextStep()
    {
        VaultPopUpsManager.Instance.ShowTransition(TransitionType.Fade);
        if (currentQuestionID < GameHandlerImmersiveGame.Instance.dataSO.datas.Count)
        {
            GetSetCurrentLevelData();
            G_Question.SetActive(false);
            ShowCurrentQuestion();
        }
        else
        {
            OnLevelCompleted();
        }
    }

    /// <summary>
    /// adding data for the Question And setting up Level Question
    /// </summary>
    private void ShowCurrentQuestion()
    {
        G_Robot.SetActive(true);
        Robotmovement.Instance.RobotInIt();
        if (G_currentquestion != null)
        {
            Destroy(G_currentquestion);
        }
        int Index = Random.Range(0, GA_Question.Length);
        G_currentquestion = Instantiate(GA_Question[Index]);
        G_currentquestion.transform.SetParent(G_QuestionSpawn.transform, false);


        questionText.SetText(currentInstructionData.instruction[0]);

        HUDManager.Instance.UpdateQuestionCountText(currentQuestionID);//Tarun

        Transform tempTransform = G_Question.transform.GetChild(0).transform.GetChild(0);
        tempTransform.GetComponent<Image>().sprite = currentData.questionData.questionSprit;
        tempTransform.GetComponent<Image>().preserveAspect = true;
        tempTransform.GetComponent<AudioSource>().clip = currentData.questionData.questionAudioClip;


        int cashLoop = G_Options.transform.childCount;
        for (int i = 0; i < cashLoop; i++)
        {
            Transform tempTransform2 = G_Options.transform.GetChild(i).transform.GetChild(0);
            G_Options.transform.GetChild(i).name = currentData.options[i].option;
            tempTransform2.GetComponent<TextMeshProUGUI>().text = currentData.options[i].option;
            tempTransform2.GetComponent<TextMeshProUGUI>().color = Color.white;
            tempTransform2.GetComponent<AudioSource>().clip = currentData.options[i].optionAudioClip;
        }

        currentWrongAnsCount = 0;
    }


    /// <summary>
    /// For Checking the answer if it is right or wrong. ()
    /// </summary>
    public override void CheckAnswer()
    {
        base.CheckAnswer();

        if (isInputUnLocked)
        {
            GameObject G_Selected = EventSystem.current.currentSelectedGameObject;
            STR_currentSelectedAnswer = EventSystem.current.currentSelectedGameObject.GetComponent<TextMeshProUGUI>().text;

            if (STR_currentSelectedAnswer == STR_currentQuestionAnswer)
            {
                G_Selected.GetComponent<AudioSource>().Play();
                CorrectAnswerSequence();
            }
            else
            {
                WrongAnswerSequence();
            }
        }
    }

    /// <summary>
    /// On correct answer sequence
    /// </summary>
    public override void CorrectAnswerSequence()
    {
        base.CorrectAnswerSequence();
        isInputUnLocked = false;

        DLearnersAudioManager.Instance.PlayCommonSound("Com_Correct");
        I_Collect_count++;
        HUDManager.Instance.UpdateScoreText(true);

        THI_TrackGameData("1");
        Invoke(nameof(NextStep), 3f);
    }

    /// <summary>
    /// On Wrong answer sequence
    /// </summary>
    public override void WrongAnswerSequence()
    {
        base.WrongAnswerSequence();
        DLearnersAudioManager.Instance.PlayCommonSound("Com_Wrong");

        THI_TrackGameData("0");
        currentWrongAnsCount++;

        if (currentWrongAnsCount == wrongAnsLifeCounts[0])//3
        {
            if (currentDifficultyLevelType == DifficultyLevelType.Easy)
            {
                isInputUnLocked = false;
                int cashLoop = G_Options.transform.childCount;
                for (int i = 0; i < cashLoop; i++)
                {
                    if (G_Options.transform.GetChild(i).name == STR_currentQuestionAnswer)
                    {
                        G_Options.transform.GetChild(i).transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.green;
                    }
                }
                Invoke(nameof(NextStep), 5f);
            }
            else if (currentDifficultyLevelType == DifficultyLevelType.Medium)
            {
                isInputUnLocked = true;
                int cashLoop = G_Options.transform.childCount;
                for (int i = 0; i < cashLoop; i++)
                {
                    if (G_Options.transform.GetChild(i).name == STR_currentQuestionAnswer)
                    {
                        G_Highlight = G_Options.transform.GetChild(i).transform.GetChild(0).gameObject;
                    }
                }
                StartCoroutine(Highlight());
            }
        }
        else if (currentWrongAnsCount == wrongAnsLifeCounts[1])//2
        {
            if (currentDifficultyLevelType == DifficultyLevelType.Hard)
            {
                isInputUnLocked = false;
                Invoke(nameof(NextStep), 2f);
            }
        }

        HUDManager.Instance.UpdateScoreText(false);
    }

    public override void UpdateQuestion()
    {
        currentQuestionID++;
        isInputUnLocked = true;
        G_Question.SetActive(true);//

        AudioClip audioClipLent = currentInstructionData.instructionAudioClip[0];//Hardcode
        DLearnersAudioManager.Instance.PlaySound3(audioClipLent);
        DOVirtual.DelayedCall(audioClipLent.length,() =>
        {
            DLearnersAudioManager.Instance.PlaySound3(currentData.questionData.questionAudioClip);
        });
    }

   

    public override void OnLevelCompleted()
    {
        base.OnLevelCompleted();
        StartCoroutine(IN_sendDataToDB());
    }

    

    IEnumerator Highlight()
    {
        for (int i = 0; i < 5; i++)
        {
            G_Highlight.GetComponent<TextMeshProUGUI>().color = Color.green;
            yield return new WaitForSeconds(0.5f);
            G_Highlight.GetComponent<TextMeshProUGUI>().color = Color.white;
            yield return new WaitForSeconds(0.5f);
        }
        G_Highlight.GetComponent<TextMeshProUGUI>().color = Color.green;
    }

    

   


    public void THI_TrackGameData(string analysis)
    {
        DBmanager TrainSortingDB = new DBmanager();
        TrainSortingDB.question_id = currentData.questionData.questionID;
        TrainSortingDB.answer = STR_currentSelectedAnswer;
        TrainSortingDB.analysis = analysis;
        string toJson = JsonUtility.ToJson(TrainSortingDB);
        STRL_gameData.Add(toJson);
        STR_Data = string.Join(",", STRL_gameData);
    }
    public IEnumerator IN_sendDataToDB()
    {
        WWWForm form = new WWWForm();
        form.AddField("child_id", DLearners.GameHandlerImmersiveGame.Instance.STR_childID);
        form.AddField("game_id", DLearners.GameHandlerImmersiveGame.Instance.STR_GameID);
        form.AddField("game_details", "[" + STR_Data + "]");


        Debug.Log("child id : " + DLearners.GameHandlerImmersiveGame.Instance.STR_childID);
        Debug.Log("game_id  : " + DLearners.GameHandlerImmersiveGame.Instance.STR_GameID);
        Debug.Log("game_details: " + "[" + STR_Data + "]");

        UnityWebRequest www = UnityWebRequest.Post(DownloadManager.Instance.sendValueURL, form);
        yield return www.SendWebRequest();
        if (www.isHttpError || www.isNetworkError)
        {
            Debug.Log("Sending data to DB failed : " + www.error);
        }
        else
        {
            MyJSON json = new MyJSON();
            json.THI_onGameComplete(www.downloadHandler.text);

            Debug.Log("Sending data to DB success : " + www.downloadHandler.text);
        }
    }   
}
