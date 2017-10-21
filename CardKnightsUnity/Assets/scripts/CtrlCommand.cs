using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerCommand
{
    #region CtrlCommand Abstract Class
    public abstract class CtrlCommand
    {
        public CtrlCommand() { }

        public void Execute() {
            CommandFunc();
        }

        public delegate void DelegateFunction();

        public DelegateFunction CommandFunc { get; set; }

        public string inputKey; // get string key from CKInputManager
    }
    #endregion


} // end namespace PlayerCommand