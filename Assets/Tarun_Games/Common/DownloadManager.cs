using UnityEngine;

namespace DLearners
{
    public class DownloadManager : Singleton<DownloadManager>
    {
        [SerializeField] private string gameID;
        const string GAMEID = "game_id";

        [Header("URL")]
        [SerializeField] protected string getValueURL;
        [SerializeField] public string sendValueURL;

        /* private void Start()
         {
             getValueURL = "http://103.117.180.121:8000/test/Game_template_api-s/game_template_1.php";
             sendValueURL = "http://103.117.180.121:8000/test/Game_template_api-s/save_child_questions.php";
         }*/

        protected override void Awake()
        {
            base.Awake();
        }
        public void SetURLData(URLData uRLData)
        {
            getValueURL = uRLData.getValueURL;
            sendValueURL = uRLData.getValueURL;
        }
    }
}