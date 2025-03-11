using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace NotWhiskey.newStateMachine
{
    public class PlayerInputHandler : MonoBehaviour
    {
        public float xInput { get; private set; }
        public float yInput { get; private set; }
        public bool JumpInput { get; private set; }
        public bool JumpInputStop { get; private set; }

        [SerializeField] 
        private float inputHoldTime = 0.2f;
        private float jumpInputStartTime;

        public Vector2 PlayerMovementInput { get; private set; }

        private void Update()
        {
            CheckJumpInputHoldTime();
        }

        public void OnMoveInput(InputAction.CallbackContext context)
        {
            PlayerMovementInput = context.ReadValue<Vector2>();
    
            xInput = PlayerMovementInput.x;
            yInput = PlayerMovementInput.y;
        }

        public void OnJumpInput(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                jumpInputStartTime = Time.time;
                JumpInput = true;
                JumpInputStop = false;
            }

            if (context.canceled)
            {
                JumpInputStop = true;
            }
        }

        public void UseJumpInput() => JumpInput = false;

        private void CheckJumpInputHoldTime()
        {
            if (Time.time >= jumpInputStartTime + inputHoldTime)
            {
                JumpInput = false;
            }
        }
    }
}

