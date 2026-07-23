using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class RoughWalkMovement : MonoBehaviour
{
    Rigidbody2D rb2d;
    [SerializeField] float walkSpeed;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //forces character to stand
        rb2d.MoveRotation(0);
        
        var input = Keyboard.current;
        Vector2 dir = new Vector2(0, 0);
        if (input.dKey.isPressed)
        {
            dir.x += 1;
        }
        else if (input.aKey.isPressed)
        {
            dir.x -= 1;
        }

        rb2d.linearVelocityX = dir.x * walkSpeed;
    }
}
