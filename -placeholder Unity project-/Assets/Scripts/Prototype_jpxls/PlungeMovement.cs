using UnityEngine;
using UnityEngine.Assertions;

[RequireComponent(typeof(Rigidbody2D))]
public class PlungeMovement : MonoBehaviour
{
    [SerializeField]
    private float plungePower = 20f;
    private Rigidbody2D rb;
    private Vector2 directionToPlunge;
    [SerializeField]
    private float plungeCD = 1.0f;
    [SerializeField]
    private float timer = 1.0f;

    private bool WillPlunge = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void OnEnable()
    {
        WillPlunge = false;
    }
    public void Plunge(Vector2 direction)
    {
        Assert.AreNotEqual(direction.y >= 0, true);
        if (timer >= plungeCD)
        {
            directionToPlunge = new(0, -1);
            timer = plungeCD;
            WillPlunge = true;
        }
    }
    void Update()
    {
        if (timer < plungeCD)
        {
            timer += Time.deltaTime;
        }
    }
    void FixedUpdate()
    {
        if (timer >= plungeCD && WillPlunge)
        {
            rb.AddForce(directionToPlunge * plungePower, ForceMode2D.Impulse);
            timer = 0.0f;
            WillPlunge = false;
        }
    }
}
