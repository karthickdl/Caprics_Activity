using UnityEngine;
using UnityEngine.UI;

public class Fruit : MonoBehaviour
{
    [SerializeField] private GameObject G_slice;
    private float startforce = 12f;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Image img;
    [SerializeField] private Collider2D collider2D;
    [SerializeField] private Text text;

    private void Start()
    {
        rb.AddForce(transform.up* startforce, ForceMode2D.Impulse);
        img.enabled = true;
        collider2D.enabled = true;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Blade")
        {
            FruitNinja_Main.Instance.GetCurrentSelectedAnswer(text.text);
            FruitNinja_Main.Instance.CheckAnswer();
            GameObject slice = Instantiate(G_slice, transform);
            slice.transform.position = transform.position;
            Invoke(nameof(triggerON), 1f);

            collider2D.enabled = false;
            text.gameObject.SetActive(false);
            img.enabled = false;
        }
    }
    private void triggerON()
    {
        for (int i = 0; i < 1; i++)
        {
            G_slice.transform.GetChild(i).GetComponent<Collider2D>().isTrigger = true;
        }
    }

    public void InitFruit(string _text)
    {
        text.text = _text;
    }
}
