
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class SwimMovement : MonoBehaviour
{
    internal enum RotationMethod
    {
        MoveTowardsAngle,
        NoSmoothing,
        RotateOnSideKeys
    }
    [Header("Movement Parameters")]
    [SerializeField]
    private float swimSpeed = 4f;
    [SerializeField]
    private float boostSpeed = 10f,

        boostSteerMod = 0.2f;
    [SerializeField]

    private float strokeMaxCooldown = 0.6f,
        strokeCooldown = 0f,
        strokeHoldModifier = 1.1f;

    [Header("Rotation Parameters")]
    [SerializeField]
    private float startingRotationSpeed = 8f;
    [SerializeField]
    private float rotationSpeed = 1f;
    [SerializeField]
    private float timeToReachMaxSpeed = 0.4f;
    [Header("Dependencies")]
    private Rigidbody2D rb2d;
    [SerializeField]
    private RotationMethod rotationMethod;

    private float localRotationTimer = 0.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();

    }
    void OnDisable()
    {
        if (!gameObject.scene.isLoaded) return;
        AudioContext.Instance.PlayerAudioEmitter.PlaySwimmingWaterSfx(stopAudio: true, PlayerState.PlayerStates.UNDERWATER);
    }
    private Vector2 moveValue = new(0, 0);
    public bool IsBoosting = false;
    public bool IsMoving = false;
    public void Move(bool isMoving, Vector2 normalizedAxis)
    {
        if (normalizedAxis.x != moveValue.x)
        {
            localRotationTimer = 0;
        }
        moveValue = normalizedAxis;
        IsMoving = isMoving;
    }
    public void Boost(bool isBoosting)
    {
        IsBoosting = isBoosting;
    }
    void FixedUpdate()
    {
        Vector2 moveForce = moveValue * swimSpeed;
        Vector2 boostForce = Vector2.zero;
        if (rotationMethod == RotationMethod.RotateOnSideKeys)
        {
            moveForce = moveValue.y * swimSpeed * transform.up;
        }
        if (IsBoosting)
        {
            strokeCooldown -= Time.deltaTime * strokeHoldModifier;
            moveForce *= boostSteerMod;
            if (strokeCooldown < 0)
            {
                boostForce = angle_to_vector(transform.rotation.eulerAngles.z) * boostSpeed;
                strokeCooldown = strokeMaxCooldown;
                //Play BreastStroke Oneshot
                AudioContext.Instance.PlayerAudioEmitter.PlaySfx(PlayerAudioEmitter.PlayerSFXTypes.Breaststroke);
            }
        }
        else
        {
            strokeCooldown -= Time.deltaTime;
        }

        rb2d.AddForce(moveForce);
        rb2d.AddForce(boostForce, ForceMode2D.Impulse);

        if (IsMoving)
        {
            if (rotationMethod == RotationMethod.MoveTowardsAngle)
            {
                float desiredRotation = vector_to_angle(rb2d.linearVelocity);
                float currentRotation = transform.rotation.eulerAngles.z;
                rb2d.MoveRotation(Mathf.MoveTowardsAngle(currentRotation, desiredRotation, rotationSpeed));
            }
            else if (rotationMethod == RotationMethod.RotateOnSideKeys)
            {

                localRotationTimer += Time.fixedDeltaTime / timeToReachMaxSpeed;
                float finalRotationSpeed = rotationSpeed * Mathf.SmoothStep(startingRotationSpeed, rotationSpeed, localRotationTimer);
                if (IsBoosting) finalRotationSpeed *= boostSteerMod;
                rb2d.MoveRotation(rb2d.rotation + (moveValue.x * -1 * finalRotationSpeed * Time.fixedDeltaTime));
            }
            else
            {
                if (vector_to_angle(rb2d.linearVelocity) != float.NaN)
                {
                    rb2d.rotation = vector_to_angle(rb2d.linearVelocity);
                }
            }
        }
        //Instead of isMoving, maybe do volume-based according to linearVelocity.
        if (IsMoving)
        {
            //Trigger Audio SFX
            AudioContext.Instance.PlayerAudioEmitter.PlaySwimmingWaterSfx(stopAudio: false, PlayerState.PlayerStates.UNDERWATER);
        }
        else if (!IsBoosting)
        {
            AudioContext.Instance.PlayerAudioEmitter.PlaySwimmingWaterSfx(stopAudio: true, PlayerState.PlayerStates.UNDERWATER);
        }
    }
    /// <summary>
    /// Takes a directional vector and returns the corresponding angle. 
    /// </summary>
    /// <param name="vector">Direction vector</param>
    /// <returns>Angle corresponding to direction vector, starting from north</returns>
    float vector_to_angle(Vector2 vector)
    {
        var rad = Mathf.Atan(vector.y / vector.x);   // arcus tangent in radians
        var deg = rad * 180 / Mathf.PI;  // converted to degrees
        if (vector.x < 0) deg += 180;        // fixed mirrored angle of arctan
        var eul = (270 + deg) % 360;    // folded to [0,360) domain
        return eul;
    }

    Vector2 angle_to_vector(float angle)
    {
        float angleRad = angle * Mathf.Deg2Rad;
        return new Vector2(-Mathf.Sin(angleRad), Mathf.Cos(angleRad));
    }
}
