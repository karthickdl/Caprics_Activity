using UnityEngine;

public class QBOX : MonoBehaviour
{
    public string STR_Selected;
    public AudioSource AS_Droping;
    public Rigidbody2D rb2D;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject != null)
        {
            STR_Selected = collision.gameObject.name;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        STR_Selected = "";
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Opt 2" || collision.gameObject.name == "Opt 3")
        {
            Debug.Log(collision.gameObject.name);
            AS_Droping.Play();

        }
    }
}
