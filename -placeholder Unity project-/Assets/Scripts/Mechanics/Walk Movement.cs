using UnityEngine;

public class WalkMovement : MonoBehaviour
{
    [Header("Movement Parameters")]
    [SerializeField]
    private float walkSpeed = 1f;
    [SerializeField]
    private PlayerState playerState;

    private Rigidbody2D rb2d;

    public bool IsMoving = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        if (playerState == null)
        {
            playerState = GetComponent<PlayerState>();
        }
    }

    private Vector2 moveValue = new(0, 0);
    public void Move(bool isMoving, Vector2 normalizedAxis)
    {
        moveValue = normalizedAxis;
        IsMoving = isMoving;
    }
    void OnDisable()
    {

        if (!gameObject.scene.isLoaded) return;
        AudioContext.Instance.PlayerAudioEmitter.PlaySwimmingWaterSfx(stopAudio: true, PlayerState.PlayerStates.ABOVEWATER);
    }


    void FixedUpdate()
    {
        if (moveValue.y > 0)
        {
            moveValue.y = 0f;
        }
        float gravityModifier = rb2d.gravityScale > 1 ? rb2d.gravityScale : 1;
        rb2d.AddForce(moveValue * walkSpeed * gravityModifier);
        float currentRotation = transform.rotation.eulerAngles.z;
        rb2d.MoveRotation(Mathf.MoveTowardsAngle(currentRotation, 0, 1));
        if (!playerState.HasFloorBeneath && IsMoving)
        {
            AudioContext.Instance.PlayerAudioEmitter.PlaySwimmingWaterSfx(stopAudio: false, PlayerState.PlayerStates.ABOVEWATER);
        }
        else
        {
            AudioContext.Instance.PlayerAudioEmitter.PlaySwimmingWaterSfx(stopAudio: true, PlayerState.PlayerStates.ABOVEWATER);
        }
    }
}
