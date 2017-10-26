using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerCommand
{
    #region CtrlCommand Abstract Class
    public abstract class CtrlCommand
    {
        public CtrlCommand() { }

        public float Execute() {
            return CommandFunc();
        }

        public delegate float DelegateFunction();

        public DelegateFunction CommandFunc { get; set; }

        public string InputKey { get; set; } // get string key from CKInputManager

        public bool IsKeyDown = false;
    }
    #endregion


} // end namespace PlayerCommand