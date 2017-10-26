using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using PlayerCommand;

public class CKInputManager : Singleton<CKInputManager>
{
    const int PS4MaxButtonCount = 20;

    // Get the string name for each of the PS4 input keys that correspond to the Unity Input settings
    [SerializeField]
    public List<PS4InputKey> PS4ButtonList = new List<PS4InputKey>(PS4MaxButtonCount);

    // PS4 Controller Object
    public PS4Controller PS4Ctrl { get; set; }

    
    // Initialize the PS4 controller
    // Use the PS4ButtonList to assign each of the button's individual input string keys
    public void SetupPS4Controller()
    {
        PS4Ctrl = new PS4Controller(ref PS4ButtonList);
    }

    // Set the Delegate function of each individual PS4 buttons
    public void SetPS4BtnCtrl(PS4Controller.PS4_CTRL_MAP _btn, CtrlCommand.DelegateFunction _btnCtrl)
    {
        PS4Ctrl.PS4Map[_btn].CommandFunc = _btnCtrl; 
    }

    public void ResetPS4BtnCtrls()
    {
        PS4Ctrl.ResetCtrlCommands();   
    }

    // Get the Input string key of the PS4 controller buttons
    public string GetCtrlKeyName(PS4Controller.PS4_CTRL_MAP _btn)
    {
        return PS4Ctrl.PS4Map[_btn].InputKey;
    }

    // Get the specific PS4 button
    // Should rarely get called
    public PS4Command GetInputKey(PS4Controller.PS4_CTRL_MAP _btn)
    {
        return PS4Ctrl.GetInputKey(_btn);
    }

    public bool IsPS4BtnDown(PS4Controller.PS4_CTRL_MAP _btn)
    {
        return PS4Ctrl.PS4Map[_btn].IsKeyDown;
    }

    void ProcessInput()
    {
        PS4Ctrl.ProcessCtrlInput();           
    }


#region MONOBEHAVIOUR functions

    public override void Awake()
    {
        base.Awake();
        SetupPS4Controller();
    }

    private void Update()
    {
        ProcessInput();
    }

#endregion

} // end class InputManager