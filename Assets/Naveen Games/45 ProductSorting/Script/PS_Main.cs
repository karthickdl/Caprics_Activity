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

    public bool B_production;

    [Header("Screens and UI elements")]
    bool B_CloseDemo;

    public GameObject G_Game;
    public GameObject G_coverPage;
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

    // public bool B_MoveUp, B_MoveDown, B_MoveForward;
    // public List<string> Lstr_ans, Lstr_wrng;
    // public List<AudioClip> AC_ans, AC_wrg;
    bool B_CanClick;
    // public GameObject G_Player;

    [Header("Values")]
    public int I_currentQuestionCount; // question number current
    public string STR_currentQuestionID;
    public int I_Points;
    public int I_wrongAnsCount;
    public int I_Counter, I_Dummmy;
    public string[] STRA_AnsList;
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
    public List<int> IL_numbers;
    public int I_correctPoints;
    public int I_wrongPoints;
    public List<string> STRL_instruction;
    public string STR_instruction;
    public string STR_video_link;
    public List<string> STRL_options;
    public List<string> STRL_questions;
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

    [Header("GAME DATA")]
    public List<string> STRL_gameData;
    public string STR_Data;

    [Header("LEVEL COMPLETE")]
    public GameObject G_levelComplete;

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
        Robotmovement.Instance.OnPlayButton();
    }



    /// <summary>
    /// We are initialising the game after all the tutorial thing is completed. 
    /// </summary>
    public override void InitGame()
    {
        base.InitGame();

        Invoke("THI_gameData", 1f);

        I_currentQuestionCount = -1;
        I_Dummmy = 0;
        I_Counter = 0;


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



    void Start()
    {
        // B_CloseDemo = true;


        // G_Player.SetActive(false);
        // G_Game.SetActive(false);
        // G_levelComplete.SetActive(false);

        // TEX_points.text = I_Points.ToString();
        // STRL_questions = new List<string>();
        // STRL_answers = new List<string>();
        // STRL_options = new List<string>();
        // Invoke("THI_gameData", 1f);

        // I_currentQuestionCount = -1;
        // I_Dummmy = 0;
        // I_Counter = 0;
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
                THI_Correct();

                for (int i = 0; i < G_Options.transform.childCount; i++)
                {
                    G_Options.transform.GetChild(i).gameObject.SetActive(false);
                }
                G_Selected.SetActive(true);
            }
            else { THI_WrongEffect(); }
        }

    }
    void THI_gameData()
    {
        // THI_getPreviewData();
        if (DLearners.GameHandlerImmersiveGame.Instance.mode == "live")
        {
            StartCoroutine(EN_getValues()); // live game in portal
        }
        if (DLearners.GameHandlerImmersiveGame.Instance.mode == "preview")
        {
            // preview data in html game generator

            Debug.Log("PREVIEW MODE RAKESH");
            THI_getPreviewData();
        }
    }

    public void DemoOver()
    {
        G_Game.SetActive(true);
    }
    void THI_Transition()
    {
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
        I_currentQuestionCount++;
        I_wrongAnsCount = 0;
        Invoke(nameof(THI_NextQuestion), 2f);
    }

    public void THI_NextQuestion()
    {

        if (I_currentQuestionCount < STRL_questions.Count)
        {
            // I_currentQuestionCount++;
            //  Debug.Log("THI_NextQuestion =" + I_currentQuestionCount);

            STRA_AnsList = null;
            STR_currentQuestionID = STRL_questionID[I_currentQuestionCount];
            int currentquesCount = I_currentQuestionCount + 1;
            TEX_questionCount.text = currentquesCount + "/" + STRL_questions.Count;
            STR_currentQuestionAnswer = STRL_answers[I_currentQuestionCount];

            if (G_Question != null)
            {
                Destroy(G_Question);
            }

            // Debug.Log("Trying Instantiate");
            G_Question = Instantiate(G_QuestionPrefab);
            //  Debug.Log("Instantiate");
            G_Question.transform.SetParent(G_Clonehere.transform, false);
            G_Question.transform.position = G_Clonehere.transform.position;
            // G_Question = G_QuestionPrefab.transform.GetChild(1).gameObject;

            /*G_Question.transform.GetChild(0).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = STRL_questions[I_currentQuestionCount];
            G_Question.transform.GetChild(0).transform.GetChild(0).GetComponent<AudioSource>().clip = ACA__questionClips[I_currentQuestionCount];*/

            G_Question.transform.GetChild(1).transform.GetChild(0).transform.GetChild(0).GetComponent<Image>().sprite = SPRA_Questions[I_currentQuestionCount];
            G_Question.transform.GetChild(1).transform.GetChild(0).transform.GetChild(0).GetComponent<Image>().preserveAspect = true;
            G_Question.transform.GetChild(1).transform.GetChild(0).transform.GetChild(0).GetComponent<AudioSource>().clip = ACA__questionClips[I_currentQuestionCount];

            // I_Dummmy = I_Counter + IL_numbers[3];

            for (int i = 0; i < G_Options.transform.childCount; i++)
            {
                G_Options.transform.GetChild(i).name = STRL_options[i];
                G_Options.transform.GetChild(i).transform.GetChild(0).transform.GetChild(0).name = STRL_options[i];
                G_Options.transform.GetChild(i).transform.GetChild(0).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = STRL_options[i];
                G_Options.transform.GetChild(i).transform.GetChild(0).transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.black;
                G_Options.transform.GetChild(i).transform.GetChild(0).transform.GetChild(0).GetComponent<AudioSource>().clip = ACA_optionClips[i];
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
        G_levelComplete.SetActive(true);
        StartCoroutine(IN_sendDataToDB());
    }


    public void THI_Correct()
    {
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

    public override void THI_WrongEffect()
    {
        base.THI_WrongEffect();
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
    public IEnumerator IN_CoverImage()
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(STRL_Cover_Image_link[0]);
        yield return www.SendWebRequest();
        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Texture2D downloadedTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;
            if (STRL_Cover_Image_link != null)
            {
                G_coverPage.GetComponent<Image>().sprite = Sprite.Create(downloadedTexture, new Rect(0.0f, 0.0f, downloadedTexture.width, downloadedTexture.height), new Vector2(0.5f, 0.5f), 100.0f);
            }
        }

        //SPRA_Options

    }

    public IEnumerator EN_getValues()
    {
        WWWForm form = new WWWForm();
        form.AddField("game_id", DLearners.GameHandlerImmersiveGame.Instance.STR_GameID);
        // Debug.Log("GAME ID : " + DLearners.TarunTesting.Instance.STR_GameID);
        UnityWebRequest www = UnityWebRequest.Post(URL, form);
        yield return www.SendWebRequest();
        if (www.isHttpError || www.isNetworkError)
        {
            Debug.Log(www.error);
        }
        else
        {
            List<string> STRL_Passagedetails = new List<string>();
            MyJSON json = new MyJSON();
            //json.Helitemp(www.downloadHandler.text);
            json.Temp_type_1(www.downloadHandler.text, IL_numbers, STRL_difficulty, STRL_instruction, STRL_BG_img_link, STRL_instructionAudio, STRL_questions,
                STRL_answers, STRL_quesitonAudios, STRL_questionID, STRL_options, STRL_optionAudios, STRL_avatar_Color, STRL_Panel_Img_link, STRL_Cover_Image_link, STRL_passageDetail);
            //        Debug.Log("GAME DATA : " + www.downloadHandler.text);

            STR_difficulty = STRL_difficulty[0];

            STR_instruction = STRL_instruction[0];
            //MainController.instance.I_correctPoints = I_correctPoints = IL_numbers[1];//Tarun
            I_wrongPoints = IL_numbers[2];
            DLearners.GameHandlerImmersiveGame.Instance.I_TotalQuestions = STRL_questions.Count;

            Debug.Log("Que = " + STRL_questions.Count + "Opt = " + STRL_options.Count);

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
            StartCoroutine(IN_CoverImage());
            StartCoroutine(IMG_Options());

        }
    }

    public IEnumerator IMG_Options()
    {

        SPRA_Questions = new Sprite[STRL_questions.Count];

        for (int i = 0; i < STRL_questions.Count; i++)
        {
            UnityWebRequest www = UnityWebRequestTexture.GetTexture(STRL_questions[i]);
            yield return www.SendWebRequest();
            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Texture2D downloadedTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;

                SPRA_Questions[i] = Sprite.Create(downloadedTexture, new Rect(0.0f, 0.0f, downloadedTexture.width, downloadedTexture.height), new Vector2(0.5f, 0.5f), 100.0f);

                string[] Names = (STRL_questions[i].Split('/'));
                string[] Finalname = (Names[Names.Length - 1].Split('.'));

                SPRA_Questions[i].name = Finalname[0];


            }
        }
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
        List<string> STRL_Passagedetails = new List<string>();
        MyJSON json = new MyJSON();
        //  json.Helitemp(DLearners.TarunTesting.Instance.STR_previewJsonAPI);
        json.Temp_type_1(DLearners.GameHandlerImmersiveGame.Instance.STR_previewJsonAPI, IL_numbers, STRL_difficulty, STRL_instruction, STRL_BG_img_link, STRL_instructionAudio, STRL_questions,
                 STRL_answers, STRL_quesitonAudios, STRL_questionID, STRL_options, STRL_optionAudios, STRL_avatar_Color, STRL_Panel_Img_link, STRL_Cover_Image_link, STRL_passageDetail);

        STR_difficulty = STRL_difficulty[0];
        STR_instruction = STRL_instruction[0];
        //MainController.instance.I_correctPoints = I_correctPoints = IL_numbers[1];//Tarun
        I_wrongPoints = IL_numbers[2];
        DLearners.GameHandlerImmersiveGame.Instance.I_TotalQuestions = STRL_questions.Count;

        for (int i = 0; i < GA_Options.Length; i++)
        {
            GA_Options[i].SetActive(false);
        }
        if (IL_numbers[3] == 2)
        {
            G_Options = GA_Options[0];
        }
        if (IL_numbers[3] == 3)
        {
            G_Options = GA_Options[1];
        }
        if (IL_numbers[3] == 4)
        {
            G_Options = GA_Options[2];
        }
        if (IL_numbers[3] == 5)
        {
            G_Options = GA_Options[3];
        }
        G_Options.SetActive(true);
        StartCoroutine(EN_getAudioClips());
        StartCoroutine(IN_CoverImage());
        StartCoroutine(IMG_Options());

        // THI_createOptions();
    }
    public void THI_TrackGameData(string analysis)
    {
        DBmanager TrainSortingDB = new DBmanager();
        TrainSortingDB.question_id = STR_currentQuestionID;
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
