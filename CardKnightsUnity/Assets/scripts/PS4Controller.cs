using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerCommand;

public sealed class PS4Controller : GamepadController
{
    public PS4Controller()
    {
        InitController();     
    }

    protected override void InitController()
    {
        squareBtn       = new SquarePS4Command();
        crossBtn        = new CrossPS4Command();
        triangleBtn     = new TrianglePS4Command();
        circleBtn       = new CirclePS4Command();

        squareBtn.inputKey      = CKInputManager.Instance.GetCtrlKeyName(CKInputManager.PS4_CTRL.SQUARE);
        crossBtn.inputKey       = CKInputManager.Instance.GetCtrlKeyName(CKInputManager.PS4_CTRL.CROSS);
        triangleBtn.inputKey    = CKInputManager.Instance.GetCtrlKeyName(CKInputManager.PS4_CTRL.TRIANGLE);
        circleBtn.inputKey      = CKInputManager.Instance.GetCtrlKeyName(CKInputManager.PS4_CTRL.CIRCLE);
    }

    public SquarePS4Command     squareBtn;
    public CrossPS4Command      crossBtn;
    public TrianglePS4Command   triangleBtn;
    public CirclePS4Command     circleBtn;

} // end class PlayerController


namespace PlayerCommand
{
#region PS4 Specific Button Command Classes
    public sealed class SquarePS4Command : CtrlCommand
    {
        // PS4 SQUARE button
    }
    public sealed class CrossPS4Command : CtrlCommand
    {
        // PS4 CROSS button
    }
    public sealed class TrianglePS4Command : CtrlCommand
    {
        // PS4 TRIANGLE button
    }
    public sealed class CirclePS4Command : CtrlCommand
    {
        // PS4 CIRCLE button
    }


    public sealed class L1PS4Command : CtrlCommand
    {
        // PS4 L1 button
    }
    public sealed class R1PS4Command : CtrlCommand
    {
        // PS4 R1 button
    }
    public sealed class L2PS4Command : CtrlCommand
    {
        // PS4 L2 button
    }
    public sealed class R2PS4Command : CtrlCommand
    {
        // PS4 R2 button
    }

#endregion

} // end namespace PlayerCommand