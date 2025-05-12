using UnityEngine;

public class PowerUp : MonoBehaviour
{

    private static readonly int Animate = Animator.StringToHash("Animate");


    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Bomb.Range++;
            GetComponent<Animator>().SetTrigger("Animate");
            GetComponent<Collider2D>().enabled = false;
        }
    }

    public void OnDestroy()
    {
        
        {
            Destroy(gameObject);

        }
    }
}
