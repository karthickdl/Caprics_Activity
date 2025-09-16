using DG.Tweening;
using DLearners;
using UnityEngine;

public class Robotmovement : Singleton<Robotmovement>
{
    [SerializeField] private float movementsped;
    [SerializeField] private float offset;

    private Vector2 startpostion;
    [SerializeField] private Rigidbody2D RB2D_robot;
    [SerializeField] private float jumpspeed;
    [SerializeField] private Animator animator;

    private bool B_canjump;
    private bool B_reducelife;

    [SerializeField] private AnimationClip AC_blast;
    [SerializeField] private GameObject blast;
    private GameObject Local_blastanim;
    [SerializeField] private ParticleSystem smokeeffect;
    [SerializeField] private ParticleSystem stareffect;
    GameObject G_portal;

    public AnimationClip AC_portaldisapears;
    private AudioSource cashedWalking;
    public void Awake()
    {
        base.Awake();
        startpostion = transform.position;
    }
    private void Start()
    {
        cashedWalking = DLearnersAudioManager.Instance.PlaySoundCashed("AS_Walking");
       
        animator.Play("land");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Jump();
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Down();
        }
    }
    private void FixedUpdate()
    {
        transform.Translate(Vector2.right * movementsped);
    }
    public void RobotInIt()
    {
        animator.Play("land");
        movementsped = 0f;
        RB2D_robot.gravityScale = 0;
        transform.position = startpostion;
        B_reducelife = true;
        HUDManager.Instance.SetTapToPlayOnAndOff(true);
    }

    public void OnPlayButton()
    {
        RB2D_robot.gravityScale = 1.5f;
        FollowingCamera.Instance.Init(this.transform);
        FollowingCamera.Instance.canfollow = true;
        DLearnersAudioManager.Instance.PlayGameSpecificSound("AS_falling");
    }

    public void Jump()
    {
        if (B_canjump)
        {
            animator.Play("jump");
            DLearnersAudioManager.Instance.StopSound2("AS_Walking");
            DLearnersAudioManager.Instance.PlayGameSpecificSound("AS_Jumping");
            RB2D_robot.velocity = Vector2.up * jumpspeed;
        }
    }
    public void Down()
    {
        if (!B_canjump)
        {
            animator.Play("land");
            RB2D_robot.velocity = Vector2.down * jumpspeed;
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "star")
        {
            StarPlayParticle();
            DLearnersAudioManager.Instance.PlayCommonSound("Com_Collect");
            Destroy(collision.gameObject);
            HUDManager.Instance.UpdateScoreText(true,2);
        }
        else if (collision.gameObject.name == "portal")
        {
            G_portal = collision.gameObject;
            G_portal.GetComponent<Animator>().SetInteger("cond", 1);
            gameObject.SetActive(false);

            DOVirtual.DelayedCall(AC_portaldisapears.length ,() =>
            {
                NextQuest();
            });
            //Invoke("nextquest", AC_portaldisapears.length);
            DLearnersAudioManager.Instance.PlayGameSpecificSound("AS_Portal");
        }
    }
    
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Floor" || collision.gameObject.name == "floating")
        {
            movementsped = 0.1f;
            PlayParticle();
            animator.Play("walk");
            if (!cashedWalking.isPlaying)
            {
                cashedWalking.Play();
            }
            B_canjump = true;
        }
        else if (collision.gameObject.name == "out")
        {
            if (B_reducelife)
            {
                DLearnersAudioManager.Instance.StopSound2("AS_Walking");
                HUDManager.Instance.UpdateScoreText(false,10);
                Robot_Out();
            }
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Floor" || collision.gameObject.name == "floating")
        {
            StopParticle();
            animator.Play("jump");
            B_canjump = false;
        }
    }

    public void backtostart()
    {
        Destroy(Local_blastanim);
        gameObject.SetActive(true);
    }

    private RB_Runner_Main rB_Runner_Main => (RB_Runner_Main)RB_Runner_Main.Instance;
    public void NextQuest()
    {
        Destroy(G_portal);
        // RB_Runner_Main.Instance.THI_ShowQuestion();

       // GameManagerBase baseInstance = RB_Runner_Main.Instance;
        rB_Runner_Main.UpdateQuestion();
    }
    public void Robot_Out()
    {
        movementsped = 0;
        gameObject.SetActive(false);

        Local_blastanim = Instantiate(blast, transform.position,Quaternion.identity);

        B_reducelife = false;
        DOVirtual.DelayedCall(AC_blast.length ,() =>
        {
            THI_Outrespawn();
            Destroy(Local_blastanim);
        });
       // Invoke("THI_Outrespawn", AC_blast.length);
    }
    private void THI_Outrespawn()
    {
        Vector2 pos = this.transform.position;
        if (pos != startpostion)
        {
            pos = new Vector2(pos.x - 10, pos.y + 20);
            this.transform.position = pos;
        }

        this.gameObject.SetActive(true);
        animator.Play("land");

        RB2D_robot.gravityScale = 0;
        B_reducelife = true;
        HUDManager.Instance.SetTapToPlayOnAndOff(true);
    }
    private void PlayParticle()
    {
        smokeeffect.Play();
    }
    private void StopParticle()
    {
        smokeeffect.Stop();
    }
    private void StarPlayParticle()
    {
        stareffect.Play();
    }
}