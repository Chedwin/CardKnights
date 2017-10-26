using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using PlayerCommand; 

[Serializable]
public struct PS4InputKey
{
    public PS4Controller.PS4_CTRL_MAP _ctrl;
    public string inputKey;
}

public class PS4Controller : GamepadController
{
    public enum PS4_CTRL_MAP : int
    {
        SQUARE = 0,
        CROSS = 1,
        CIRCLE = 2,
        TRIANGLE = 3,
        L1 = 4,
        R1 = 5,
        L2 = 6,
        R2 = 7,
        SHARE = 8,
        OPTIONS = 9,
        L3 = 10,
        R3 = 11,
        PS_BTN = 12,
        TRACKPAD_BTN = 13,
        LS_X = 14,
        LS_Y = 15,
        RS_X = 16,
        RS_Y = 17,
        DPAD_X = 18,
        DPAD_Y = 19
    }

    public Dictionary<PS4_CTRL_MAP, PS4Command> PS4Map { get; protected set; }

    public PS4Controller(ref List<PS4InputKey> _keyList)
    {
        PS4Map = new Dictionary<PS4_CTRL_MAP, PS4Command>();
        foreach (PS4_CTRL_MAP _psKey in Enum.GetValues(typeof(PS4_CTRL_MAP)))
        {
            PS4Map.Add(_psKey, new PS4Command() { InputKey = _keyList[(int)_psKey].inputKey });
        }
    }

    public PS4Command GetInputKey(PS4_CTRL_MAP _key)
    {
        return PS4Map[_key];
    }

    // Reset all assigned delegate functions for each button
    public override void ResetCtrlCommands()
    {
        foreach (KeyValuePair<PS4_CTRL_MAP, PS4Command> entry in PS4Map)
        {
            entry.Value.CommandFunc = null;
        }
    }

    public bool IsBtnDown(PS4_CTRL_MAP _btn)
    {
        return PS4Map[_btn].IsKeyDown;
    }

    // Check if any of the registered buttons have been pressed
    // If so, execute their delegate function (if assigned)
    public override void ProcessCtrlInput()
    {
        foreach (KeyValuePair<PS4_CTRL_MAP, PS4Command> entry in PS4Map)
        {
            // Make sure that the delegate function is not NULL
            if (Input.GetButtonUp(entry.Value.InputKey)) 
                if (entry.Value.CommandFunc != null)
                    entry.Value.Execute();

            if (Input.GetButton(entry.Value.InputKey))
                entry.Value.IsKeyDown = true;
            else
                entry.Value.IsKeyDown = false;
        }
    }

} // end class PlayerController



public class PS4Command : CtrlCommand
{
    // PS4 Command buttons
}
