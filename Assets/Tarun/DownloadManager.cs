using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DownloadManager : MonoBehaviour
{
    [SerializeField] private string gameID;
    const string GAMEID = "game_id";

    public string getDataURL;
    public string sentDataURL;

    private void Start()
    {
        getDataURL = "http://103.117.180.121:8000/test/Game_template_api-s/game_template_1.php";
        sentDataURL = "http://103.117.180.121:8000/test/Game_template_api-s/save_child_questions.php";
    }
}
