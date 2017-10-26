using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using CKInterface;

public class MainMenuState : IState<CKGameManager>
{
    public string StateName { get; set; }
    CKInputManager plyCtrl;

    public MainMenuState()
    {
        StateName = "MainMenu";
    }


    public void EnterState(CKGameManager _owner)
    {
        Debug.Log("Welcome to Card Knights!");
        plyCtrl = CKInputManager.Instance;
        plyCtrl.SetPS4BtnCtrl(PS4Controller.PS4_CTRL_MAP.CROSS, CrossButton);
    }

    float CrossButton()
    {
        CKGameManager.Instance.AdvanceState(CKGameManager.GAME_STATE_TYPE.WORLD_MAP);
        return 0.0f;
    }

    float CircleButton()
    {
        CKGameManager.Instance.ReturnState();
        return 0.0f;
    }

    public void ExitState(CKGameManager _owner)
    {
        Debug.Log("Loading Game");
        plyCtrl.PS4Ctrl.ResetCtrlCommands();
    }

    public void UpdateState(CKGameManager _owner)
    {

    }

} // end class MainMenuState