using UnityEngine;

public class WaterEdgeBehaviour : MonoBehaviour
{
    [SerializeField]
    private Transform objectCenter;
    [SerializeField]
    private Transform upperEdge;
    [SerializeField]
    private Transform lowerEdge;
    [Header("Water Edge Settings")]
    [SerializeField]
    private float upperGravityMax = 9.8f;
    [SerializeField]
    private float lowerGravityMax = 2.0f;
    [SerializeField]
    private float upperEdgeForce = 30f;
    [SerializeField]
    private float lowerEdgeForce = 30f;
    Rigidbody2D objRigidbody;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            objRigidbody = collision.gameObject.GetComponent<Rigidbody2D>();
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (objRigidbody.position.y - objectCenter.position.y < 0)
            {
                objRigidbody.gravityScale = 0;
            }
            else
            {
                objRigidbody.gravityScale = upperGravityMax;
            }
            objRigidbody = null;
        }
    }

    void FixedUpdate()
    {
        if (objRigidbody != null)
        {
            //Adjust gravity based on distance to center.
            //Get Difference in Y
            float yDiff = objRigidbody.position.y - objectCenter.position.y;
            //If the player is below the center point.
            if (yDiff <= 0)
            {
                objRigidbody.gravityScale = Mathf.Lerp(-lowerGravityMax, 0, yDiff);
            }
            if (yDiff > 0)
            {
                objRigidbody.gravityScale = Mathf.Lerp(4, upperGravityMax, yDiff);
            }
            if (objRigidbody.position.y >= upperEdge.position.y)
            {
                Debug.Log("Adding force");
                objRigidbody.AddForceY(-upperEdgeForce, ForceMode2D.Force);
            }
            if (objRigidbody.position.y <= lowerEdge.position.y)
            {
                Debug.Log("Adding force");
                objRigidbody.AddForceY(lowerEdgeForce, ForceMode2D.Force);
            }
        }
    }
}
