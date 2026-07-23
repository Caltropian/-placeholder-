using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class SwimMovement : MonoBehaviour
{
    private InputAction moveAction,
        boostAction;

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
    private float rotationSpeed = 1f;
    [Header("Dependencies")]

    private Rigidbody2D rb2d;
    private void OnEnable()
    {
        //moveAction is a vector2, boostAction is a button press
        moveAction = InputSystem.actions.FindAction("Move");
        boostAction = InputSystem.actions.FindAction("Sprint");
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        
    }

    // FixedUpdate is called once every physics tick
    void FixedUpdate()
    {
        Vector2 moveValue = moveAction.ReadValue<Vector2>();
        Vector2 currentVelocity = rb2d.linearVelocity;
        Vector2 moveForce = moveValue * swimSpeed;
        Vector2 boostForce = Vector2.zero;

        if (boostAction.IsPressed())
        {
            strokeCooldown -= Time.deltaTime * strokeHoldModifier;
            moveForce *= boostSteerMod;
            if (strokeCooldown < 0)
            {
                boostForce = angle_to_vector(transform.rotation.eulerAngles.z) * boostSpeed;
                strokeCooldown = strokeMaxCooldown;
            }
        }
        else
        {
            strokeCooldown -= Time.deltaTime;
        }

        rb2d.AddForce(moveForce);
        rb2d.AddForce(boostForce, ForceMode2D.Impulse);

        if (moveAction.IsPressed())
        {
            //Slowly turns towards direction of movement
            float desiredRotation = vector_to_angle(rb2d.linearVelocity);
            float currentRotation = transform.rotation.eulerAngles.z;

            rb2d.MoveRotation(Mathf.MoveTowardsAngle(currentRotation, desiredRotation, rotationSpeed));
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
