using DLearners;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class SlingShot_Main : GameManagerBase
{
    [Header("Objects")]
    public GameObject G_Question;
    public GameObject G_Options;
    public GameObject[] GA_Options;
    public GameObject G_Sling;
    public GameObject G_Sling_pos;
    GameObject G_Slingclone;
    int I_Dummy;
    public Vector3[] V3_OptPos;


    [Header("URL")]
    public string URL;
    public string SendValueURL;
   

    [Header("GAME DATA")]
    public List<string> STRL_gameData;
    public string STR_Data;

    [Header("AUDIO ASSIGN")]
    public AudioClip[] ACA__questionClips;
    public AudioClip[] ACA_optionClips;
    public AudioClip[] ACA_instructionClips;

    #region Unity
    protected override void Awake()
    {
        base.Awake();
    }
    /*   private void Awake()
       {

           if (B_production)
           {
               URL = "https://dlearners.in/template_and_games/Game_template_api-s/game_template_2.php"; // PRODUCTION FETCH DATA
               SendValueURL = "https://dlearners.in/template_and_games/Game_template_api-s/save_child_questions.php"; // PRODUCTION SEND DATA
           }
           else
           {
               URL = "http://20.120.84.12/Test/template_and_games/Game_template_api-s/game_template_2.php"; // UAT FETCH DATA
               SendValueURL = "http://20.120.84.12/Test/template_and_games/Game_template_api-s/save_child_questions.php"; // UAT SEND DATA

           }
       }*/
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

        currentQuestionID = 0;

        SetUpOptionsPanel();        
        Next();
    }

    /// <summary>
    /// Seting up level data from SO (per level) From base class
    /// </summary>
    /// </summary>
    protected override void GetSetCurrentLevelData()
    {
        base.GetSetCurrentLevelData();
    }

    /// <summary>
    /// Showing transition and moving to next question, or checking for level complete 
    /// </summary>
    private void Next()
    {
        VaultPopUpsManager.Instance.ShowTransition(TransitionType.Fade);
        if (currentQuestionID < GameHandlerImmersiveGame.Instance.dataSO.datas.Count)
        {
            GetSetCurrentLevelData();
            G_Question.SetActive(false);
            THI_NextQuestion();
        }
        else
        {
            OnLevelCompleted();
        }
        currentQuestionID++;
    }


   

    /// <summary>
    /// For Checking the answer if it is right or wrong. ()
    /// </summary>
    public override void CheckAnswer(Transform Selected)
    {
        if (isInputUnLocked)
        {
            STR_currentSelectedAnswer = Selected.name;

            if (STR_currentSelectedAnswer == STR_currentQuestionAnswer)
            {
                isInputUnLocked = false;
                Selected.GetComponent<Rigidbody2D>().gravityScale = 1f;
                SetAnswerOnBord();
                THI_Correct();
            }
            else
            {
                WrongAnswerSequence();
            }
        }
    }
    private void SetAnswerOnBord()
    {
        string STR_dummy = currentData.questionData.question;        
        STR_dummy = currentData.correctOptions + STR_dummy.Substring(1, STR_dummy.Length-1);
        G_Question.transform.GetChild(0).transform.GetChild(1).transform.GetChild(0).GetComponent<TextMeshProUGUI>().SetText(STR_dummy);
    }






    public void THI_ShowQuestion()
    {
        isInputUnLocked = true;
        G_Question.SetActive(true);
        G_Question.transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).GetComponent<AudioSource>().Play();
    }

    public void THI_CloneSling()
    {
        if (G_Slingclone != null)
        {
            Destroy(G_Slingclone);
        }
        G_Slingclone = Instantiate(G_Sling, G_Sling_pos.transform);
        G_Slingclone.transform.position = G_Sling_pos.transform.position;
    }

    public void THI_NextQuestion()
    {

        THI_CloneSling();

        G_Question.transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).GetComponent<Image>().sprite = currentData.questionData.questionSprit;
        G_Question.transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).GetComponent<AudioSource>().clip = currentData.questionData.questionAudioClip;


        G_Question.transform.GetChild(0).transform.GetChild(1).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = currentData.questionData.question;

        for (int i = 0; i < G_Options.transform.childCount; i++)
        {
            G_Options.transform.GetChild(i).name = currentData.options[i].option;
            G_Options.transform.GetChild(i).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = currentData.options[i].option;
            G_Options.transform.GetChild(i).transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.white;
            G_Options.transform.GetChild(i).transform.GetChild(0).GetComponent<AudioSource>().clip = currentData.options[i].optionAudioClip;
            G_Options.transform.GetChild(i).GetComponent<Rigidbody2D>().gravityScale = 0f;
            G_Options.transform.GetChild(i).transform.position = V3_OptPos[i];
        }

        THI_ShowQuestion();
        currentWrongAnsCount = 0;
        HUDManager.Instance.UpdateQuestionCountText(currentQuestionID);//Tarun
    }



    public void THI_Correct()
    {
        DLearnersAudioManager.Instance.PlayCommonSound("Com_Correct");

        I_Collect_count++;
        // TEX_points.text = I_Points.ToString();
        HUDManager.Instance.UpdateScoreText(true);

        // Release bird animation
        THI_TrackGameData("1");
        Invoke(nameof(Next), 3f);


    }
    IEnumerator Highlight()
    {
        
        for (int i = 0; i < G_Options.transform.childCount; i++)
        {
            if (G_Options.transform.GetChild(i).name == STR_currentQuestionAnswer)
            {
                I_Dummy = i;
                G_Options.transform.GetChild(i).transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.green;
            }
        }


        G_Options.transform.GetChild(I_Dummy).GetComponent<Image>().color = Color.green;
         yield return new WaitForSeconds(0.5f);
            G_Options.transform.GetChild(I_Dummy).GetComponent<Image>().color = Color.white;
            yield return new WaitForSeconds(0.5f);
        G_Options.transform.GetChild(I_Dummy).GetComponent<Image>().color = Color.green;
        yield return new WaitForSeconds(0.5f);
        G_Options.transform.GetChild(I_Dummy).GetComponent<Image>().color = Color.white;
        yield return new WaitForSeconds(0.5f);

        
    }

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
                StartCoroutine(Highlight());
                Invoke(nameof(Next), 10f);

                //Show answer and move to next question
            }
            else if (currentDifficultyLevelType == DifficultyLevelType.Medium)
            {
                StartCoroutine(Highlight());

               // Invoke(nameof(THI_Transition), 3f);

                //Show answer and after click next question
            }

        }
        else if (currentWrongAnsCount == wrongAnsLifeCounts[1])//2
        {
            if (currentDifficultyLevelType == DifficultyLevelType.Hard)
            {
                Invoke(nameof(Next), 2f);
            }
               

            //next question
        }

        THI_CloneSling();
        HUDManager.Instance.UpdateScoreText(false);
    }

  

    void SetUpOptionsPanel()
    {
        int cacheLoop = GA_Options.Length;
        int cash = GameHandlerImmersiveGame.Instance.dataSO.datas[currentQuestionID].options.Count;
        for (int i = 0; i < cacheLoop; i++)
        {
            GA_Options[i].SetActive(false);
        }
        if (cash == 2)
        {
            G_Options = GA_Options[0];
        }
        if (cash == 4)
        {
            G_Options = GA_Options[1];
        }

        V3_OptPos = new Vector3[cash];

        if (G_Options != null)
        {
            G_Options.SetActive(true);
            for (int i = 0; i < cash; i++)
            {
                V3_OptPos[i] = G_Options.transform.GetChild(i).transform.position;
            }
        }
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


    public void BUT_closeInstruction()
    {
        G_Slingclone.SetActive(true);
        Time.timeScale = 1;
       
    }
}
