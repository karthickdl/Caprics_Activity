﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;



public class PauseController : MonoBehaviour
{
    public GameObject G_pauseMenu;
    public GameObject G_resumeButton, G_dashboardButton;
    public float F_volume;
    public Slider SL_volume;



    private void Start()
    {
        SL_volume.value = F_volume;



        Time.timeScale = 1;
        G_dashboardButton.SetActive(true);
        G_resumeButton.SetActive(true);
    }




    public void BUT_pause()
    {
        G_pauseMenu.SetActive(true);
        Time.timeScale = 0;
    }
    public void BUT_resume()
    {
        G_pauseMenu.SetActive(false);
        Time.timeScale = 1;
    }
    public void BUT_dashboard()
    {
#if UNITY_ANDROID || UNITY_IOS
//Screen.orientation = ScreenOrientation.Portrait;
        //Destroy(VAKT_controller.instance.G_currentActivity);
        Application.Quit();
#elif UNITY_WEBGL
        Application.ExternalEval("closeApplication()");
#endif
    }
}