using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GamepadController
{
    public abstract void ProcessCtrlInput();
    public abstract void ResetCtrlCommands();
}