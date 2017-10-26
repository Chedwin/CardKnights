using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using CKInterface;

public class TitleScreenTest : MonoBehaviour
{
    CKInputManager plyCtrl;
    IState<CKGameManager> main;
    IState<CKGameManager> world;


    void InitPlyCtrl()
    {
        plyCtrl = CKInputManager.Instance;
        plyCtrl.SetPS4BtnCtrl(PS4Controller.PS4_CTRL_MAP.CROSS, CrossButton);
        plyCtrl.SetPS4BtnCtrl(PS4Controller.PS4_CTRL_MAP.CIRCLE, CircleButton);
    }

    private void Start()
    {
        InitPlyCtrl();
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

    private void Update()
    {
        if (Input.GetButtonUp(plyCtrl.GetCtrlKeyName(PS4Controller.PS4_CTRL_MAP.CROSS))) 
            plyCtrl.GetInputKey(PS4Controller.PS4_CTRL_MAP.CROSS).Execute();
        else if (Input.GetButtonUp(plyCtrl.GetCtrlKeyName(PS4Controller.PS4_CTRL_MAP.CIRCLE)))
            plyCtrl.GetInputKey(PS4Controller.PS4_CTRL_MAP.CIRCLE).Execute();

        //Debug.()
    }


} // end class TitleScreenTest