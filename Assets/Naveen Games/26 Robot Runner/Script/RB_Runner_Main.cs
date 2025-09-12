using DLearners;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class RB_Runner_Main : GameManagerBase
{
    public bool B_production;

    [Header("Screens and UI elements")]
    //public GameObject G_Demo;
    bool B_CloseDemo;

    public GameObject G_Game;
    public GameObject G_Transition;
    

    [Header("Objects")]
    public GameObject[] GA_Question;
    public GameObject G_QuestionSpawn;
    public GameObject G_currentquestion;
    public GameObject G_Robot;
    public GameObject G_Question;
    public Image questionIMG;//Tarun
    public GameObject G_Options;
    public GameObject[] GA_Options;
    GameObject G_Highlight;


    [Header("Values")]
    public string STR_currentSelectedAnswer;
    public int I_currentQuestionCount; // question number current
    public string STR_currentQuestionID;
    public int I_wrongAnsCount;
    //public string[] STRA_AnsList;
    public int I_Collect_count;


    [Header("URL")]
    public string URL;
    public string SendValueURL;

    //Dummy values only for helicopter game

    [Header("GAME DATA")]
    public List<string> STRL_gameData;
    public string STR_Data;

    [Header("LEVEL COMPLETE")]
    public GameObject G_levelComplete;

    [SerializeField] private Sprite[] SPRA_ArrowsWebGL;
    [SerializeField] private Sprite[] SPRA_ArrowsMobile;
    [SerializeField] private Image[] IMGA_Up;
    [SerializeField] private Image[] IMGA_Down;



    protected override void Awake()
    {
        base.Awake();

        if (B_production)
        {
            URL = "https://dlearners.in/template_and_games/Game_template_api-s/game_template_1.php"; // PRODUCTION FETCH DATA
            SendValueURL = "https://dlearners.in/template_and_games/Game_template_api-s/save_child_questions.php"; // PRODUCTION SEND DATA

        }
        else
        {
            /*  URL = "http://20.120.84.12/Test/template_and_games/Game_template_api-s/game_template_1.php"; // UAT FETCH DATA
               SendValueURL = "http://20.120.84.12/Test/template_and_games/Game_template_api-s/save_child_questions.php"; // UAT SEND DATA*/

            URL = "http://103.117.180.121:8000/test/Game_template_api-s/game_template_1.php"; // UAT FETCH DATA
            SendValueURL = "http://103.117.180.121:8000/test/Game_template_api-s/save_child_questions.php"; // UAT SEND DATA
        }

    }
    void Start()
    {
        Tarun();
        B_CloseDemo = true;
        THI_Transition();

        G_Game.SetActive(false);
        G_Transition.SetActive(false);
        G_levelComplete.SetActive(false);

       // STRL_answers = new List<string>();
        Invoke("THI_gameData", 1f);

        I_currentQuestionCount = -1;


        #region----------Platform Checking to set sprites for controls in Demo

        if (MainController.instance.WEB)
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
        }

        #endregion


    }


    public void Tarun()
    {
        currentData = new Data();
        currentInstructionData = new InstructionData();
        currentDifficultyLevelType = TarunTesting.Instance.dataSO.difficultyLevelType;
        DataSO cashDataSO = TarunTesting.Instance.dataSO;

        currentData = cashDataSO.GetData(0);//ID HardCode
        currentOptionCount = currentData.options.Count;
        currentInstructionData = cashDataSO.instructionData;

        STR_currentQuestionAnswer = currentData.correctOptions;

        // questionIMG.sprite = TarunTesting.Instance.dataSO.GetQuestionSprit(0);
    }


    public void THI_Check()
    {
        if (isInputUnLocked)
        {
            GameObject G_Selected = EventSystem.current.currentSelectedGameObject;
            STR_currentSelectedAnswer = EventSystem.current.currentSelectedGameObject.GetComponent<TextMeshProUGUI>().text;


            if (STR_currentSelectedAnswer == STR_currentQuestionAnswer)
            {
                G_Selected.GetComponent<AudioSource>().Play();
                isInputUnLocked = false;
                THI_Correct();
                //I_Collect_count++;
            }
            else { THI_Wrong(); }
        }

    }

    public void CheckForAnswer()//Tarun
    {
        
    }


    void THI_gameData()
    {
        // THI_getPreviewData();
        if (MainController.instance.mode == "live")
        {
            StartCoroutine(EN_getValues()); // live game in portal
        }
        if (MainController.instance.mode == "preview")
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
        G_Question.SetActive(false);
        G_Transition.SetActive(true);
        Invoke(nameof(THI_NewQuestion), 2f);
    }

    public override void UpdateQuestion()
    {
        isInputUnLocked = true;
        G_Question.SetActive(true);//

        AudioClip ggd = currentInstructionData.instructionAudioClip[0];//Hardcode
         DLearners.DLearnersAudioManager.Instance.PlaySound3(ggd);
        //TEXM_instruction2.gameObject.GetComponent<AudioSource>().Play();
        //Invoke(nameof(PlayQuestionAudio), TEXM_instruction2.gameObject.GetComponent<AudioSource>().clip.length);

        DLearners.DLearnersAudioManager.Instance.PlaySound3(currentData.questionData.questionAudioClip, ggd.length);
    }

    void PlayQuestionAudio()
    {
        G_Question.transform.GetChild(0).transform.GetChild(0).GetComponent<AudioSource>().Play();
    }
    public void THI_NewQuestion()
    {
        G_Robot.SetActive(true);
        Robotmovement.Instance.RobotInIt();
        if (G_currentquestion != null)
        {
            Destroy(G_currentquestion);
        }

        THI_NextQuestion();
    }
    
    public void THI_NextQuestion()
    {

        G_Transition.SetActive(false);
        if (I_currentQuestionCount <  TarunTesting.Instance.dataSO.datas.Count-1)
        {
            int Index = Random.Range(0, GA_Question.Length);
            G_currentquestion = Instantiate(GA_Question[Index]);
            G_currentquestion.transform.SetParent(G_QuestionSpawn.transform, false);

            I_currentQuestionCount++;


            //STRA_AnsList = null;
            //STR_currentQuestionID = STRL_questionID[I_currentQuestionCount];
            int currentquesCount = I_currentQuestionCount + 1;


            HUDManager.Instance.UpdateQuestionCountText(I_currentQuestionCount);//Tarun

           // TEX_questionCount.text = currentquesCount + "/" + STRL_questions.Count;Tarun
           // STR_currentQuestionAnswer = STRL_answers[I_currentQuestionCount];
            /*G_Question.transform.GetChild(0).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = STRL_questions[I_currentQuestionCount];
            G_Question.transform.GetChild(0).transform.GetChild(0).GetComponent<AudioSource>().clip = ACA__questionClips[I_currentQuestionCount];*/

            G_Question.transform.GetChild(0).transform.GetChild(0).GetComponent<Image>().sprite = currentData.questionData.questionSprit;
            G_Question.transform.GetChild(0).transform.GetChild(0).GetComponent<Image>().preserveAspect = true;
            G_Question.transform.GetChild(0).transform.GetChild(0).GetComponent<AudioSource>().clip = currentData.questionData.questionAudioClip;






            for (int i = 0; i < G_Options.transform.childCount; i++)
            {
                G_Options.transform.GetChild(i).name = currentData.options[i].option;
                G_Options.transform.GetChild(i).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = currentData.options[i].option;
                G_Options.transform.GetChild(i).transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.white;
                G_Options.transform.GetChild(i).transform.GetChild(0).GetComponent<AudioSource>().clip = currentData.options[i].optionAudioClip;
            }

            I_wrongAnsCount = 0;
        }
        else
        {
            THI_Levelcompleted();
            // Invoke(nameof(THI_Levelcompleted), 3f);
        }
    }



    void THI_Levelcompleted()
    {
        MainController.instance.I_TotalPoints = TarunTesting.Instance.dataSO.GetCorrectAnswerPoint();
        G_levelComplete.SetActive(true);
        StartCoroutine(IN_sendDataToDB());
    }

    public void THI_Correct()
    {
        DLearnersAudioManager.Instance.PlaySound2("AS_Correct");
        I_Collect_count++;
        HUDManager.Instance.UpdateScoreText(true);

        // Release bird animation
        THI_TrackGameData("1");
        Invoke(nameof(THI_Transition), 3f);


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

    public override void THI_WrongEffect()
    {
        if (currentWrongAnsCount == wrongAnsLifeCounts[0])//3
        {
            if (currentDifficultyLevelType == DifficultyLevelType.Easy)
            {
                isInputUnLocked = false;
                for (int i = 0; i < G_Options.transform.childCount; i++)
                {
                    if (G_Options.transform.GetChild(i).name == STR_currentQuestionAnswer)
                    {
                        G_Options.transform.GetChild(i).transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.green;
                    }
                }


                Invoke(nameof(THI_Transition), 5f);
            }
            else if (currentDifficultyLevelType == DifficultyLevelType.Medium)
            {
                isInputUnLocked = true;
                for (int i = 0; i < G_Options.transform.childCount; i++)
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
                Invoke(nameof(THI_Transition), 2f);
            }
        }
    }

    public void THI_Wrong()
    {
        // Debug.Log("Wrong ans");

        DLearnersAudioManager.Instance.PlaySound2("AS_Wrong");
        THI_TrackGameData("0");
        I_wrongAnsCount++;


        /*  if (I_wrongAnsCount == 5)
          {
              Debug.Log("Restart or use coins");
          }*/
        //REDO the same question

        // wrong bird animation
        THI_WrongEffect();

        HUDManager.Instance.UpdateScoreText(false);
    }
    public IEnumerator IN_CoverImage()
    {
        //UnityWebRequest www = UnityWebRequestTexture.GetTexture(STRL_cover_img_link[0]);
        UnityWebRequest www = UnityWebRequestTexture.GetTexture("https://dlearners.in/template_and_games/Game_Generator/generatedGame/know_my_nameLIEZ/Q_a/ques/rock.png");
        yield return www.SendWebRequest();
        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            // Texture2D downloadedTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;
            Texture2D downloadedTexture = DownloadHandlerTexture.GetContent(www);
          //  if (STRL_cover_img_link != null)
            {
                var jj = Sprite.Create(downloadedTexture, new Rect(0.0f, 0.0f, downloadedTexture.width, downloadedTexture.height), new Vector2(0.5f, 0.5f), 100.0f);

               // TarunTesting.Instance.dataSO.SetData(downloadedTexture);
               // TarunTesting.Instance.dataSO.SetDatasp(jj);

                Data gg = new Data();
               // gg.questionSP = jj;
                //TarunTesting.Instance.dataSO.datas.Add(gg);
              //  TarunTesting.Instance.coverPage.bgIMG.sprite = TarunTesting.Instance.dataSO.datas[0].questionSP;
                /* G_coverPage.GetComponent<Image>().sprite = Sprite.Create(downloadedTexture, new Rect(0.0f, 0.0f, downloadedTexture.width, downloadedTexture.height), new Vector2(0.5f, 0.5f), 100.0f);*/

            }
        }


        //SPRA_Options

    }

    public IEnumerator EN_getValues()
    {
        WWWForm form = new WWWForm();
        form.AddField("game_id", MainController.instance.STR_GameID);
        // Debug.Log("GAME ID : " + MainController.instance.STR_GameID);
        UnityWebRequest www = UnityWebRequest.Post(URL, form);
        yield return www.SendWebRequest();
        if (www.isHttpError || www.isNetworkError)
        {
            Debug.Log(www.error);
        }
        else
        {

            MyJSON json = new MyJSON();
            List<string> STRL_Passagedetails = new List<string>();
            //json.Helitemp(www.downloadHandler.text);
            json.Temp_type_2(www.downloadHandler.text, null, null, null, null, null, null, null, null, null,
            null, null, STRL_Passagedetails);
            //        Debug.Log("GAME DATA : " + www.downloadHandler.text);

           // STR_difficulty = STRL_difficulty[0];

          //  STR_instruction = STRL_instruction[0];
            ////MainController.instance.I_correctPoints = I_correctPoints = IL_numbers[1];//Tarun//Tarun
            //I_wrongPoints = IL_numbers[2];//Tarun
            MainController.instance.I_TotalQuestions = TarunTesting.Instance.dataSO.datas.Count;

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

            //StartCoroutine(EN_getAudioClips());
            StartCoroutine(IN_CoverImage());
            //StartCoroutine(IMG_Options());

        }
    }
    void THI_playAudio()
    {
        EventSystem.current.currentSelectedGameObject.GetComponent<AudioSource>().Play();
        Debug.Log("player clicked. so playing audio");
    }
    public void THI_getPreviewData()
    {
        MyJSON json = new MyJSON();
        List<string> STRL_Passagedetails = new List<string>();
        //  json.Helitemp(MainController.instance.STR_previewJsonAPI);
        json.Temp_type_2(MainController.instance.STR_previewJsonAPI, null, null, null, null, null, null, null, null, null,
            null, null, STRL_Passagedetails);

        //STR_difficulty = STRL_difficulty[0];
       // STR_instruction = STRL_instruction[0];
        //MainController.instance.I_correctPoints = I_correctPoints = IL_numbers[1];//Tarun
        //I_wrongPoints = IL_numbers[2];Tarun
        MainController.instance.I_TotalQuestions = TarunTesting.Instance.dataSO.datas.Count;

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
        //StartCoroutine(EN_getAudioClips());
        StartCoroutine(IN_CoverImage());
       // StartCoroutine(IMG_Options());

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
        form.AddField("child_id", MainController.instance.STR_childID);
        form.AddField("game_id", MainController.instance.STR_GameID);
        form.AddField("game_details", "[" + STR_Data + "]");


        Debug.Log("child id : " + MainController.instance.STR_childID);
        Debug.Log("game_id  : " + MainController.instance.STR_GameID);
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
