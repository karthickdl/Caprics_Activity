using DLearners;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class FruitNinja_Main : GameManagerBase
{
    public GameObject G_Answer;
    int I_Attempt;

    public float mindelay = 1.5f, maxdelay = 3f;
    public float wrgdelay = 6f;
    public GameObject[] GA_ansSpawnPoints;
    public GameObject[] GA_bombSpawnPoints;
    GameObject ansspwan,wrgspawn;
    public GameObject[] GA_ansprefabs;
    public GameObject G_Bombprefabs;

    public GameObject G_hint;

    public float F_Maxslices;
    public GameObject G_wrgeffect;
    public Image IMG_progress;
    public GameObject G_Blade;

   // public List<string> Lstr_ans, Lstr_wrng;
   // public List<AudioClip> AC_ans, AC_wrg;
    bool B_Correct;

    [Header("Values")]
    public int I_currentQuestionCount; // question number current
    public string STR_currentQuestionID;
    public int I_Points;
    public int I_wrongAnsCount;
    public int I_Counter, I_Dummmy;
    public string[] STRA_AnsList;
    public int I_Collect_count;


    [Header("Audios")]
    public AudioSource AS_Correct;
    public AudioSource AS_Wrong;
    public AudioSource AS_Jumping;
    public AudioClip[] AC_jump;


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
        // THI_Transition();        
    }
    /// <summary>
    /// We are initialising the game after all the tutorial thing is completed. 
    /// </summary>
    public override void InitGame()
    {
        base.InitGame();
        Invoke(nameof(NextStep), 1f);
    }
    /// <summary>
    /// Seting up level data from SO (per level) From base class
    /// </summary>
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
        G_Answer.SetActive(false);
        VaultPopUpsManager.Instance.ShowTransition(TransitionType.Fade);
        if (currentQuestionID < GameHandlerImmersiveGame.Instance.dataSO.datas.Count)
        {
            GetSetCurrentLevelData();
            Invoke(nameof(THI_NextQuestion), 2f);

        }
        else
        {
            OnLevelCompleted();
        }
        base.NextStep();//Need to be called after GetSetCurrentLevelData
    }




    IEnumerator Spawnans()
    {
        while (I_Collect_count < 15)
        {
           // Lstr_ans = Lstr_wrng = null;
           // AC_ans = AC_wrg = null;

            float delay = Random.Range(mindelay, maxdelay);
                yield return new WaitForSeconds(delay);

                int spawnpos = Random.Range(0, GA_ansSpawnPoints.Length);
                GameObject spawnpoint = GA_ansSpawnPoints[spawnpos];

                int ansindex = Random.Range(0, GA_ansprefabs.Length);
                ansspwan = Instantiate(GA_ansprefabs[ansindex]);
                ansspwan.transform.SetParent(spawnpoint.transform, false);
                ansspwan.transform.position = spawnpoint.transform.position;
                ansspwan.transform.rotation = spawnpoint.transform.rotation;

                int optnum = Random.Range(I_Counter, I_Dummmy);
                //ansspwan.transform.GetChild(0).GetComponent<Text>().text = STRL_options[optnum];
                ansspwan.transform.GetChild(0).GetComponent<Text>().text = currentData.options[optnum].option;
                //ansspwan.GetComponent<AudioSource>().clip = ACA_optionClips[optnum];

            int index = Random.Range(0, AC_jump.Length);
            AS_Jumping.clip = AC_jump[index];
            AS_Jumping.Play();
                Destroy(ansspwan, 5f);
            
        }
    }

    IEnumerator SpawnWrong()
    {
        while (I_Collect_count < 15)
        {

            float delay = Random.Range(5, 8);
            yield return new WaitForSeconds(delay);

            int spawnpos = Random.Range(0, GA_bombSpawnPoints.Length);
            GameObject spawnpoint = GA_bombSpawnPoints[spawnpos];

           // int wrgindex = Random.Range(0, G_Bombprefabs.Length);
            wrgspawn = Instantiate(G_Bombprefabs);
            wrgspawn.transform.SetParent(spawnpoint.transform, false);
            wrgspawn.transform.position = spawnpoint.transform.position;
            wrgspawn.transform.rotation = spawnpoint.transform.rotation;
            
            int index = Random.Range(0, AC_jump.Length);
            AS_Jumping.clip = AC_jump[index];
            AS_Jumping.Play();
            // Destroy(ansspwan, 5f);

        }
    }

    public void THI_Check()
    {
        B_Correct = false;
        for (int i=0;i<STRA_AnsList.Length;i++)
        {
            if(STRA_AnsList[i]==STR_currentSelectedAnswer)
            {
                B_Correct = true;
            }
        }

        if(B_Correct)
        {
            THI_Correct();
            //I_Collect_count++;
        }
        else { THI_Wrong(); }
    }

   

    public void THI_NextQuestion()
    {
       /* Blade_slicing.OBJ_blade_Slicing.formtrail = true;
        if (I_currentQuestionCount < STRL_questions.Count - 1)
        {
           
            I_currentQuestionCount++;
            if (I_currentQuestionCount != 0)
            {
                I_Counter = I_Counter + IL_numbers[3];
            }
            I_Collect_count = 0;
           
            STRA_AnsList = null;
            STR_currentQuestionID = STRL_questionID[I_currentQuestionCount];
            int currentquesCount = I_currentQuestionCount + 1;
            TEX_questionCount.text = currentquesCount + "/" + STRL_questions.Count;
            STR_currentQuestionAnswer = STRL_answers[I_currentQuestionCount];

            I_Dummmy = I_Counter + IL_numbers[3];

            I_wrongAnsCount = 0;

            STRA_AnsList = STRL_answers[I_currentQuestionCount].Split(',');

            for(int i=0;i<G_wrgeffect.transform.childCount;i++)
            {
                G_wrgeffect.transform.GetChild(i).gameObject.SetActive(false);
            }

            G_Answer.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "The answers are : " + STRL_answers[I_currentQuestionCount];


            IMG_progress.fillAmount = 0 / F_Maxslices;

        }
        else
        {
            G_Blade.SetActive(false);
            OnLevelCompleted();
        }*/
    }



    public override void OnLevelCompleted()
    {
        base.OnLevelCompleted();
        StartCoroutine(IN_sendDataToDB());
    }


    public void THI_Correct()
    {
        I_Collect_count++;
        HUDManager.Instance.UpdateScoreText(true);

        // float F_score = (float)I_Collect_count;



        // Debug.Log(F_calculation);
        IMG_progress.fillAmount = (float)I_Collect_count / F_Maxslices;


        // Release bird animation
        THI_TrackGameData("1");
        if(I_Collect_count==15) //no of items to be collected
        {
            StopAllCoroutines();
            Blade_slicing.OBJ_blade_Slicing.formtrail = false;
            Invoke(nameof(NextStep), 3f);
        }

    }

    IEnumerator Highlight()
    {
        for (int i = 0; i < 5; i++)
        {
            yield return new WaitForSeconds(1);
        }
    }

    void THI_WrongEffect()
    {
        if (currentDifficultyLevelType == DifficultyLevelType.Easy || currentDifficultyLevelType == DifficultyLevelType.Medium)
        {
            G_Answer.SetActive(true);
            Invoke(nameof(NextStep), 5f);

            //Show answer and move to next question
        }
        else if (currentDifficultyLevelType == DifficultyLevelType.Hard)
        {
            I_Attempt++;
            I_wrongAnsCount = I_wrongAnsCount - 3;
            G_Answer.SetActive(true);

            for(int i=0;i< G_wrgeffect.transform.childCount; i++)
            {
                G_wrgeffect.transform.GetChild(i).gameObject.SetActive(false);
            }
            for (int i = 0; i < 2; i++)
            {
                G_wrgeffect.transform.GetChild(i).gameObject.SetActive(true);
            }
            if(I_Attempt==2)
            {
                Invoke(nameof(NextStep), 5f);
               
            }
            else
            {
                Invoke(nameof(THI_Continue), 3f);
            }
            
            //Show answer and after click next question
        }
    }

    void THI_Continue()
    {
        G_Answer.SetActive(false);

        Blade_slicing.OBJ_blade_Slicing.formtrail = true;
        StartCoroutine(Spawnans());
        StartCoroutine(SpawnWrong());
    }

    public void THI_Wrong()
    {
        Debug.Log("Wrong ans");
      
        AS_Wrong.Play();
        HUDManager.Instance.UpdateScoreText(false);
        THI_TrackGameData("0");
        I_wrongAnsCount++;

        G_wrgeffect.transform.GetChild(I_wrongAnsCount - 1).gameObject.SetActive(true);

        if (I_wrongAnsCount==5)
        {
            StopAllCoroutines();
            THI_WrongEffect();
            Debug.Log("Restart or use coins");
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