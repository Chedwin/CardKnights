using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTurn : State<TurnManager>
{
    public string xButton;  // cross
    public string cButton;  // circle
    public string tButton;  // triangle
    public string sButton;  // square

    public string dpadY;

    public bool finishTurn = false;

    public PlayerTurn(string _name)
    {
        stateName = _name;
    } 

    public override void EnterState(TurnManager _owner)
    {
        Debug.Log(stateName + "'s turn");
        xButton = CKInputManager.Instance.GetCtrlKeyName(CKInputManager.PS4_CTRL.CROSS);
        cButton = CKInputManager.Instance.GetCtrlKeyName(CKInputManager.PS4_CTRL.CIRCLE);
        tButton = CKInputManager.Instance.GetCtrlKeyName(CKInputManager.PS4_CTRL.TRIANGLE);
        sButton = CKInputManager.Instance.GetCtrlKeyName(CKInputManager.PS4_CTRL.SQUARE);

        dpadY = CKInputManager.Instance.GetCtrlKeyName(CKInputManager.PS4_CTRL.DPAD_Y);

        finishTurn = false;
    }

    public override void ExitState(TurnManager _owner)
    {
        
    }

    public override void UpdateState(TurnManager _owner)
    {
        float dpY = Input.GetAxis(dpadY);

        if (dpY > 0.1f)
            _owner.GetTurnQueue();
        else if (dpY < -0.1f)
            Debug.Log(_owner.WhoHasNextTurn());


        if (Input.GetButtonUp(xButton))
        {
            Debug.Log("ATTACK");
            finishTurn = true;
        }
        else if (Input.GetButtonUp(cButton))
        {
            Debug.Log("DEFEND");
            finishTurn = true;
        }
        else if (Input.GetButtonUp(tButton))
        {
            Debug.Log("MAGIC ATTACK");
            finishTurn = true;
        }
        else if (Input.GetButtonUp(sButton))
        {
            Debug.Log("USED ITEM");
            finishTurn = true;
        }


        if (finishTurn) 
            _owner.AdvanceTurn();
        
    }
    
}  // end class PlayerTurn
