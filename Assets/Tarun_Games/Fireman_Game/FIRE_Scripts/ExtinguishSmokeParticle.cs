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
            if (firemanController.I_fire == firemanController.I_fireCount)
            {
                PlayerController.Instance.B_floorCleared = true;


                if (PlayerController.Instance.G_currentPlatform != null)
                    PlayerController.Instance.G_currentPlatform.GetComponent<FiremanPlatform>().B_platformCleared = true;
                if (PlayerController.Instance.G_extinguishButton != null)
                    PlayerController.Instance.G_extinguishButton.GetComponent<Button>().interactable = false;
                if (PlayerController.Instance.G_ladderButton != null)
                    PlayerController.Instance.G_ladderButton.GetComponent<Button>().interactable = true;

                if (PlayerController.Instance.G_currentPlatform != null)
                {
                    if (PlayerController.Instance.G_currentPlatform.GetComponent<FiremanPlatform>() != null)
                    {
                        if (!PlayerController.Instance.G_currentPlatform.GetComponent<FiremanPlatform>().B_questionCleared)
                        {
                            firemanController.THI_showQuestion();
                            PlayerController.Instance.G_currentPlatform.GetComponent<FiremanPlatform>().B_questionCleared = true;
                        }
                    }
                }
            }
        }
    }
}