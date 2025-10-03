using DLearners;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : Singleton<PlayerController>
{

    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Rigidbody2D rigidbody2D;


    public GameObject G_extinguishFXPrefab;
    public Transform T_extinguisherPosright;
    public Transform T_extinguisherPosleft;

    public GameObject G_ladderPrefab;
    public GameObject G_currentLadder;

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

    public GameObject G_ladderButton;
    public GameObject G_extinguishButton;

    

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
    [SerializeField] private Button ladderButton;
    [SerializeField] private Button fireButton;

    #region Unity
    private void Start()
    {
        B_moveRight = B_moveLeft = B_canClimb = false;
        F_maxHealth = F_currentHealth = 100f;
        ladderButton.onClick.AddListener(() => { OnSpawnLadder(); });
        fireButton.onClick.AddListener(() => { OnExtinguishFire();});

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
                G_ladderButton.GetComponent<Button>().interactable = true;

            }
            else
            {
                G_ladderButton.GetComponent<Button>().interactable = false;
                Invoke(nameof(INVOKEextinguish), 0.5f);
            }
        }
        else if (collision.gameObject.transform.parent.name == "terrace")
        {
            G_extinguishButton.GetComponent<Button>().interactable = false;
            G_ladderButton.GetComponent<Button>().interactable = false;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == G_currentLadder) // ladder
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
        if (collision.gameObject == G_currentLadder)
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
        G_extinguishButton.GetComponent<Button>().interactable = true;
        if (G_currentLadder != null)
            Destroy(G_currentLadder);
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
        G_ladderButton.GetComponent<Button>().interactable = true;
        //transform.position = SpawnPos;//Tarun
        HUDManager.Instance.UpdateScoreText(false);//points need to be 0
    }


    #region Buttons
    private void OnExtinguishFire()
    {
        if (!B_dead)
        {
            DLearnersAudioManager.Instance.PlaySound("Fire_Small_Fire_Out");
            var extinguishSmoke = Instantiate(G_extinguishFXPrefab);
            if (!GetComponent<SpriteRenderer>().flipX)
            {
                extinguishSmoke.transform.position = T_extinguisherPosright.position;
            }
            else
            {
                extinguishSmoke.transform.position = T_extinguisherPosleft.position;
            }
        }
    }
    private void OnSpawnLadder()
    {
        if (!B_dead)
        {
            DLearnersAudioManager.Instance.PlaySound("Fire_Lader");

            if (G_currentLadder != null)
                Destroy(G_currentLadder);
            G_currentLadder = Instantiate(G_ladderPrefab);
            if (!spriteRenderer.flipX)
                G_currentLadder.transform.position = new Vector3(transform.position.x + 1f, transform.position.y + 1.25f);

            if (spriteRenderer.flipX)
                G_currentLadder.transform.position = new Vector3(transform.position.x - 1f, transform.position.y + 1.25f);
        }
    }
    #endregion
}
