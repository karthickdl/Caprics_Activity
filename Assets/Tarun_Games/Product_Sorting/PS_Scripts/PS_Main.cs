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
    [Header("Screens and UI elements")]
    public TextMeshProUGUI TEXM_instruction;
    public Text TEX_points;
    public Text TEX_questionCount;
    public TextMeshProUGUI TM_pointFx;

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

   

    [Header("Values")]
    public int I_Points;
    public int I_wrongAnsCount;
    public int I_Counter, I_Dummmy;
    // public int I_Collect_count;


    [Header("URL")]
    public string URL;
    public string SendValueURL;

    [Header("Audios")]
    public AudioSource AS_collecting;
    public AudioSource AS_oops;
    public AudioSource AS_crtans;

    [Header("DB")]
    public List<string> STRL_difficulty;
    public string STR_difficulty;
    public int I_correctPoints;
    public int I_wrongPoints;
    public List<string> STRL_instruction;
    public string STR_instruction;
    public string STR_video_link;
    public List<string> STRL_options;
    public List<string> STRL_answers;
    public List<string> STRL_quesitonAudios;
    public List<string> STRL_optionAudios;
    public List<string> STRL_instructionAudio;
    public List<string> STRL_questionID;
    public string STR_customizationKey;
    //Dummy values only for helicopter game
    public List<string> STRL_BG_img_link;
    public List<string> STRL_avatar_Color;
    public List<string> STRL_Panel_Img_link;
    public List<string> STRL_Cover_Image_link;
    public List<string> STRL_passageDetail;


    [Header("AUDIO ASSIGN")]
    public AudioClip[] ACA__questionClips;
    public AudioClip[] ACA_optionClips;
    public AudioClip[] ACA_instructionClips;


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
        THI_Transition();
    }



    /// <summary>
    /// We are initialising the game after all the tutorial thing is completed. 
    /// </summary>
    public override void InitGame()
    {
        base.InitGame();

        Invoke("THI_gameData", 1f);

        I_Dummmy = 0;
        I_Counter = 0;
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

        THI_getPreviewData();
        // questionIMG.sprite = TarunTesting.Instance.dataSO.GetQuestionSprit(0);
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

    void THI_Transition()
    {
        GetSetCurrentLevelData();
        // this.GetComponent<N_SwipeControls>().enabled = true;
        VaultPopUpsManager.Instance.ShowTransition(TransitionType.Fade);
        THI_NewQuestion();
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
        currentQuestionID++;
        I_wrongAnsCount = 0;
        Invoke(nameof(THI_NextQuestion), 2f);
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

            G_Question.transform.GetChild(1).transform.GetChild(0).transform.GetChild(0).GetComponent<Image>().sprite = currentData.questionData.questionSprit;
            G_Question.transform.GetChild(1).transform.GetChild(0).transform.GetChild(0).GetComponent<Image>().preserveAspect = true;
            G_Question.transform.GetChild(1).transform.GetChild(0).transform.GetChild(0).GetComponent<AudioSource>().clip = currentData.questionData.questionAudioClip;

            // I_Dummmy = I_Counter + IL_numbers[3];

            for (int i = 0; i < G_Options.transform.childCount; i++)
            {
                G_Options.transform.GetChild(i).name = currentData.options[i].option;

                
                G_Options.transform.GetChild(i).transform.GetChild(0).transform.GetChild(0).name = currentData.options[i].option;
                G_Options.transform.GetChild(i).transform.GetChild(0).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = currentData.options[i].option;
                G_Options.transform.GetChild(i).transform.GetChild(0).transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.black;
                G_Options.transform.GetChild(i).transform.GetChild(0).transform.GetChild(0).GetComponent<AudioSource>().clip = currentData.options[i].optionAudioClip;
                G_Options.transform.GetChild(i).gameObject.SetActive(true);
            }



            //  I_Counter = I_Counter + IL_numbers[3];



            THI_ShowQuestion();
        }
        else
        {
            THI_Levelcompleted();
            // Invoke(nameof(THI_Levelcompleted), 3f);
        }
    }



    void THI_Levelcompleted()
    {
        DLearners.GameHandlerImmersiveGame.Instance.I_TotalPoints = I_Points;
        VaultPopUpsManager.Instance.ShowPopup(NormalPopUpTypes.LevelCompletePOPUP);
        StartCoroutine(IN_sendDataToDB());
    }


    public override void CorrectAnswerSequence()
    {
        base.CorrectAnswerSequence();
        AS_crtans.Play();
        // I_Collect_count++;
        I_Points += I_correctPoints;
        TEX_points.text = I_Points.ToString();
        THI_pointFxOn(true);

        // Release bird animation
        THI_TrackGameData("1");
        /* if (I_currentQuestionCount < STRL_questions.Count - 1)
         {
             Invoke(nameof(THI_OpenDam), 1f);
         }*/
        Invoke(nameof(THI_NewQuestion), 3f);

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
                THI_Transition();   // in 2 seconds

            }
            else if (currentDifficultyLevelType == DifficultyLevelType.Medium)
            {
                isInputUnLocked = false;
                THI_Transition();   // in 2 seconds
            }

            //next question
        }
        else if (currentWrongAnsCount == wrongAnsLifeCounts[1])//2
        {

            if (currentDifficultyLevelType == DifficultyLevelType.Hard)
            {
                isInputUnLocked = false;
                THI_Transition();   // in 2 seconds
            }

            AS_oops.Play();
            Invoke(nameof(THI_NextQuestion), 2f);
        }

        HUDManager.Instance.UpdateScoreText(false);
    }


    public void THI_CoinCollected()
    {
        AS_collecting.Play();
        TM_pointFx.text = "+ 2 points";

        I_Points += 2;

        TEX_points.text = I_Points.ToString();

        Invoke("THI_pointFxOff", 1f);
    }

    public void THI_pointFxOn(bool plus)
    {
        if (plus)
        {
            if (I_correctPoints != 1)
            {
                TM_pointFx.text = "+" + I_correctPoints + " points";
            }
            else
            {
                TM_pointFx.text = "+" + I_correctPoints + " point";
            }
        }
        else
        {
            if (I_Points > 0)
            {
                if (I_wrongPoints != 0)
                {
                    if (I_wrongPoints != 1)
                    {
                        TM_pointFx.text = "-" + I_wrongPoints + " points";
                    }
                    else
                    {
                        TM_pointFx.text = "-" + I_wrongPoints + " point";
                    }
                }
            }
        }
        Invoke("THI_pointFxOff", 1f);
    }
    public void THI_pointFxOff()
    {
        TM_pointFx.text = "";
    }
    

    public IEnumerator EN_getValues()
    {

            for (int i = 0; i < GA_Options.Length; i++)
            {
                GA_Options[i].SetActive(false);
                GA_OptionsBG[i].SetActive(false);
            }
            if (STRL_options.Count == 2)
            {
                G_Options = GA_Options[0];
                G_OptionsBG = GA_OptionsBG[0];
                //  Debug.Log(G_Options.name);
            }
            if (STRL_options.Count == 3)
            {
                G_Options = GA_Options[1];
                G_OptionsBG = GA_OptionsBG[1];
                //  Debug.Log(G_Options.name);
            }

            G_Options.SetActive(true);
            G_OptionsBG.SetActive(true);

            StartCoroutine(EN_getAudioClips());
        yield return null;
    }

   
    public IEnumerator EN_getAudioClips()
    {
        ACA__questionClips = new AudioClip[STRL_quesitonAudios.Count];
        ACA_optionClips = new AudioClip[STRL_optionAudios.Count];
        ACA_instructionClips = new AudioClip[STRL_instructionAudio.Count];

        for (int i = 0; i < STRL_quesitonAudios.Count; i++)
        {
            UnityWebRequest www1 = UnityWebRequestMultimedia.GetAudioClip(STRL_quesitonAudios[i], AudioType.MPEG);
            yield return www1.SendWebRequest();
            if (www1.result == UnityWebRequest.Result.ConnectionError || www1.isHttpError || www1.isNetworkError)
            {
                Debug.Log(www1.error);
            }
            else
            {
                ACA__questionClips[i] = DownloadHandlerAudioClip.GetContent(www1);
            }
        }

        for (int i = 0; i < STRL_optionAudios.Count; i++)
        {
            UnityWebRequest www2 = UnityWebRequestMultimedia.GetAudioClip(STRL_optionAudios[i], AudioType.MPEG);
            yield return www2.SendWebRequest();
            if (www2.result == UnityWebRequest.Result.ConnectionError || www2.isHttpError || www2.isNetworkError)
            {
                Debug.Log(www2.error);
            }
            else
            {
                ACA_optionClips[i] = DownloadHandlerAudioClip.GetContent(www2);
            }
        }


        for (int i = 0; i < STRL_instructionAudio.Count; i++)
        {
            UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(STRL_instructionAudio[i], AudioType.MPEG);
            yield return www.SendWebRequest();
            if (www.result == UnityWebRequest.Result.ConnectionError || www.isHttpError || www.isNetworkError)
            {
                Debug.Log(www.error);
            }
            else
            {

                ACA_instructionClips[i] = DownloadHandlerAudioClip.GetContent(www);
                Debug.Log("audio clips fetched instruction");

            }
        }
        THI_assignAudioClips();
    }

    void THI_assignAudioClips()
    {
        if (ACA_instructionClips.Length > 0)
        {
            TEXM_instruction.text = STR_instruction;
            TEXM_instruction.gameObject.AddComponent<AudioSource>();
            TEXM_instruction.gameObject.GetComponent<AudioSource>().playOnAwake = false;
            TEXM_instruction.gameObject.GetComponent<AudioSource>().clip = ACA_instructionClips[0];
            TEXM_instruction.gameObject.AddComponent<Button>();
            TEXM_instruction.gameObject.GetComponent<Button>().onClick.AddListener(THI_playAudio);


        }

        // DemoOver();//remove later
        // THI_Transition();
    }
    void THI_playAudio()
    {
        EventSystem.current.currentSelectedGameObject.GetComponent<AudioSource>().Play();
        Debug.Log("player clicked. so playing audio");
    }
    public void THI_getPreviewData()
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
        StartCoroutine(EN_getAudioClips());

        // THI_createOptions();

        
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

        UnityWebRequest www = UnityWebRequest.Post(SendValueURL, form);
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
