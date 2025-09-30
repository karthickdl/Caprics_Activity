using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExtinguishSmokeParticle : MonoBehaviour
{

    private FiremanController firemanController => (FiremanController)FiremanController.Instance;
    private void OnParticleCollision(GameObject other)
    {
        if (other.name == "Fire")
        {

    var coin = Instantiate(firemanController.G_coinPrefab);
            coin.transform.position = new Vector3(other.transform.position.x, other.transform.position.y + 2f);
            Destroy(other);
            firemanController.I_fire++;
            if(firemanController.I_fire== firemanController.I_fireCount)
            {
                firemanController.B_floorCleared = true;


                if (firemanController.G_currentPlatform != null)
                    firemanController.G_currentPlatform.GetComponent<fireman_platform>().B_platformCleared = true;
                if (firemanController.G_extinguishButton != null)
                    firemanController.G_extinguishButton.GetComponent<Button>().interactable = false;
                if (firemanController.G_ladderButton != null)
                    firemanController.G_ladderButton.GetComponent<Button>().interactable = true;

                if (firemanController.G_currentPlatform != null)
                {
                    if (firemanController.G_currentPlatform.GetComponent<fireman_platform>() != null)
                    {
                        if (!firemanController.G_currentPlatform.GetComponent<fireman_platform>().B_questionCleared)
                        {
                            firemanController.THI_showQuestion();
                            firemanController.G_currentPlatform.GetComponent<fireman_platform>().B_questionCleared = true;
                        }
                    }
                }
            }
        }
    }


}
