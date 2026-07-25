using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class WallColliderChecker : MonoBehaviour
{
    [SerializeField]
    private PlayerState playerState;

    void Awake()
    {
        if (playerState == null)
        {
            playerState = GetComponent<PlayerState>();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("CaveSurface") && !playerState.HasFloorBeneath)
        {
            AudioContext.Instance.PlayerAudioEmitter.PlaySfx(PlayerAudioEmitter.PlayerSFXTypes.Wallhit);
        }
    }
}
