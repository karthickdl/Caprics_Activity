using DLearners;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.UI;

public class DG_Game : GameManagerBase
{

    [Header("Objects")]
    public GameObject G_Penguin;
    GameObject G_Selected;
    public GameObject G_Question;
    public GameObject[] G_Options;
    public GameObject[] G_Option_Text;
    public TextMeshProUGUI TEXM_instruction2;
    GameObject G_Answer;

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


        currentQuestionID = 0;
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
        for (int i = 0; i < currentOptionCount; i++)
        {
            G_Options[i].name = currentData.options[i].option;
            G_Options[i].transform.GetComponent<Button>().onClick.RemoveAllListeners();
            G_Options[i].transform.GetComponent<Button>().onClick.AddListener(()=>BUT_OptionClicking(i));
            G_Option_Text[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = currentData.options[i].option;
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
        //currentWrongAnsCount = 0;
        //I_wrongAnsCount = 0;
        STR_currentSelectedAnswer = "";
        // I_currentQuestionCount++;
        //STR_currentQuestionID = STRL_questionID[I_currentQuestionCount];

        G_Question.GetComponent<AudioSource>().clip = currentData.questionData.questionAudioClip;

        for (int i = 0; i < G_Options.Length; i++)
        {
            G_Options[i].transform.GetChild(0).GetComponent<Image>().color = Color.white;
        }
        THI_Initial_Anim();
    }


    /// <summary>
    /// For Checking the answer if it is right or wrong. ()
    /// </summary>
    public override void CheckAnswer()
    {
        base.CheckAnswer();
        if (STR_currentSelectedAnswer == STR_currentQuestionAnswer)
        {
            G_Selected.transform.GetChild(0).GetComponent<Image>().color = Color.green;
            // Debug.Log(G_Selected.name);
            G_Penguin.GetComponent<Animator>().SetBool("Bool", true);
            CorrectAnswerSequence();
        }
        else
        {
            Invoke(nameof(WrongAnswerSequence), 3f);
            G_Penguin.GetComponent<Animator>().SetBool("Bool", false);

            G_Selected.GetComponent<Animator>().Play("underwater");
            //G_penguin fall anim
        }
    }

    /// <summary>
    /// On correct answer sequence
    /// </summary>
    public override void CorrectAnswerSequence()
    {
        base.CorrectAnswerSequence();
        isInputUnLocked = false;
        I_Collect_count++;
        HUDManager.Instance.UpdateScoreText(true);
        DLearnersAudioManager.Instance.PlayCommonSound("Com_Correct");

        THI_TrackGameData("1");

        Invoke(nameof(NextStep), 1f);
    }

    /// <summary>
    /// On Wrong answer sequence
    /// </summary>
    public override void WrongAnswerSequence()
    {
        base.WrongAnswerSequence();
        DLearnersAudioManager.Instance.PlayCommonSound("Com_Wrong");

        HUDManager.Instance.UpdateScoreText(false);
        THI_TrackGameData("0");
        currentWrongAnsCount++;
        isInputUnLocked = false;
        if (currentWrongAnsCount == wrongAnsLifeCounts[0])//3
        {
            for (int i = 0; i < G_Options.Length; i++)
            {
                if (G_Options[i].activeInHierarchy)
                {
                    if (G_Options[i].name == STR_currentQuestionAnswer)
                    {
                        G_Answer = G_Options[i];
                    }
                }
            }
            if (currentDifficultyLevelType == DifficultyLevelType.Easy)
            {

                StartCoroutine(GreenHiglight());


                //Show answer and move to next question
            }
            else if (currentDifficultyLevelType == DifficultyLevelType.Medium)
            {
                StartCoroutine(GreenHiglight());

                //Show answer and after click next question
            }

        }
        else
        if (currentWrongAnsCount == wrongAnsLifeCounts[1])//2
        {
                VaultPopUpsManager.Instance.ShowTransition(TransitionType.Fade);
            if (currentDifficultyLevelType == DifficultyLevelType.Hard)
            {
                Invoke(nameof(ShowCurrentQuestion), 2f);
            }
            else
            {
                Invoke(nameof(THI_Initial_Anim), 2f);
            }

            //next question
        }
        else
        {
            VaultPopUpsManager.Instance.ShowTransition(TransitionType.Fade);
            Invoke(nameof(THI_Initial_Anim), 2f);
        }

        HUDManager.Instance.UpdateScoreText(false);
    }




    private void THI_Initial_Anim()
    {
        for (int i = 0; i < G_Options.Length; i++)
        {
            if (G_Options[i].activeInHierarchy)
                G_Options[i].GetComponent<Animator>().Play("Idle");

        }

        Animator tempAnimator = G_Penguin.GetComponent<Animator>();
        tempAnimator.Play("Penguin_Intro");
        tempAnimator.SetInteger("Cond", 0);
        tempAnimator.SetBool("Bool", true);
        G_Question.GetComponent<AudioSource>().Play();
        isInputUnLocked = true;
    }



    public void BUT_OptionClicking(int I_Penguin)
    {
        DLearnersAudioManager.Instance.PlayCommonSound("Com_Clicking");

         if (isInputUnLocked)
         {
            isInputUnLocked = false;
              G_Selected = EventSystem.current.currentSelectedGameObject;
            G_Penguin.GetComponent<Animator>().SetInteger("Cond", I_Penguin);
            STR_currentSelectedAnswer = G_Selected.name;
            if(STR_currentSelectedAnswer=="stay")
            {
                CheckAnswer();
            }
            else
            {
                Invoke(nameof(CheckAnswer), 4f);
            }            
         }
    }






    public override void OnLevelCompleted()
    {
        base.OnLevelCompleted();
        StartCoroutine(IN_sendDataToDB());
    }

  

   
   
    IEnumerator GreenHiglight()
    {
        for (int i = 0; i < 4; i++)
        {
            G_Answer.transform.GetChild(0).GetComponent<Image>().color = Color.green;
              yield return new WaitForSeconds(0.5f);
            G_Answer.transform.GetChild(0).GetComponent<Image>().color = Color.white;
            yield return new WaitForSeconds(0.5f);
        }
        if(currentDifficultyLevelType == DifficultyLevelType.Easy)
        { 
            Invoke(nameof(ShowCurrentQuestion), 2f);
        }
        else if(currentDifficultyLevelType == DifficultyLevelType.Medium) 
        { 
            Invoke(nameof(THI_Initial_Anim), 2f);
        }
       
    }

    
   
   
    public void BUT_Speaker()
    {
       // Debug.Log("Playing Audio");
        G_Question.transform.GetChild(1).GetComponent<AudioSource>().Play();
        isInputUnLocked = true;
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
