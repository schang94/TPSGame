using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace RPG.Control
{ 
    public class InputHandler : MonoBehaviour
    {
        public bool IsMouse { get; private set; } = false;


        public void OnMove(InputAction.CallbackContext ctx)
        {
            if (ctx.performed)
            {
                IsMouse = true;
            }
            else if (ctx.canceled)
            {
                IsMouse = false;
            }
        }
    }
}
