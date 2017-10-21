using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
//using UnityEngine.
using UnityEngine.SceneManagement;

using CKInterface;
using CKFactory;



public class CKGameManager : Singleton<CKGameManager>
{                                                                 
    public enum GAME_STATE_TYPE : uint
    {
        MAIN_MENU   = 0,
        WORLD_MAP   = 1,
        LOCAL_MAP   = 2,
        BATTLE      = 3,
        INGAME_MENU = 4
    }

    // Visible in inspector
    // However, modifying it in the inspector does not change the actual game state
    [SerializeField]
    public GAME_STATE_TYPE currentGameStateLabel;

    protected FiniteStateMachine<CKGameManager> gameStateMachine                 = new FiniteStateMachine<CKGameManager>(Instance);
    protected Dictionary<GAME_STATE_TYPE, IState<CKGameManager>> gameStateMap    = new Dictionary<GAME_STATE_TYPE, IState<CKGameManager>>();

    

    void CreateGameStates()
    {
        GameStateFactory gsf = new GameStateFactory();

        // Create only a SINGLE STATE per state type
        foreach (GAME_STATE_TYPE gs in Enum.GetValues(typeof(GAME_STATE_TYPE)))
        {
            IState<CKGameManager> state = gsf.CreateInstance(gs);
            gameStateMap.Add(gs, state);
            gameStateMachine.AddState(ref state);
        }
    }


    void ChangeScene(GAME_STATE_TYPE targetState)
    {
        if (gameStateMap.Count == 0 || gameStateMachine.GetStateCount() == 0)
            CreateGameStates();
        

        gameStateMachine.ChangeState(gameStateMap[targetState]);
        SceneManager.LoadScene((int)targetState);
    }

    protected IState<CKGameManager> GetGameState(GAME_STATE_TYPE _type)
    {
        return gameStateMap[_type];
    }

    void SetGameState(GAME_STATE_TYPE _st)
    {
         
        
    }










    public override void Awake()
    {
        base.Awake();
        CreateGameStates();
    }

    public void Start()
    {
           
    }

    private void Update()
    {
        gameStateMachine.UpdateState();
    }

} // end class CKGameManager
