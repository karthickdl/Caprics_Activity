using DLearners;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.UI;


public class RB_Runner_Main : GameManagerBase
{
    

    [Header("Screens and UI elements")]

    
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

    }

    public override void OnPlayButton()
    {
        base.OnPlayButton();
        Robotmovement.Instance.OnPlayButton();
    }
    public override void InitGame()
    {
        base.InitGame();
        Tarun();

        G_Transition.SetActive(false);
        G_levelComplete.SetActive(false);

        ff();
        THI_Transition();
        I_currentQuestionCount = -1;


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
    private void Tarun()
    {
        currentData = new Data();
        currentInstructionData = new InstructionData();
        currentDifficultyLevelType = TarunTesting.Instance.dataSO.difficultyLevelType;
        DataSO cashDataSO = TarunTesting.Instance.dataSO;

        currentData = cashDataSO.GetData(2);//ID HardCode
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
    void THI_Transition()
    {
        G_Question.SetActive(false);
        G_Transition.SetActive(true);
        THI_NewQuestion();
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
        DLearnersAudioManager.Instance.PlaySound2("AS_Wrong");

        THI_TrackGameData("0");
        I_wrongAnsCount++;
        THI_WrongEffect();
        HUDManager.Instance.UpdateScoreText(false);
    }

    void THI_playAudio()
    {
        EventSystem.current.currentSelectedGameObject.GetComponent<AudioSource>().Play();
        Debug.Log("player clicked. so playing audio");
    }
    private void ff()
    {
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
