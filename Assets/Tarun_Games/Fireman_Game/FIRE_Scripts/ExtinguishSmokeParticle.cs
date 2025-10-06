using UnityEngine;

public class ExtinguishSmokeParticle : MonoBehaviour
{
    private FiremanController firemanController => (FiremanController)FiremanController.Instance;
    private void OnParticleCollision(GameObject other)
    {
        if (other.name == "Fire")
        {
            var coin = Instantiate(PlayerController.Instance.coinPF, new Vector3(other.transform.position.x, other.transform.position.y + 2f),Quaternion.identity);
            coin.transform.GetComponent<CoinPF>().CoinMoveToPoint(PlayerController.Instance.transform.position,0.5f);
            Destroy(other);

            firemanController.I_fire++;
            if (firemanController.I_fire == firemanController.I_fireCount)
            {
                PlayerController.Instance.B_floorCleared = true;


                if (PlayerController.Instance.G_currentPlatform != null)
                    PlayerController.Instance.G_currentPlatform.GetComponent<FiremanPlatform>().B_platformCleared = true;
                if (PlayerController.Instance.extinguishButton != null)
                    PlayerController.Instance.extinguishButton.interactable = false;
                if (PlayerController.Instance.ladderButton != null)
                    PlayerController.Instance.ladderButton.interactable = true;

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