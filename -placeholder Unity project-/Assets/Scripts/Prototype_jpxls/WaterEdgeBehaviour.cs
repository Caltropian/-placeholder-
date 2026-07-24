using UnityEngine;

public class WaterEdgeBehaviour : MonoBehaviour
{
    [SerializeField]
    private Transform objectCenter;
    [SerializeField]
    private Transform lowerEdge;
    [Header("Water Edge Settings")]
    [SerializeField]
    private float upperGravityMax = 9.8f;
    [SerializeField]
    private float lowerGravityMax = 2.0f;
    [SerializeField]
    private float nonPlungableGravity = 0.1f;
    [SerializeField]
    private bool IsPlungable = true;
    Rigidbody2D objRigidbody;
    PlayerState playerState;
    PlayerInputs playerInputs;


    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //Change this so there's a big player script that contains all its pertinent dependencies. 
            //Calling multiple GetComponents like so can be bad for performance.
            objRigidbody = collision.gameObject.GetComponent<Rigidbody2D>();
            playerState = collision.gameObject.GetComponent<PlayerState>();
            playerInputs = collision.gameObject.GetComponent<PlayerInputs>();
            if (!IsPlungable)
            {
                upperGravityMax = nonPlungableGravity;
                lowerGravityMax = nonPlungableGravity;
            }
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (objRigidbody.position.y - objectCenter.position.y < 0)
            {
                objRigidbody.gravityScale = 0;
                if (playerState.CurrentState != PlayerState.PlayerStates.UNDERWATER)
                {
                    playerState.CurrentState = PlayerState.PlayerStates.UNDERWATER;
                }
            }
            else
            {
                objRigidbody.gravityScale = upperGravityMax;
            }
            playerInputs.CanPlunge = false;
            objRigidbody = null;
            playerState = null;
        }
    }

    void FixedUpdate()
    {
        if (objRigidbody != null)
        {
            //Adjust gravity based on distance to center.
            //Get Difference in Y
            float yDiff = objRigidbody.position.y - objectCenter.position.y;
            float lowerYDiff = objRigidbody.position.y - lowerEdge.position.y;
            //If the player is below the center point.
            if (yDiff <= 0)
            {
                objRigidbody.gravityScale = Mathf.Lerp(-lowerGravityMax, 0, yDiff);
            }
            if (yDiff > 0)
            {
                objRigidbody.gravityScale = Mathf.Lerp(0, upperGravityMax, yDiff);
            }
            if (lowerYDiff <= 0)
            {
                if (playerState.CurrentState != PlayerState.PlayerStates.UNDERWATER)
                {
                    playerState.CurrentState = PlayerState.PlayerStates.UNDERWATER;
                }
                playerInputs.CanPlunge = false;
            }
            else
            {
                if (playerState.CurrentState == PlayerState.PlayerStates.UNDERWATER)
                {
                    playerState.CurrentState = PlayerState.PlayerStates.ABOVEWATER;
                }
                if (IsPlungable) playerInputs.CanPlunge = true;
            }
            /*if (objRigidbody.position.y >= upperEdge.position.y)
            {
                Debug.Log("Adding force");
                objRigidbody.AddForceY(-upperEdgeForce, ForceMode2D.Force);
            }
            if (objRigidbody.position.y <= lowerEdge.position.y)
            {
                Debug.Log("Adding force");
                objRigidbody.AddForceY(lowerEdgeForce, ForceMode2D.Force);
            }*/
        }
    }
}
