using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class RoughWaterMovement : MonoBehaviour
{
    Rigidbody2D rb2d;
    [SerializeField] 
    float swimAccel,
        swimSpeedCap;
    [SerializeField] 
    float dashSpeed,
        dashCooldown,
        dashMaxCooldown;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Quick keyboard controls
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

        if (input.wKey.isPressed)
        {
            dir.y +=1;
        }
        else if (input.sKey.isPressed)
        {
            dir.y -= 1;
        }
        dashCooldown -= Time.deltaTime;


        float dash = 0;
        if (input.shiftKey.isPressed && dashCooldown <= 0)
        {
            dash = dashSpeed;
            dashCooldown = dashMaxCooldown;
        }



        rb2d.AddForce(dir * (swimAccel + dash));

        //Caps the swim speed
        //At least, it did. To implement the dash easily, I switched this responsibility to the linear dampening in the rigidbody
        /*if (Mathf.Abs(rb2d.linearVelocityX) > swimSpeedCap)
        {
            rb2d.linearVelocityX = swimSpeedCap * Mathf.Sign(rb2d.linearVelocityX);
        }
        if (Mathf.Abs(rb2d.linearVelocityY) > swimSpeedCap)
        {
            rb2d.linearVelocityY = swimSpeedCap * Mathf.Sign(rb2d.linearVelocityY);
        }*/


        rb2d.rotation = euler_angle(rb2d.linearVelocity);
    }

    //function scraped and modified from stackOverflow
    //Takes a vector2 and returns the euler angle
    float euler_angle(Vector2 vector)
    {
        var rad = Mathf.Atan(vector.y / vector.x);   // arcus tangent in radians
        var deg = rad * 180 / Mathf.PI;  // converted to degrees
        if (vector.x < 0) deg += 180;        // fixed mirrored angle of arctan
        var eul = (270 + deg) % 360;    // folded to [0,360) domain
        return eul;
    }
}
