using SimpleJSON;
using System;
using UnityEngine;

public class JS_GameID : MonoBehaviour
{
    public static JS_GameID instance;
    public GameObject G_gameManager;


    public void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        Application.ExternalEval("OnAppReady()");
    }

    public void JS_getID(string val)
    {
        MainController.instance.STR_IDjson = val;
        Debug.Log("json string from javascript : " + val);
        MyJSON myjson = new MyJSON();
        myjson.FetchIDs();
        MainController.instance.mode = "live";
    }

    public void JS_getMode(string val)
    {
        MainController.instance.mode = "preview";
        MainController.instance.STR_previewJsonAPI = val;
    }
   /* function OnAppReady()
    {

        console.log('on app ready crash');
        let gameID = '1283';
        let _template_name = 'G0016';
        console.log(gameID);
        if (gameID == 0)
        {

            let generatedGameName = ''
  

        console.log('generatedGameName: ' + generatedGameName);

            let accessLink
          if (generatedGameName != '')
            {
                _template_name = generatedGameName
          accessLink = '../../../../../Game_Generator/create_and_update_file.php'
          console.log('accessLink');
            }
            else
            {
                accessLink = '../../../../create_and_update_file.php'
            }


            console.log(accessLink);

            let encodedJSON = '';

            let getEncodedPreviewData = function() {
          $.get(accessLink, {
                template_name: _template_name
          }, function(response) {
                    // console.log('create_and_update_file.php response');
                    // console.log(response)
                    encodedJSON = response
            console.log(encodedJSON);

                    gameInstance.SendMessage('GameID', 'JS_getMode', atob(encodedJSON)) // call this for only on preview
          })
        }
            getEncodedPreviewData()
  

      }
        else if (gameID > 0)
        {

            if ('' == '')
            {
                child_data = '0'
            }
            else
            {
                child_data = ''
        }

            let jsonFormat = {
          child_id: child_data,
          game_id: gameID
        }

        console.log(jsonFormat);
        console.log(JSON.stringify(jsonFormat));
        gameInstance.SendMessage('GameID', 'JS_getID', JSON.stringify(jsonFormat)) // call this only for live


      }
}*/

}

