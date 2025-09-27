using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;
using DLearners;
using UnityEngine.SceneManagement;

public class PS_Main : GameManagerBase
{
    [Header("Objects")]
    public GameObject G_Question;
    public GameObject G_Options;
    public GameObject G_OptionsBG;
    public GameObject[] GA_Options;
    public GameObject[] GA_OptionsBG;
    public Sprite[] SPRA_Questions;
    GameObject G_Highlight;
    public GameObject G_QuestionPrefab;
    public GameObject G_Clonehere;
       

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
        NextStep();
    }

    /// <summary>
    /// We are initialising the game after all the tutorial thing is completed. 
    /// </summary>
    public override void InitGame()
    {
        base.InitGame();

        currentQuestionID = 0;
       // THI_getPreviewData();

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
        // questionIMG.sprite = TarunTesting.Instance.dataSO.GetQuestionSprit(0);
    }

    /// <summary>
    /// Seting up Number of options for the game 
    /// </summary>
    private void SetUpOptionsPanel()
    {

        for (int i = 0; i < GA_Options.Length; i++)
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

        // THI_createOptions();


    }

    /// <summary>
    /// Showing transition and moving to next question, or checking for level complete 
    /// </summary>
    private void NextStep()
    {
        GetSetCurrentLevelData();
        // this.GetComponent<N_SwipeControls>().enabled = true;
        VaultPopUpsManager.Instance.ShowTransition(TransitionType.Fade);

        if (currentQuestionID < GameHandlerImmersiveGame.Instance.dataSO.datas.Count)
        {
            THI_NewQuestion();
            currentQuestionID++;
        }
        else
        {
            OnLevelCompleted();
        }
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
            STR_currentSelectedAnswer = G_Selected.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text;


            if (STR_currentSelectedAnswer == STR_currentQuestionAnswer)
            {
                G_Selected.transform.GetChild(0).GetComponent<AudioSource>().Play();
                CorrectAnswerSequence();

                for (int i = 0; i < G_Options.transform.childCount; i++)
                {
                    G_Options.transform.GetChild(i).gameObject.SetActive(false);
                }
                G_Selected.SetActive(true);
            }
            else { WrongAnswerSequence(); }
        }

    }

    

    public void THI_ShowQuestion()
    {
        for (int i = 0; i < G_Options.transform.childCount; i++)
        {
            G_Options.transform.GetChild(i).gameObject.SetActive(true);
        }
        G_Question.transform.GetChild(1).transform.GetChild(0).transform.GetChild(0).GetComponent<AudioSource>().Play();
    }



    public void THI_NewQuestion()
    {
        Invoke(nameof(THI_NextQuestion),0f);
    }

    public void THI_NextQuestion()
    {

        if (currentQuestionID < GameHandlerImmersiveGame.Instance.dataSO.datas.Count)
        {
            // STR_currentQuecurrentQuestionIDstionID = STRL_questionID[I_currentQuestionCount];

            HUDManager.Instance.UpdateQuestionCountText(currentQuestionID);//Tarun


            if (G_Question != null)
            {
                Destroy(G_Question);
            }

            // Debug.Log("Trying Instantiate");
            G_Question = Instantiate(G_QuestionPrefab);
            //  Debug.Log("Instantiate");
            G_Question.transform.SetParent(G_Clonehere.transform, false);
            G_Question.transform.position = G_Clonehere.transform.position;

            Transform tempTransform2 = G_Question.transform.GetChild(1).transform.GetChild(0).transform.GetChild(0);


            tempTransform2.GetComponent<Image>().sprite = currentData.questionData.questionSprit;
            tempTransform2.GetComponent<Image>().preserveAspect = true;
            tempTransform2.GetComponent<AudioSource>().clip = currentData.questionData.questionAudioClip;

            // I_Dummmy = I_Counter + IL_numbers[3];

            int cashLoop = G_Options.transform.childCount;
            for (int i = 0; i < cashLoop; i++)
            {
                G_Options.transform.GetChild(i).name = currentData.options[i].option;

                Transform tempTransform = G_Options.transform.GetChild(i).transform.GetChild(0).GetChild(0).transform;
                tempTransform.name = currentData.options[i].option;
                tempTransform.GetComponent<TextMeshProUGUI>().text = currentData.options[i].option;
                tempTransform.GetComponent<TextMeshProUGUI>().color = Color.black;
                tempTransform.GetComponent<AudioSource>().clip = currentData.options[i].optionAudioClip;
                G_Options.transform.GetChild(i).gameObject.SetActive(true);
            }

            //  I_Counter = I_Counter + IL_numbers[3];
            THI_ShowQuestion();
        }
        else
        {
            OnLevelCompleted();
            // Invoke(nameof(THI_Levelcompleted), 3f);
        }
    }



    public override void OnLevelCompleted()
    {
        base.OnLevelCompleted();
        StartCoroutine(IN_sendDataToDB());
    }


    public override void CorrectAnswerSequence()
    {
        base.CorrectAnswerSequence();
        DLearnersAudioManager.Instance.PlayCommonSound("Com_Correct");
        // I_Collect_count++;
        HUDManager.Instance.UpdateScoreText(true);

        // Release bird animation
        THI_TrackGameData("1");
        /* if (I_currentQuestionCount < STRL_questions.Count - 1)
         {
             Invoke(nameof(THI_OpenDam), 1f);
         }*/
        Invoke(nameof(NextStep), 3f);

    }




    /* void Highlight()
     {
         for (int i = 0; i < G_Options.transform.childCount; i++)
         {
             G_Options.transform.GetChild(i).gameObject.SetActive(false);
         }
         G_Highlight.SetActive(true);

     }*/

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
                NextStep();   // in 2 seconds

            }
            else if (currentDifficultyLevelType == DifficultyLevelType.Medium)
            {
                isInputUnLocked = false;
                NextStep();   // in 2 seconds
            }

            //next question
        }
        else if (currentWrongAnsCount == wrongAnsLifeCounts[1])//2
        {

            if (currentDifficultyLevelType == DifficultyLevelType.Hard)
            {
                isInputUnLocked = false;
                NextStep();   // in 2 seconds
            }

           // Invoke(nameof(THI_NextQuestion), 2f);
        }
        NextStep();//tarun
        HUDManager.Instance.UpdateScoreText(false);
    }



   
    void THI_playAudio()
    {
        EventSystem.current.currentSelectedGameObject.GetComponent<AudioSource>().Play();
        Debug.Log("player clicked. so playing audio");
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
    public void BUT_playAgain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


}
