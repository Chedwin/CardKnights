using DequeUtility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSystem : MonoBehaviour
{
    public TurnManager turnManager;
    public static BattleSystem _instance;

    public static BattleSystem Instance
    {
        get
        {
            if (_instance == null)
                _instance = new BattleSystem();
            return _instance;
        }
    }


    void InitBattle()
    {
        turnManager = new TurnManager();
        
    }


    void Start()
    {
        InitBattle();
    }

    void Update()
    {
        turnManager.UpdateTurnManager();
    }
    
} // end class BattleSystem
