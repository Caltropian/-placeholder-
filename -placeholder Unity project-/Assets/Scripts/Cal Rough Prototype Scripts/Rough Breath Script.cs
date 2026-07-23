using UnityEngine;

public class RoughBreathScript : MonoBehaviour
{
    [SerializeField] float maxBreath;
    [SerializeField]public float currentBreath;
    [SerializeField] Rigidbody2D rb2d;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentBreath = maxBreath;
    }

    // Update is called once per frame
    void Update()
    {
        currentBreath -= Time.deltaTime;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Respawn")
        {
            currentBreath = maxBreath;
            float yLevel = collision.bounds.min.y;
            rb2d.gravityScale = (transform.position.y - yLevel)/2 + 0.2f;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Respawn")
        {
            rb2d.gravityScale = 0;
        }
    }
}
