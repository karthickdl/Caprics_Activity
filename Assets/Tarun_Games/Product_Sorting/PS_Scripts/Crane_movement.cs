using UnityEngine;
using UnityEngine.UI;

public class Crane_movement : MonoBehaviour
{
    
    [SerializeField] private Joystick FixedJoystick;
    [SerializeField] private GameObject craneOBJ;
    public QBOX qBOX;

    [SerializeField] private float speed;

    public bool B_MoveForward, B_MoveBackward;
    
    public GameObject G_Box1, G_Box2;
    public bool B_Lerp1, B_Lerp2;
    [SerializeField] private Button dropButton;
    [SerializeField] private Vector3 tmpPos, tmpPos1;
    
    // Start is called before the first frame update
    void Start()
    {
        B_Lerp1 = true;
        B_Lerp2 = false;
        Invoke(nameof(Offlerp), 3f);
        FixedJoystick.gameObject.SetActive(false);
        dropButton.onClick.AddListener(() => { BUT_DropDown(); });
        SetDropButtonOnOff(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (FixedJoystick.Horizontal > 0 || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            B_MoveForward = true;
            B_MoveBackward = false;
        }
        else if (FixedJoystick.Horizontal < 0 || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            B_MoveForward = false;
            B_MoveBackward = true;
        }
        else
        {
            B_MoveBackward = false;
            B_MoveForward = false;
        }



        if (B_MoveForward)
        {
            craneOBJ.transform.Translate(Vector2.right * speed * Time.deltaTime);
            qBOX.transform.Translate(Vector2.right * speed * Time.deltaTime);
            THI_Poscheck();
            if (!craneOBJ.GetComponent<AudioSource>().isPlaying)
            craneOBJ.GetComponent<AudioSource>().Play();

        }else
        if (B_MoveBackward)
        {
            craneOBJ.transform.Translate(Vector2.left * speed * Time.deltaTime);
            qBOX.transform.Translate(Vector2.left * speed * Time.deltaTime);


            THI_Poscheck();

            if (!craneOBJ.GetComponent<AudioSource>().isPlaying)
                craneOBJ.GetComponent<AudioSource>().Play();
        }else
        {
            craneOBJ.GetComponent<AudioSource>().Stop();
        }

        if(B_Lerp1)
        {
          //  Debug.Log("Calling Lepr1 Start");
            this.gameObject.transform.GetChild(1).transform.position = Vector3.Lerp(this.gameObject.transform.GetChild(1).transform.position, G_Box2.transform.position, 2f*Time.deltaTime);
           
          //  Debug.Log("Calling Lepr1 End");
        }

        if (B_Lerp2)
        {
          //  Debug.Log("Calling Lepr2 Start");

            this.gameObject.transform.GetChild(0).gameObject.SetActive(true);
            this.gameObject.transform.GetChild(0).transform.position = Vector3.Lerp(this.gameObject.transform.GetChild(0).transform.position, G_Box2.transform.position, 2f * Time.deltaTime);
            

           
          //  Debug.Log("Calling Lepr2 End");
        }

       
    }

    void THI_Poscheck()
    {
        tmpPos = craneOBJ.transform.position;
        tmpPos.x = Mathf.Clamp(tmpPos.x, -5f, 8f);
        // tmpPos.y = Mathf.Clamp(tmpPos.y, -3f, 2f);
        craneOBJ.transform.position = tmpPos;

        tmpPos1 = qBOX.transform.position;
        tmpPos1.x = Mathf.Clamp(tmpPos.x, -5f, 8f);
        // tmpPos.y = Mathf.Clamp(tmpPos.y, -3f, 2f);
        qBOX.transform.position = tmpPos;
    }

    void Offlerp()
    {
        B_Lerp1 = false;
        B_Lerp2 = true;
        Invoke(nameof(Offlerp2), 3f);
    }

    void Offlerp2()
    {
        B_Lerp2 = false;
        Debug.Log(B_Lerp2);
        B_Lerp1 = false;
        
        Offlerp3();
    }

    void Offlerp3()
    {
        Debug.Log("Before =" + this.gameObject.transform.GetChild(1).transform.position.y);
        this.gameObject.transform.GetChild(1).transform.position = new Vector3(this.gameObject.transform.GetChild(1).transform.position.x, this.gameObject.transform.GetChild(1).transform.position.y + 1f, this.gameObject.transform.GetChild(1).transform.position.z);
        this.gameObject.transform.GetChild(0).transform.position = new Vector3(this.gameObject.transform.GetChild(0).transform.position.x, this.gameObject.transform.GetChild(0).transform.position.y + 1f, this.gameObject.transform.GetChild(0).transform.position.z);
        Debug.Log("After =" + this.gameObject.transform.GetChild(1).transform.position.y);

        FixedJoystick.gameObject.SetActive(true);
        SetDropButtonOnOff(true);
    }


    public void SetDropButtonOnOff(bool isOn)
    {
        dropButton.gameObject.SetActive(isOn);
    }


    private void BUT_DropDown()
    {
        if (qBOX.STR_Selected == "")
        {
            PS_Main.Instance.WrongAnswerSequence();
            THIDropDown();
        }
        else
        {
            if (qBOX.STR_Selected == PS_Main.Instance.GetCurrentQuestionAnswer())
            {
                PS_Main.Instance.CorrectAnswerSequence();
                THIDropDown();
            }
            else
            {
                PS_Main.Instance.WrongAnswerSequence();
                THIDropDown();
            }
        }
    }

    private void THIDropDown()
    {
        FixedJoystick.gameObject.SetActive(false);
        SetDropButtonOnOff(false);
        //ps_PS_Main.STR_currentQuestionAnswer = STR_Selected;
        qBOX.rb2D.gravityScale = 1f;
    }
}
