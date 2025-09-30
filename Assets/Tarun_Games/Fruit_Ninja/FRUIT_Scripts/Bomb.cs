using UnityEngine;
using UnityEngine.UI;

public class Bomb : MonoBehaviour
{
    [SerializeField] private GameObject bombeffectPF;
    private float startforce = 12f;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Image img;
    [SerializeField] private Collider2D collider2D;

    private void Start()
    {
        rb.AddForce(transform.up * startforce, ForceMode2D.Impulse);
        img.enabled = true;
        collider2D.enabled = true;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Blade")
        {
           FruitNinja_Main.Instance.WrongAnswerSequence();
            Vector3 pos= transform.position;
            img.enabled = false;
            collider2D.enabled = false;
            startforce = 0;
            GameObject slice = Instantiate(bombeffectPF, transform.parent.transform);
            slice.transform.position = pos;
            
            Destroy(this.gameObject,2f);
        }
    }
}
