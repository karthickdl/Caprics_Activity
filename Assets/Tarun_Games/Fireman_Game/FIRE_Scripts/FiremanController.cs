using DLearners;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.UI;

public class FiremanController : GameManagerBase
{
    [Header("DB")]
    public List<string> STRL_difficulty;
    public string STR_difficulty;
    public int I_totalQuestionCount;
    public List<int> IL_numberValues;
    public List<string> STRL_questionsDB;
    public List<string> STRL_questionID;
    public List<string> STRL_optionsDB;
    public List<string> STRL_answersDB;
    public List<string> STRL_optionsSpriteName;
    public List<Sprite> SPRL_optionsSprite;
    public List<string> STRL_instruction;
    public int I_correctPoints;
    public int I_wrongPoints;
    int calledonce;
    public List<string> STRL_coverImage;
    public Image IM_coverImage;
    public List<string> STRL_passageDetail;


    [Header("Questions")]
    public string STR_currentQuestionID;
    public int I_wrongAnsCount;
    public int I_points;
    public Text TEX_points;
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
    
    
    public GameObject G_coinPrefab;

    [Header("Health")]
    public Vector2 SpawnPos;

    

    [Header("Initialization")]
    public AnimationClip AC_introCam;
    public AnimationClip AC_controlsAnim;
    public GameObject G_dog;
    public bool B_dogRun;
    public float F_dogSpeed;

    [Header("Bird")]
    public GameObject G_bird1;
    public GameObject G_bird2;
    public GameObject G_currentBird;
    public Vector2 V_birdStart;
    public Vector2 V_birdEnd;
    public bool B_birdFly;

    [Header("Audios")]
    public AudioSource AS_baby;
    public AudioSource AS_siren;
    public AudioSource AS_correct;
    public AudioSource AS_wrong;
    public AudioSource AS_dogbark;
    public AudioSource AS_BGM;


    [Header("Instruction")]
    public GameObject G_instructionPage;
    public TextMeshProUGUI TEXM_instruction;

    [Header("AUDIO DB")]
    public List<string> STRL_questionAudios;
    public List<string> STRL_optionsAudios;
    public List<string> STRL_instructionAudios;

    [Header("AUDIO ASSIGN")]
    public AudioClip[] ACA__questionClips;
    public AudioClip[] ACA_optionClips;
    public AudioClip[] ACA_instructionClips;


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
    }

    /// <summary>
    /// We are initialising the game after all the tutorial thing is completed. 
    /// </summary>
    public override void InitGame()
    {
        base.InitGame();

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

    void Start()
    {

        firemancamera.GetComponent<Animator>().enabled = false;
        SpawnPos = transform.position;
        G_instructionPage.SetActive(false);
        I_currentQuestionCount = -1;
        int curqcount = I_currentQuestionCount + 1;
        //TEX_questionCount.text = "" + curqcount + "/" + I_totalQuestionCount;tarun
        TEX_points.text = "0";
        
        //G_controlButtons.SetActive(false);//Taru8n
        THI_addAudios();
       
        Invoke("THI_gameData", 1f);

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

        if (I_currentQuestionCount < STRL_questionsDB.Count - 1)
        {
            Debug.LogWarning("Assign THI_enableClickonButtons");
            I_fire = 0;
            I_fireCount = IA_FireCount[I_currentQuestionCount + 1];
        }

        G_bgm.SetActive(true);
        StartCoroutine(EN_birdfly());
        int curqcount = I_currentQuestionCount + 1;
        //TEX_questionCount.text = "" + curqcount + "/" + I_totalQuestionCount;Tarun
        HUDManager.Instance.UpdateQuestionCountText(currentQuestionID);//Tarun
    }

    void stopExtraAudios()
    {
        AS_dogbark.Stop();
        AS_siren.Stop();
    }

   

    IEnumerator EN_birdfly()
    {
        while (true)
        {
            yield return new WaitForSeconds(7f);
            THI_birdSpawn();
        }
    }

    void THI_birdSpawn()
    {
        int randomBird = Random.Range(1, 3);
        if (randomBird == 1)
        {
            G_currentBird = Instantiate(G_bird1);
        }
        if (randomBird == 2)
        {
            G_currentBird = Instantiate(G_bird2);
        }
        int randomYpos = Random.Range(-3, 4);
        V_birdStart = new Vector2(transform.position.x - 7.5f, transform.position.y + randomYpos);
        V_birdEnd = new Vector2(transform.position.x + 15f, transform.position.y + randomYpos);
        G_currentBird.transform.position = V_birdStart;
        B_birdFly = true;
    }    

    void THI_gameData()
    {
        // THI_getPreviewData();
       // if (MainController.instance.mode == "live")
        {
            StartCoroutine(EN_GetData()); // live game in portal
        }
      //  if (MainController.instance.mode == "preview")
        {
            // preview data in html game generator

            THI_getPreviewData();
        }
    }
    IEnumerator EN_GetData()
    {
        WWWForm form = new WWWForm();

       // form.AddField("game_id", MainController.instance.STR_GameID);

        UnityWebRequest www = UnityWebRequest.Post(DownloadManager.Instance.sendValueURL, form);
        yield return www.SendWebRequest();
        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log("GAME DATA: " + www.downloadHandler.text);
            MyJSON json = new MyJSON();
            json.Temp_type_2(www.downloadHandler.text, STRL_difficulty, IL_numberValues, STRL_questionsDB, STRL_answersDB, STRL_optionsDB, STRL_questionID, STRL_instruction, STRL_questionAudios, STRL_optionsAudios, STRL_instructionAudios, STRL_coverImage, STRL_passageDetail);
            STR_difficulty = STRL_difficulty[0];
            StartCoroutine(IN_downloadOptionImages());
            STRL_optionsSpriteName = STRL_optionsDB;
            I_totalQuestionCount = IL_numberValues[0];
            I_correctPoints = IL_numberValues[1];
            I_wrongPoints = IL_numberValues[2];
            StartCoroutine(IN_downloadCoverImage());
           // MainController.instance.I_TotalQuestions = I_totalQuestionCount;
          //  MainController.instance.I_correctPoints = I_correctPoints;
            StartCoroutine(EN_getAudioClips());
            StartCoroutine(EN_getAudioClips1());
            StartCoroutine(EN_getAudioClips2());
        }
    }
    public IEnumerator IN_downloadCoverImage()
    {
        if (STRL_coverImage[0] != "")
        {
            UnityWebRequest www = UnityWebRequestTexture.GetTexture(STRL_coverImage[0]);

            yield return www.SendWebRequest();


            if (www.isNetworkError || www.isHttpError)
            {
                //Debug.Log(www.error);
                Debug.Log(STRL_coverImage[0] + " => " + www.error);
            }
            else
            {
                Texture2D downloadedTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;
                List<Sprite> SPRL_coverImage = new List<Sprite>();
                SPRL_coverImage.Add(Sprite.Create(downloadedTexture, new Rect(0.0f, 0.0f, downloadedTexture.width, downloadedTexture.height), new Vector2(0.5f, 0.5f), 100.0f));
                IM_coverImage.sprite = SPRL_coverImage[0];
            }
        }

    }
    public IEnumerator EN_getAudioClips()
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

                  

                    Debug.Log("RAK LENGTH QUESTION CLIPS : " + ACA__questionClips.Length + "/" + I_totalQuestionCount);
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
        THI_assignAudioClips();
    }

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

    public void THI_assignAudioClips()
    {
   
        if (ACA_instructionClips.Length > 0)
        {
            TEXM_instruction.gameObject.AddComponent<AudioSource>();
            TEXM_instruction.gameObject.GetComponent<AudioSource>().playOnAwake = true;
            TEXM_instruction.gameObject.GetComponent<AudioSource>().clip = ACA_instructionClips[0];
            TEXM_instruction.gameObject.AddComponent<Button>();
            TEXM_instruction.gameObject.GetComponent<Button>().onClick.AddListener(THI_playAudio);
        }
    }

    void THI_playAudio()
    {
        EventSystem.current.currentSelectedGameObject.GetComponent<AudioSource>().Play();
    }
    public void THI_getPreviewData()
    {
        MyJSON json = new MyJSON();
       // json.Temp_type_2(MainController.instance.STR_previewJsonAPI, STRL_difficulty, IL_numberValues, STRL_questionsDB, STRL_answersDB, STRL_optionsDB, STRL_questionID, STRL_instruction, STRL_questionAudios, STRL_optionsAudios, STRL_instructionAudios, STRL_coverImage, STRL_passageDetail);
        STR_difficulty = STRL_difficulty[0];
        StartCoroutine(IN_downloadOptionImages());
        STRL_optionsSpriteName = STRL_optionsDB;
        I_totalQuestionCount = IL_numberValues[0];
        I_correctPoints = IL_numberValues[1];
        I_wrongPoints = IL_numberValues[2];
        StartCoroutine(IN_downloadCoverImage());
        //MainController.instance.I_TotalQuestions = I_totalQuestionCount;
        //MainController.instance.I_correctPoints = I_correctPoints;
        StartCoroutine(EN_getAudioClips());
        StartCoroutine(EN_getAudioClips1());
        StartCoroutine(EN_getAudioClips2());
    }

    public IEnumerator IN_downloadOptionImages()
    {
        for (int i = 0; i < STRL_optionsDB.Count; i++)
        {
            UnityWebRequest www = UnityWebRequestTexture.GetTexture(STRL_optionsDB[i]);
            yield return www.SendWebRequest();
            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(STRL_optionsDB[i] +" => "+www.error); 
            }
            else
            {
                Texture2D downloadedTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;
                Sprite SPR_optionSprite = Sprite.Create(downloadedTexture, new Rect(0.0f, 0.0f, downloadedTexture.width, downloadedTexture.height), new Vector2(0.5f, 0.5f), 100.0f);

                string[] Names = (STRL_optionsDB[i].Split('/'));
                string[] Finalname = (Names[Names.Length - 1].Split('.'));



                SPR_optionSprite.name = Finalname[0];

                SPRL_optionsSprite.Add(SPR_optionSprite);

 

                Debug.Log(STRL_optionsDB[i] + " => " + downloadedTexture.name);

                Debug.Log("RAK LENGTH OPTION IMAGES : " + SPRL_optionsSprite.Count + "/" + STRL_optionsDB.Count);
            }
        }
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

            if (I_currentQuestionCount < STRL_questionsDB.Count - 1)
            {
                I_fire = 0;
                I_fireCount = IA_FireCount[I_currentQuestionCount + 1];
            }

            I_points = I_points + I_correctPoints;
            TEX_points.text = I_points.ToString();
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
            if (I_points > I_wrongPoints)
            {
                I_points -= I_wrongPoints;
            }
            else
            {
                if (I_points > 0)
                {
                    I_points = 0;
                }
            }
            TEX_points.text = I_points.ToString();


            I_wrongAnsCount++;
            if (I_wrongAnsCount == 3)
            {
                if (STR_difficulty == "assistive")
                {
                    // show ans and go to next question
                    G_ansDisplay.SetActive(true);
                    for (int i = 0; i < GA_options.Length; i++)
                    {
                        if (GA_options[i].name.Contains(STRL_answersDB[I_currentQuestionCount]))
                        {
                            IM_answerDisplay.sprite = GA_options[i].GetComponent<Image>().sprite;
                        }

                        TEX_annswerDisplay.text = STRL_answersDB[I_currentQuestionCount];

                        Invoke("disableAnswer", 2.5f);

                    }
                    if (STR_difficulty == "intuitive")
                    {
                        // show ans and make the child click to go to next question
                        G_ansDisplay.SetActive(true);
                        for (int i = 0; i < GA_options.Length; i++)
                        {
                            if (GA_options[i].name.Contains(STRL_answersDB[I_currentQuestionCount]))
                            {
                                IM_answerDisplay.sprite = GA_options[i].GetComponent<Image>().sprite;
                            }
                        }
                        TEX_annswerDisplay.text = STRL_answersDB[I_currentQuestionCount];
                        Invoke("THI_disableAnswerDisplay", 2.5f);
                    }
                }
                if (I_wrongAnsCount == 2)
                {
                    if (STR_difficulty == "independent")
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
        PlayerController.Instance.G_extinguishButton.GetComponent<Button>().interactable = false;//Tarun
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

        if (I_currentQuestionCount < I_totalQuestionCount)
        {
            //game still running
            int curqcount = I_currentQuestionCount + 1;
           // TEX_questionCount.text = curqcount + "/" + I_totalQuestionCount;Tarun
            HUDManager.Instance.UpdateQuestionCountText(currentQuestionID);//Tarun
            STR_currentQuestionID = STRL_questionID[I_currentQuestionCount];
            STR_currentCorrectAnswer = STRL_answersDB[I_currentQuestionCount];
            STR_currentQuestion = STRL_questionsDB[I_currentQuestionCount];
            TEX_currentQuestion.text = STR_currentQuestion;
            if (TEX_currentQuestion.gameObject.GetComponent<AudioSource>() != null)
            {
                TEX_currentQuestion.gameObject.GetComponent<AudioSource>().clip = ACA__questionClips[I_currentQuestionCount];
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

                if (STR_1stOptionName != "" && STR_2ndOptionName != "" && STR_3rdOptionName == "")
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
                }
            }

            Debug.Log("RAK 1ST SPRITE INDEX : " + I_1stSprite);
            Debug.Log("RAK 2ND SPRITE INDEX : " + I_2ndSprite);
            Debug.Log("RAK 3RD SPRITE INDEX : " + I_3rdSprite);

            Debug.Log("RAK 1ST SPRITE NAME : " + SPRL_optionsSprite[I_1stSprite].name);
            Debug.Log("RAK 2ND SPRITE NAME : " + SPRL_optionsSprite[I_2ndSprite].name);
            Debug.Log("RAK 3RD SPRITE NAME : " + SPRL_optionsSprite[I_3rdSprite].name);

            GA_options[0].GetComponent<Image>().sprite = SPRL_optionsSprite[I_1stSprite];
            GA_options[1].GetComponent<Image>().sprite = SPRL_optionsSprite[I_2ndSprite];
            GA_options[2].GetComponent<Image>().sprite = SPRL_optionsSprite[I_3rdSprite];

            Debug.Log("RAK 1ST OPTION : " + GA_options[0].GetComponent<Image>().sprite.name);
            Debug.Log("RAK 2ND OPTION : " + GA_options[1].GetComponent<Image>().sprite.name);
            Debug.Log("RAK 3RD OPTION : " + GA_options[2].GetComponent<Image>().sprite.name);

            GA_options[0].name = STR_1stOptionName;
            GA_options[1].name = STR_2ndOptionName;
            GA_options[2].name = STR_3rdOptionName;


            GA_options[0].GetComponent<AudioSource>().clip = ACA_optionClips[I_1stSprite];
            GA_options[1].GetComponent<AudioSource>().clip = ACA_optionClips[I_2ndSprite];
            GA_options[2].GetComponent<AudioSource>().clip = ACA_optionClips[I_3rdSprite];

            Debug.Log("RAK 1ST OPTION CLIP : " + GA_options[0].GetComponent<AudioSource>().clip.name);
            Debug.Log("RAK 2ND OPTION CLIP : " + GA_options[1].GetComponent<AudioSource>().clip.name);
            Debug.Log("RAK 3RD OPTION CLIP : " + GA_options[2].GetComponent<AudioSource>().clip.name);


            I_lastOptionReqCount = I_currentOptionReqCount;


            PlayerController.Instance.G_ladderButton.GetComponent<Button>().interactable = true;//Tarun

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
}