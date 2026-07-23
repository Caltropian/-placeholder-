using UnityEngine;

public class RoughPlayerStateController : MonoBehaviour
{
    Rigidbody2D rb2d;
    RoughWaterMovement water;
    RoughWalkMovement walk;
    [SerializeField] bool swimming;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        water = GetComponent<RoughWaterMovement>();
        walk = GetComponent<RoughWalkMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Keeps track of whether the player should be walking or swimming using dedicated trigger zones.
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Walk Trigger")
        {
            swimming = false;
            walk.enabled = true;
            water.enabled = false;
            //rb2d.gravityScale = 1;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Respawn")
        {
            swimming = true;
            walk.enabled = false;
            water.enabled = true;
            //rb2d.gravityScale = 0;
        }
    }
}
