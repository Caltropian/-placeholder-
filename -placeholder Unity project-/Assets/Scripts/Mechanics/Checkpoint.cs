using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField]
    private Transform respawnPosition;
    private PlayerState playerState;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (!playerState)
            {
                playerState = collision.GetComponent<PlayerState>();
            }
            playerState.CurrentCheckpoint = respawnPosition;
        }
    }
}
