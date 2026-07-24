using System;
using Prototype_jpxls;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Prototype_jpxls
{
    public class PlayerInput : MonoBehaviour
    {
        PlayerInputActions inputActions;
        [SerializeField]
        private PlayerMovement playerMovement;
        [SerializeField]
        private float swimInputCooldown = 0.3f;

        private PlayerInputActions.PlayerActions playerActions;
        private InputAction strafeInput;
        private InputAction swimInput;

        private Vector2 movementAxis;
        private float currSwimInputCD = 0.0f;



        void Awake()
        {
            inputActions ??= new PlayerInputActions();
            playerActions = inputActions.Player;
            strafeInput = playerActions.Strafe;
            swimInput = playerActions.Swim;

        }
        void OnEnable()
        {
            playerActions.Enable();

            swimInput.started += ExecuteSwimAction;
        }

        void OnDisable()
        {
            swimInput.started -= ExecuteSwimAction;
            playerActions.Disable();
        }
        void Update()
        {
            movementAxis = strafeInput.ReadValue<Vector2>();
            if (currSwimInputCD >= 0)
            {
                currSwimInputCD -= Time.deltaTime;
            }
        }
        void FixedUpdate()
        {



            playerMovement.PerformStrafe(movementAxis);

        }

        private void ExecuteSwimAction(InputAction.CallbackContext context)
        {
            if (movementAxis.magnitude == 0 || currSwimInputCD >= 0) return;
            currSwimInputCD = swimInputCooldown;
            playerMovement.PerformSwim(movementAxis);
        }
    }
}
