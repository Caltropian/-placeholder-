using UnityEngine;

public class BubbleBehaviour : MonoBehaviour
{
    private static readonly int DieHash = Animator.StringToHash("Die");
    private Rigidbody2D rb2d;
    [SerializeField]
    private float riseSpeed = 0.1f,
        swayIntensity = 0.1f;
    private float timeOfCreation;
    [SerializeField]
    Collider2D circleCollider;
    [SerializeField]
    private Animator animator;
    private float timeToDie;

    void OnEnable()
    {
        rb2d = GetComponent<Rigidbody2D>();
        timeOfCreation = Time.time;
    }

    void FixedUpdate()
    {
        Vector2 currentPos = transform.position;
        Vector2 desiredPos = new Vector2(currentPos.x + Mathf.Sin(Time.time - timeOfCreation) * swayIntensity, currentPos.y + (riseSpeed * Time.deltaTime));

        rb2d.MovePosition(desiredPos);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("CaveSurface"))
        {
            collision.gameObject.BroadcastMessage("OnBubbleCollide", SendMessageOptions.DontRequireReceiver);
            circleCollider.enabled = false;
            animator.SetTrigger(DieHash);
            AnimatorClipInfo[] m_CurrentClipInfo = animator.GetCurrentAnimatorClipInfo(0);
            timeToDie = m_CurrentClipInfo[0].clip.length;
            Invoke(nameof(Die), timeToDie);
        }
    }
    private void Die()
    {
        Destroy(gameObject);
    }
}
