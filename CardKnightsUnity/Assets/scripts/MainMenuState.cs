﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using CKInterface;

public class MainMenuState : IState<CKGameManager>
{
    public MainMenuState()
    {
        StateName = "MainMenu";
    }

    public string StateName { get; set; }

    public void EnterState(CKGameManager _owner)
    {

    }

    public void ExitState(CKGameManager _owner)
    {

    }

    public void UpdateState(CKGameManager _owner)
    {

    }

} // end class MainMenuState