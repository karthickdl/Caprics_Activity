using DLearners;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : Singleton<PlayerController>
{

    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Rigidbody2D rigidbody2D;

    public GameObject coinPF;

    [Header("Extinguish Logics")]
    [SerializeField] private GameObject extinguishFXPF;
    [SerializeField] private Transform extinguisherPosRight;
    [SerializeField] private Transform extinguisherPosLeft;

    [Header("ladderPF")]
    [SerializeField] private GameObject ladderPF;
    private GameObject currentLadder;

    [Header("Movement Logics")]
    public bool B_moveRight;
    public bool B_moveLeft;
    public float moveSpeed;

    [Header("Climbing Logics")]
    public float climbSpeed;
    public bool B_canClimb;

    [Header("Health")]
    public float F_maxHealth;
    public float F_currentHealth;
    public Image IM_health;
    public bool B_dead;

    [Header("Baby")]
    public GameObject G_rope;
    public Transform T_ropeStartPos;
    public Transform T_ropeStopPos;
    public bool B_ropeStart;
    public float F_ropeLerpTimer;



    public bool B_floorCleared;
    public GameObject G_currentPlatform;

    public GameObject G_controlButtons;

    [Header ("Buttons")]
    public Button ladderButton;
    public Button extinguishButton;

    #region Unity
    private void Start()
    {
        B_moveRight = B_moveLeft = B_canClimb = false;
        F_maxHealth = F_currentHealth = 100f;
        ladderButton.onClick.AddListener(() => { OnSpawnLadder(); });
        extinguishButton.onClick.AddListener(() => { OnExtinguishFire();});

        //G_extinguishButton.GetComponent<Button>().interactable = false;//Tarun
    }
    private void Update()
    {
        keyboardControls();
        // if (EventSystem.current.currentSelectedGameObject == MainController.instance.G_coverPageStart && !firemancamera.GetComponent<Animator>().enabled)
       /* {
            firemancamera.GetComponent<Animator>().enabled = true;
            Invoke(nameof(THI_enableControlButtons), AC_introCam.length);
            IM_health.fillAmount = F_currentHealth / F_maxHealth;
        }


        if (B_birdFly && G_currentBird != null)
        {
            G_currentBird.transform.position = Vector3.MoveTowards(G_currentBird.transform.position, V_birdEnd, 0.075f);
        }
        if (B_dogRun && G_dog != null)
        {
            G_dog.transform.Translate(Vector2.left * F_dogSpeed * Time.deltaTime);
        }*/

        if (B_moveRight)
        {
            transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);
            animator.Play("firemanrun");
            spriteRenderer.flipX = false;
        }
        else if (B_moveLeft)
        {

            transform.Translate(Vector2.left * moveSpeed * Time.deltaTime);
            animator.Play("firemanrun");
            spriteRenderer.flipX = true;
        }
        else
        {

            if (!B_canClimb && !B_dead && !B_ropeStart)
            {
                animator.Play("firemanidle");
            }
        }
        if (B_canClimb)
        {
            rigidbody2D.isKinematic = true;
            transform.Translate(Vector2.up * climbSpeed * Time.deltaTime);
            animator.Play("firemanclimb");
        }
        else
        {
            rigidbody2D.isKinematic = false;
        }

        if (B_ropeStart)
        {
            transform.position = Vector3.Lerp(transform.position, T_ropeStopPos.position, F_ropeLerpTimer);
        }
    }
    private void keyboardControls()
    {
        if (!B_dead && !B_ropeStart)
        {
            if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                B_moveRight = true;
            }
            else if (Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.RightArrow))
            {
                B_moveRight = false;
            }
            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                B_moveLeft = true;
            }
            else if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.LeftArrow))
            {
                B_moveLeft = false;
            }
        }
    }
    #endregion

    #region Collision
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "fireman_platform")
        {
            // I_fire = 0;
            // I_fireCount = collision.gameObject.GetComponent<fireman_platform>().I_fireCount;
            G_currentPlatform = collision.gameObject;
            collision.gameObject.GetComponent<FiremanPlatform>().enabled = true;

            B_floorCleared = collision.gameObject.GetComponent<FiremanPlatform>().B_platformCleared;
            if (B_floorCleared)
            {
                ladderButton.interactable = true;
            }
            else
            {
                ladderButton.interactable = false;
                Invoke(nameof(INVOKEextinguish), 0.5f);
            }
        }
        else if (collision.gameObject.transform.parent.name == "terrace")
        {
            extinguishButton.interactable = false;
            ladderButton.interactable = false;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == currentLadder) // ladder
        {
            B_canClimb = true;
        }
        if (collision.gameObject.tag == "DL_coin") // coin   
        {
            DLearnersAudioManager.Instance.PlaySound("Fire_Coin");

            HUDManager.Instance.UpdateScoreText(true);
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.name == "Heart")
        {
            DLearnersAudioManager.Instance.PlaySound("Fire_Heast_Collect");
            F_currentHealth = F_maxHealth;
            IM_health.fillAmount = F_currentHealth / F_maxHealth;
            IM_health.color = Color.green;
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.name == "Baby")
        {
            G_rope.SetActive(true);
            animator.Play("firemanrope");
            transform.position = T_ropeStartPos.position;
            B_ropeStart = true;
            B_moveLeft = B_moveRight = false;
            B_canClimb = false;
            Destroy(GetComponent<Rigidbody2D>());
            Destroy(GetComponent<BoxCollider2D>());
            Destroy(collision.gameObject);
            G_controlButtons.SetActive(false);
            Invoke(nameof(FiremanController.Instance.OnLevelCompleted), 4f);
        }
    }    
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Fire")
        {
            if (F_currentHealth > 75f)
            {
                IM_health.color = Color.green;
            }
            if (F_currentHealth < 75f && F_currentHealth > 35f)
            {
                IM_health.color = Color.yellow;
            }
            if (F_currentHealth < 35f)
            {
                IM_health.color = Color.red;
            }
            if (F_currentHealth > 0)
            {
                F_currentHealth--;
                IM_health.fillAmount = F_currentHealth / F_maxHealth;
            }
            else
            {
                B_dead = true;

                DLearnersAudioManager.Instance.PlaySound("Fire_Dead");
                GetComponent<Collider2D>().enabled = false;
                F_currentHealth = 0;
                IM_health.transform.parent.transform.parent.gameObject.SetActive(false);
                animator.Play("firemandie");
                spriteRenderer.color = Color.white;
                Invoke(nameof(Respawn), 2f);
                return;
            }
            spriteRenderer.color = Color.red;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == currentLadder)
        {
            B_canClimb = false;
        }
        if (collision.gameObject.name == "Fire")
        {
            spriteRenderer.color = Color.white;
        }
    }
    #endregion

    void INVOKEextinguish()
    {
        extinguishButton.interactable = true;
        if (currentLadder != null)
            Destroy(currentLadder);
    }
    private void Respawn()
    {
        B_dead = false;
        GetComponent<Collider2D>().enabled = true;
        F_currentHealth = 100f;
        IM_health.fillAmount = F_currentHealth / F_maxHealth;
        IM_health.color = Color.green;
        IM_health.transform.parent.transform.parent.gameObject.SetActive(true);
        GetComponent<SpriteRenderer>().color = Color.white;
        B_moveLeft = B_moveRight = false;
        ladderButton.GetComponent<Button>().interactable = true;
        //transform.position = SpawnPos;//Tarun
        HUDManager.Instance.UpdateScoreText(false);//points need to be 0
    }


    #region Buttons
    private void OnExtinguishFire()
    {
        if (!B_dead)
        {
            DLearnersAudioManager.Instance.PlaySound("Fire_Small_Fire_Out");
            var extinguishSmoke = Instantiate(extinguishFXPF);
            if (!GetComponent<SpriteRenderer>().flipX)
            {
                extinguishSmoke.transform.position = extinguisherPosRight.position;
            }
            else
            {
                extinguishSmoke.transform.position = extinguisherPosLeft.position;
            }
        }
    }
    private void OnSpawnLadder()
    {
        if (!B_dead)
        {
            DLearnersAudioManager.Instance.PlaySound("Fire_Lader");

            if (currentLadder != null)
                Destroy(currentLadder);
            currentLadder = Instantiate(ladderPF);
            if (!spriteRenderer.flipX)
                currentLadder.transform.position = new Vector3(transform.position.x + 1f, transform.position.y + 1.25f);

            if (spriteRenderer.flipX)
                currentLadder.transform.position = new Vector3(transform.position.x - 1f, transform.position.y + 1.25f);
        }
    }
    #endregion
}
