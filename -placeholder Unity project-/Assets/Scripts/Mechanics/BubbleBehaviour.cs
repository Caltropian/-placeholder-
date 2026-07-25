using UnityEngine;

public class BubbleBehaviour : MonoBehaviour
{
    private Rigidbody2D rb2d;
    [SerializeField]
    private float riseSpeed = 0.1f,
        swayIntensity = 0.1f;
    private float timeOfCreation;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnEnable()
    {
        rb2d = GetComponent<Rigidbody2D>();
        timeOfCreation = Time.time;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 currentPos = transform.position;
        Vector2 desiredPos = new Vector2(currentPos.x + Mathf.Sin(Time.time - timeOfCreation) * swayIntensity, currentPos.y + (riseSpeed * Time.deltaTime));

        rb2d.MovePosition(desiredPos);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        collision.gameObject.BroadcastMessage("OnBubbleCollide", SendMessageOptions.DontRequireReceiver);
        Destroy(gameObject);
    }
}
