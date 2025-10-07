using DG.Tweening;
using DLearners;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.UI;

public class FiremanController : GameManagerBase
{
    [Header("DB")]
    int calledonce;


    [Header("Questions")]
    public string STR_currentQuestionID;
    public int I_wrongAnsCount;
    public string STR_currentQuestion;
    public string STR_currentCorrectAnswer;
    public string STR_clickedAnswer;
    public int I_currentQuestionCount;
    public int I_currentOptionReqCount;
    public int I_lastOptionReqCount;
    public GameObject G_questionScreen;
    public Text TEX_currentQuestion;
    public GameObject G_ansDisplay;
    public Image IM_answerDisplay;
    public Text TEX_annswerDisplay;
    public GameObject[] GA_options;
    public GameObject G_levelComp;
    int I_1stSprite;
    int I_2ndSprite;
    int I_3rdSprite;
    string STR_1stOptionName;
    string STR_2ndOptionName;
    string STR_3rdOptionName;
    bool B_called;
    int z;


   

    [Header("Extinguish")]
    public int[] IA_FireCount;
    public int I_fireCount;
    public int I_fire;
    
    
    

    [Header("Health")]
    public Vector2 SpawnPos;

    

    [Header("Initialization")]
    public AnimationClip AC_controlsAnim;
    public bool B_dogRun;

   

    [Header("Audios")]
    public AudioSource AS_baby;
    public AudioSource AS_siren;
    public AudioSource AS_correct;
    public AudioSource AS_wrong;
    public AudioSource AS_dogbark;
    public AudioSource AS_BGM;


    public GameObject G_bgm;
    public GameObject G_skip;


    [Header ("Cam")]
    public Camera firemancamera;

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
        firemancamera.GetComponent<Animator>().enabled = true;
    }

    /// <summary>
    /// We are initialising the game after all the tutorial thing is completed. 
    /// </summary>
    public override void InitGame()
    {
        base.InitGame();

        NextStep();

       
        SpawnPos = transform.position;
        I_currentQuestionCount = -1;
        int curqcount = I_currentQuestionCount + 1;
        //TEX_questionCount.text = "" + curqcount + "/" + I_totalQuestionCount;tarun

        //G_controlButtons.SetActive(false);//Taru8n
        THI_addAudios();

        Invoke("THI_gameData", 1f);
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
    }

    /// <summary>
    /// Showing transition and moving to next question, or checking for level complete 
    /// </summary>
    protected override void NextStep()
    {
        VaultPopUpsManager.Instance.ShowTransition(TransitionType.Fade);
        if (currentQuestionID < GameHandlerImmersiveGame.Instance.dataSO.datas.Count)
        {
            GetSetCurrentLevelData();
           // G_Question.SetActive(false);
          //  ShowCurrentQuestion();
        }
        else
        {
            OnLevelCompleted();
        }
        base.NextStep();//Need to be called after GetSetCurrentLevelData
    }

    void THI_enableControlButtons()
    {
        //G_controlButtons.SetActive(true);//Tarun

        AS_baby.Stop();
        AS_siren.Play();
        Invoke("THI_enableClickonButtons", AC_controlsAnim.length);
    }

    void THI_enableClickonButtons()
    {
       /* Button[] controlbuttons = G_controlButtons.GetComponentsInChildren<Button>();
        for (int i = 0; i < controlbuttons.Length; i++)
        {
            controlbuttons[i].enabled = true;
        }*///Tarun
        B_dogRun = true;
        AS_dogbark.Play();
        Invoke(nameof(stopExtraAudios), 10f);
        G_skip.SetActive(false);

        if (currentQuestionID < GameHandlerImmersiveGame.Instance.dataSO.datas.Count)
        {
            Debug.LogWarning("Assign THI_enableClickonButtons");
            I_fire = 0;
            I_fireCount = IA_FireCount[I_currentQuestionCount + 1];
        }

        G_bgm.SetActive(true);
        StartCoroutine(OnStartBirdFly());
        int curqcount = I_currentQuestionCount + 1;
        //TEX_questionCount.text = "" + curqcount + "/" + I_totalQuestionCount;Tarun
        HUDManager.Instance.UpdateQuestionCountText(currentQuestionID);//Tarun
    }

    void stopExtraAudios()
    {
        AS_dogbark.Stop();
        AS_siren.Stop();
    }

   

        

    void THI_gameData()
    {
        // THI_getPreviewData();
       // if (MainController.instance.mode == "live")
        {
           // StartCoroutine(EN_GetData()); // live game in portal
        }
      //  if (MainController.instance.mode == "preview")
        {
            // preview data in html game generator

           // THI_getPreviewData();
        }
    }
   /* public IEnumerator EN_getAudioClips()
    {
        ACA__questionClips = new AudioClip[STRL_questionAudios.Count];

        for (int i = 0; i < STRL_questionAudios.Count; i++)
        {
            UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(STRL_questionAudios[i], AudioType.MPEG);
            yield return www.SendWebRequest();
            if (www.result == UnityWebRequest.Result.ConnectionError || www.isHttpError || www.isNetworkError)
            {
                Debug.Log( STRL_questionAudios[i] +" => " + www.error);
            }
            else
            {
                if (DownloadHandlerAudioClip.GetContent(www) != null)
                {
                    ACA__questionClips[i] = DownloadHandlerAudioClip.GetContent(www);

                    string[] Names = (STRL_questionAudios[i].Split('/'));
                    string[] Finalname = (Names[Names.Length - 1].Split('.'));
                    ACA__questionClips[i].name = Finalname[0];


                    Debug.Log(ACA__questionClips[i] + " => " + ACA__questionClips[i].name);

                  

                }
            }
        }

    }
    public IEnumerator EN_getAudioClips1()
    {
        ACA_optionClips = new AudioClip[STRL_optionsAudios.Count];
       
        for (int i = 0; i < STRL_optionsAudios.Count; i++)
        {
            UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(STRL_optionsAudios[i], AudioType.MPEG);
            yield return www.SendWebRequest();
            if (www.result == UnityWebRequest.Result.ConnectionError || www.isHttpError || www.isNetworkError)
            {
                //Debug.Log(www.error);
                Debug.Log(STRL_optionsAudios[i] + " => " + www.error);
            }
            else
            {
                if (DownloadHandlerAudioClip.GetContent(www) != null)
                {
                    ACA_optionClips[i] = DownloadHandlerAudioClip.GetContent(www);

                    string[] Names = (STRL_optionsAudios[i].Split('/'));
                    string[] Finalname = (Names[Names.Length - 1].Split('.'));
                    ACA_optionClips[i].name = Finalname[0];


                    Debug.Log(ACA_optionClips[i] + " => " + ACA_optionClips[i].name);

                    Debug.Log("RAK LENGTH OPTION CLIPS : " + ACA_optionClips.Length + "/" + 30);
                }
            }
        }

    }
    public IEnumerator EN_getAudioClips2()
    {
        ACA_instructionClips = new AudioClip[STRL_instructionAudios.Count];

        for (int i = 0; i < STRL_instructionAudios.Count; i++)
        {
            UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(STRL_instructionAudios[i], AudioType.MPEG);
            yield return www.SendWebRequest();
            if (www.result == UnityWebRequest.Result.ConnectionError || www.isHttpError || www.isNetworkError)
            {
                // Debug.Log(www.error);
                Debug.Log(STRL_instructionAudios[i] + " => " + www.error);
            }
            else
            {
                if (DownloadHandlerAudioClip.GetContent(www) != null)
                {
                    ACA_instructionClips[i] = DownloadHandlerAudioClip.GetContent(www);


                    string[] Names = (STRL_instructionAudios[i].Split('/'));
                    string[] Finalname = (Names[Names.Length - 1].Split('.'));
                    ACA_instructionClips[i].name = Finalname[0];

                    Debug.Log(ACA_instructionClips[i] + " => " + ACA_instructionClips[i].name);

                    Debug.Log("RAK LENGTH INSTRUCTION CLIP : " + ACA_instructionClips.Length + "/" + 1);
                }
            }
        }
    }*///Tarun

    public void THI_addAudios()
    {
        TEX_currentQuestion.gameObject.AddComponent<AudioSource>();
        TEX_currentQuestion.gameObject.GetComponent<AudioSource>().playOnAwake = true;

        TEX_currentQuestion.gameObject.AddComponent<Button>();
        TEX_currentQuestion.gameObject.GetComponent<Button>().onClick.AddListener(THI_playAudio);


        for (int i = 0; i < GA_options.Length; i++)
        {
            GA_options[i].gameObject.GetComponent<Button>().onClick.AddListener(THI_playAudio);
        }
    }

    void THI_playAudio()
    {
        EventSystem.current.currentSelectedGameObject.GetComponent<AudioSource>().Play();
    }


    void THI_disableAnswerDisplay()
    {
        G_ansDisplay.SetActive(false);
    }

    public void BUT_optionClick()
    {
        string clickedanswer = EventSystem.current.currentSelectedGameObject.name;
        string[] clickedanswersplit = clickedanswer.Split('/');
        int lastelement = clickedanswersplit.Length - 1;
        string lastelementanswer = clickedanswersplit[lastelement];
        string[] lastelementsplit = lastelementanswer.Split('.');
        STR_clickedAnswer = lastelementsplit[0];



        if (STR_clickedAnswer.Contains(STR_currentCorrectAnswer))
        {
            //correct
            AS_correct.Play();

            if (currentQuestionID < GameHandlerImmersiveGame.Instance.dataSO.datas.Count)
            {
                I_fire = 0;
                I_fireCount = IA_FireCount[I_currentQuestionCount + 1];
            }
            HUDManager.Instance.UpdateScoreText(true);
            G_ansDisplay.SetActive(true);
            IM_answerDisplay.sprite = EventSystem.current.currentSelectedGameObject.GetComponent<Image>().sprite;
            IM_answerDisplay.preserveAspect = true;
            TEX_annswerDisplay.text = STR_currentCorrectAnswer;
            THI_TrackGameData("1");
            Invoke("disableAnswer", 3f);
        }
        else
        {
            //wrong
            AS_wrong.Play();
            THI_TrackGameData("0");
            HUDManager.Instance.UpdateScoreText(false);


            I_wrongAnsCount++;
            if (I_wrongAnsCount == 3)
            {
                if (currentDifficultyLevelType == DifficultyLevelType.Easy)
                {
                    // show ans and go to next question
                    G_ansDisplay.SetActive(true);
                    for (int i = 0; i < GA_options.Length; i++)
                    {
                        //if (GA_options[i].name.Contains(STRL_answersDB[I_currentQuestionCount]))//Tarun
                        if (GA_options[i].name.Contains(currentData.correctOptions))
                        {
                            IM_answerDisplay.sprite = GA_options[i].GetComponent<Image>().sprite;
                        }

                        TEX_annswerDisplay.text = currentData.correctOptions;
                        //TEX_annswerDisplay.text = STRL_answersDB[I_currentQuestionCount];//Tarun

                        Invoke("disableAnswer", 2.5f);

                    }
                    if (currentDifficultyLevelType == DifficultyLevelType.Medium)
                    {
                        // show ans and make the child click to go to next question
                        G_ansDisplay.SetActive(true);
                        for (int i = 0; i < GA_options.Length; i++)
                        {
                            //if (GA_options[i].name.Contains(STRL_answersDB[I_currentQuestionCount]))//Tarun
                            if (GA_options[i].name.Contains(currentData.correctOptions))
                            {
                                IM_answerDisplay.sprite = GA_options[i].GetComponent<Image>().sprite;
                            }
                        }
                        //TEX_annswerDisplay.text = STRL_answersDB[I_currentQuestionCount];Tarun
                        TEX_annswerDisplay.text = currentData.correctOptions;
                        Invoke("THI_disableAnswerDisplay", 2.5f);
                    }
                }
                if (I_wrongAnsCount == 2)
                {
                    if (currentDifficultyLevelType == DifficultyLevelType.Hard)
                    {
                        // dont show ans and go to next question
                        disableAnswer();

                    }
                }
            }
        }
    }
    public override void OnLevelCompleted()
    {
        base.OnLevelCompleted();
        // MainController.instance.I_TotalPoints = I_points;
        G_levelComp.SetActive(true);
        //AS_BGM.Stop();
       // if (MainController.instance.mode == "live")
        {
            StartCoroutine(IN_sendDataToDB());
        }
    }
    void disableAnswer()
    {
        G_questionScreen.SetActive(false);
    }

    public void THI_showQuestion()
    {
        PlayerController.Instance.extinguishButton.interactable = false;//Tarun
        I_wrongAnsCount = 0;
        G_questionScreen.SetActive(true);
        G_ansDisplay.SetActive(false);
        I_1stSprite = I_2ndSprite = I_3rdSprite = 99;  //  default sprite val for checking and assigning in for loop
        STR_1stOptionName = STR_2ndOptionName = STR_3rdOptionName = ""; //  default name val for checking and assigning in for loop


        if (B_called)
        {
            I_currentQuestionCount++;
            I_currentOptionReqCount += 3;
        }
        else
        {
            I_currentQuestionCount++;
            I_currentOptionReqCount += 2;
        }

        if (currentQuestionID < GameHandlerImmersiveGame.Instance.dataSO.datas.Count)
        {
            //game still running
            int curqcount = I_currentQuestionCount + 1;
           // TEX_questionCount.text = curqcount + "/" + I_totalQuestionCount;Tarun
            HUDManager.Instance.UpdateQuestionCountText(currentQuestionID);//Tarun
           /* STR_currentQuestionID = STRL_questionID[I_currentQuestionCount];
            STR_currentCorrectAnswer = STRL_answersDB[I_currentQuestionCount];
            STR_currentQuestion = STRL_questionsDB[I_currentQuestionCount];*///Tarun
            TEX_currentQuestion.text = STR_currentQuestion;
            if (TEX_currentQuestion.gameObject.GetComponent<AudioSource>() != null)
            {
                // TEX_currentQuestion.gameObject.GetComponent<AudioSource>().clip = ACA__questionClips[I_currentQuestionCount];//Tarun
                TEX_currentQuestion.gameObject.GetComponent<AudioSource>().clip = currentData.options[currentQuestionID].optionAudioClip;
                TEX_currentQuestion.gameObject.GetComponent<AudioSource>().Play();
            }
            if (B_called)
            {
                z = I_lastOptionReqCount + 1;
            }
            else
            {
                z = 0;
            }
            B_called = true;



            for (int i = z; i <= I_currentOptionReqCount; i++)
            {
                if (I_1stSprite != 99 && I_2ndSprite != 99 && I_3rdSprite == 99)
                {
                    I_3rdSprite = i;
                }
                if (I_1stSprite != 99 && I_2ndSprite == 99)
                {
                    I_2ndSprite = i;
                }
                if (I_1stSprite == 99)
                {
                    I_1stSprite = i;
                }

               /* if (STR_1stOptionName != "" && STR_2ndOptionName != "" && STR_3rdOptionName == "")
                {
                    STR_3rdOptionName = STRL_optionsSpriteName[i];
                }
                if (STR_1stOptionName != "" && STR_2ndOptionName == "")
                {
                    STR_2ndOptionName = STRL_optionsSpriteName[i];
                }
                if (STR_1stOptionName == "")
                {
                    STR_1stOptionName = STRL_optionsSpriteName[i];
                }*///Tarun
            }

            Debug.Log("RAK 1ST SPRITE INDEX : " + I_1stSprite);
            Debug.Log("RAK 2ND SPRITE INDEX : " + I_2ndSprite);
            Debug.Log("RAK 3RD SPRITE INDEX : " + I_3rdSprite);

            Debug.Log("RAK 1ST SPRITE NAME : " + currentData.options[I_1stSprite].optionSprit.name);
            Debug.Log("RAK 2ND SPRITE NAME : " + currentData.options[I_2ndSprite].optionSprit.name);
            Debug.Log("RAK 3RD SPRITE NAME : " + currentData.options[I_3rdSprite].optionSprit.name);

            GA_options[0].GetComponent<Image>().sprite = currentData.options[I_1stSprite].optionSprit;
            GA_options[1].GetComponent<Image>().sprite = currentData.options[I_2ndSprite].optionSprit;
            GA_options[2].GetComponent<Image>().sprite = currentData.options[I_3rdSprite].optionSprit;

            Debug.Log("RAK 1ST OPTION : " + GA_options[0].GetComponent<Image>().sprite.name);
            Debug.Log("RAK 2ND OPTION : " + GA_options[1].GetComponent<Image>().sprite.name);
            Debug.Log("RAK 3RD OPTION : " + GA_options[2].GetComponent<Image>().sprite.name);

            GA_options[0].name = STR_1stOptionName;
            GA_options[1].name = STR_2ndOptionName;
            GA_options[2].name = STR_3rdOptionName;


           // GA_options[0].GetComponent<AudioSource>().clip = ACA_optionClips[I_1stSprite];//Tarun
            GA_options[0].GetComponent<AudioSource>().clip = currentData.options[I_1stSprite].optionAudioClip;
            GA_options[1].GetComponent<AudioSource>().clip = currentData.options[I_2ndSprite].optionAudioClip;
            GA_options[2].GetComponent<AudioSource>().clip = currentData.options[I_3rdSprite].optionAudioClip;

            Debug.Log("RAK 1ST OPTION CLIP : " + GA_options[0].GetComponent<AudioSource>().clip.name);
            Debug.Log("RAK 2ND OPTION CLIP : " + GA_options[1].GetComponent<AudioSource>().clip.name);
            Debug.Log("RAK 3RD OPTION CLIP : " + GA_options[2].GetComponent<AudioSource>().clip.name);


            I_lastOptionReqCount = I_currentOptionReqCount;


            PlayerController.Instance.ladderButton.interactable = true;//Tarun

        }

    }

    public void THI_TrackGameData(string analysis)
    {
        DBmanager firemanDB = new DBmanager();
        firemanDB.question_id = STR_currentQuestionID;
        firemanDB.answer = STR_clickedAnswer;
        firemanDB.analysis = analysis;
        string toJson = JsonUtility.ToJson(firemanDB);
        STRL_gameData.Add(toJson);
        STR_Data = string.Join(",", STRL_gameData);
    }

    public IEnumerator IN_sendDataToDB()
    {
        if (calledonce == 0)
        {
            calledonce = 1;

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
                Debug.Log("Sending data to DB success : " + www.downloadHandler.text);
                MyJSON json = new MyJSON();
                json.THI_onGameComplete(www.downloadHandler.text);
            }
        }
    }




    [Header("Bird")]
    [SerializeField] private GameObject birdPF1;
    [SerializeField] private GameObject birdPF2;
    private GameObject currentBird;
    public Vector2 V_birdEnd;


    private IEnumerator OnStartBirdFly()
    {
        while (true)
        {
            yield return new WaitForSeconds(7f);
            THI_birdSpawn();
        }
    }

    private void THI_birdSpawn()
    {

        int randomBird = Random.Range(1, 3);
        int randomYpos = Random.Range(-3, 4);
        Vector2 V_birdStart = new Vector2(transform.position.x - 7.5f, transform.position.y + randomYpos);
        V_birdEnd = new Vector2(transform.position.x + 15f, transform.position.y + randomYpos);
        currentBird.transform.position = V_birdStart;
        if (randomBird == 1)
        {
            currentBird = Instantiate(birdPF1);
        }
        if (randomBird == 2)
        {
            currentBird = Instantiate(birdPF2);
        }
    }
}