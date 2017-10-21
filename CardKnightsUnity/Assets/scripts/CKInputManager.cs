using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CKInputManager : MonoBehaviour
{
    public enum PS4_CTRL {
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

    [SerializeField]
    public List<InputElement> controllerMap = new List<InputElement>();


    public string GetCtrlKeyName(PS4_CTRL _btn) {
        string btnKey = "";
        foreach (InputElement ie in controllerMap) {
            if (_btn == ie._ctrl)
                btnKey = ie.inputKey;
        }
        return btnKey;
    }


    public static CKInputManager Instance
    {
        get;
        private set;
    }

    void Awake()
    {
        // First we check if there are any other instances conflicting
        if (Instance != null && Instance != this)
        {
            // If that is the case, we destroy other instances            
            Destroy(gameObject);
        }

        // Here we save our singleton instance
        Instance = this;
    }

} // end class InputManager

[System.Serializable]
public struct InputElement
{
    public CKInputManager.PS4_CTRL _ctrl;
    public string inputKey;
}