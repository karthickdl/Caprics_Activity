using UnityEngine;
using UnityEngine.EventSystems;

public class Blade : MonoBehaviour
{
    public bool iscutting = false;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Camera cam;
    [SerializeField] private CircleCollider2D circleCollider;

    [SerializeField] private float mincutvelocity = .001f;

    [SerializeField] private GameObject trailPF;
    private GameObject currenttrail;

    [SerializeField] private AudioSource AS_Slicing;
    [SerializeField] private AudioClip clip;

    Vector2 previouspos;
    public bool formtrail = true;

    private void Start()
    {
        AS_Slicing.clip = clip;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            
            StartCutting();
        }
        else
        if (Input.GetMouseButtonUp(0))
        {
            StopCutting();
        }
        if (iscutting)
        {
            Updatecut();
        }
    }
    void Updatecut()
    {
        Vector2 newpos = cam.ScreenToWorldPoint(Input.mousePosition);
        rb.position = newpos;

        float velocity = (newpos - previouspos).magnitude * Time.deltaTime;
        if (velocity > mincutvelocity)
        {
            circleCollider.enabled = true;
            //formtrail = true;
        }
        else
        {
            circleCollider.enabled = false;
            //formtrail = false;
        }
        previouspos = newpos;
    }
    public void StartCutting()
    {
        iscutting = true;
        circleCollider.enabled = true;
        if (formtrail)
        {
            AS_Slicing.Play();
            currenttrail = Instantiate(trailPF, transform);
        }
        previouspos = cam.ScreenToWorldPoint(Input.mousePosition);

    }
    public void StopCutting()
    {
        iscutting = false;
        circleCollider.enabled = false;
        //formtrail = false;
        if (currenttrail != null)
        {
            currenttrail.transform.SetParent(null);
            Destroy(currenttrail);
        }

    }
}
