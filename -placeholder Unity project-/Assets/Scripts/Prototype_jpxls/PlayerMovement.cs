using System;
using System.Collections;
using System.Runtime.CompilerServices;
using NUnit.Framework;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Events;

namespace Prototype_jpxls
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField]
        private float movementSpeed;
        [SerializeField]
        private float extraSwimSpeed = 10f;
        [SerializeField]
        private float swimSpeedStirStrength = 8f;
        [SerializeField]
        private float swimRamptownTime = 0.3f;
        [SerializeField]
        private Rigidbody2D rb;
        [SerializeField]
        private float speedToStop = 0.5f;
        [Header("Dependencies")]
        [SerializeField]
        private PlayerStamina playerStamina;


        private Vector2 lastMovementAxis = new(0, 0);
        private Vector2 swimDirection = new(0, 0);
        private float speedToStopCountdown = 0.0f;
        private IEnumerator swimProcess;
        //private Vector2 currentSwimSpeed = new(0, 0);
        private float currentSwimSpeed = 0.0f;

        //ROTATION STUFF:
        [Header("Rotation Settings")]
        [Tooltip("Transform that will be rotated to match the player's facing direction.")]
        [SerializeField]
        private Transform targetTransformRotation;
        [SerializeField]
        private float rotationSeekSpeed = 0.1f;
        private float _roundedAngle = 180.0f;
        private Quaternion startingRotation = Quaternion.identity;
        private Quaternion endingRotation = Quaternion.identity;
        private float localrotationSeekTimer = 0.0f;

        public UnityEvent onSwimmingAction;
        void Start()
        {
            if (rb == null)
            {
                rb = GetComponent<Rigidbody2D>();
            }
            if (playerStamina == null)
            {
                playerStamina = GetComponent<PlayerStamina>();
            }
        }
        //Easy in Rotation.
        void Update()
        {
            if (startingRotation != endingRotation)
            {

                targetTransformRotation.rotation = Quaternion.Lerp(startingRotation, endingRotation, localrotationSeekTimer / rotationSeekSpeed);
                localrotationSeekTimer += Time.deltaTime;
            }
        }
        public void StopSwim()
        {
            if (swimDirection != null)
            {
                StopCoroutine(swimProcess);
                swimDirection = new(0, 0);
            }
        }
        public void PerformSwim(Vector2 movementAxis)
        {
            if (!playerStamina.CanDash()) return;
            Assert.AreNotEqual(movementAxis.magnitude, 0);
            swimDirection = movementAxis;
            if (swimProcess != null) StopCoroutine(swimProcess);
            swimProcess = PerformSwimMovement(swimDirection, extraSwimSpeed, swimRamptownTime);
            StartCoroutine(swimProcess);
            onSwimmingAction?.Invoke();
        }

        private IEnumerator PerformSwimMovement(Vector2 movementAxis, float swimSpeed, float rampDownTime)
        {
            float swimTimer = rampDownTime;
            while (swimTimer >= 0)
            {
                currentSwimSpeed = Mathf.Lerp(movementSpeed, extraSwimSpeed, swimTimer / rampDownTime);
                swimTimer -= Time.deltaTime;
                yield return null;
            }
            swimDirection = new(0, 0);
            currentSwimSpeed = 0.0f;
            modifiedSwimDirection = new(0, 0);
            Debug.Log("End Swim");
        }
        private Vector2 modifiedSwimDirection = new(0, 0);
        /// <summary>
        /// Should be run in FixedDeltaTime. 
        /// </summary>
        /// <param name="movementAxis"></param>
        public void PerformStrafe(Vector2 movementAxis)
        {
            Vector2 currentMovementAxis;
            float currSpeed = 0;
            Vector2 currSwimDirection = modifiedSwimDirection.magnitude == 0 ? swimDirection : modifiedSwimDirection;
            //If there's already a swim direction, and this is equal to the movement axis, lock it in!
            if (currSwimDirection.magnitude != 0 && (currSwimDirection == movementAxis || movementAxis.magnitude == 0))
            {
                currentMovementAxis = currSwimDirection;
                currSpeed = currentSwimSpeed;
            }
            else if (movementAxis.magnitude == 0)
            {
                currentMovementAxis = new Vector2(Mathf.Lerp(0, lastMovementAxis.x, speedToStopCountdown / speedToStop), Mathf.Lerp(0, lastMovementAxis.y, speedToStopCountdown / speedToStop));
                speedToStopCountdown -= Time.fixedDeltaTime;
            }
            else
            {
                lastMovementAxis = movementAxis;
                currentMovementAxis = lastMovementAxis;
                speedToStopCountdown = speedToStop;
            }
            if (currSpeed == 0)
            {
                if (currentSwimSpeed == 0)
                {
                    currSpeed = movementSpeed;
                }
                else
                {
                    Vector2 newDirection = new((currentSwimSpeed * swimDirection.x) + (swimSpeedStirStrength * movementAxis.x), (currentSwimSpeed * swimDirection.y) + (swimSpeedStirStrength * movementAxis.y));
                    newDirection.Normalize();
                    currentMovementAxis = newDirection;
                    modifiedSwimDirection = newDirection;
                    currSpeed = currentSwimSpeed;
                }
            }
            rb.MovePosition(rb.position + currSpeed * Time.fixedDeltaTime * currentMovementAxis);
            Rotate(currentMovementAxis);
        }
        public void Rotate(Vector2 m_Input)
        {
            if (m_Input.magnitude > 0.1f)
            {
                localrotationSeekTimer = 0;
                startingRotation = targetTransformRotation.rotation;
                // Calculate angle from input
                float angle = Mathf.Atan2(m_Input.x, m_Input.y) * Mathf.Rad2Deg;

                // Snap to 45-degree increments (0, 45, 90, 135, 180, 225, 270, 315)
                _roundedAngle = Mathf.Round(angle / 45f) * 45f;

                // Apply rotation to the target transform (usually the visual model)
                Quaternion targetRotation = Quaternion.Euler(0f, 0f, 360 - _roundedAngle);
                endingRotation = targetRotation;
            }
        }
    }
}
