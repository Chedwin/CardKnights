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
        COMBAT      = 3,
        INGAME_MENU = 4
    }

    // Visible in inspector
    // However, modifying it in the inspector does not change the actual game state
    [SerializeField]
    public GAME_STATE_TYPE currentGameStateLabel;


    // Dictionary container that just HOLDS our 5 essential game states
    protected Dictionary<GAME_STATE_TYPE, IState<CKGameManager>> gameStateMap = new Dictionary<GAME_STATE_TYPE, IState<CKGameManager>>();


    // The StateStack will keep track of the current game state
    // Allows for fast advancement to next states and quick return for pervious ones
    protected StateStack<CKGameManager> gameStateStack = new StateStack<CKGameManager>(Instance);


    void CreateGameStates()
    {
        GameStateFactory gsf = new GameStateFactory();

        // Create only a SINGLE STATE per state type
        foreach (GAME_STATE_TYPE gs in Enum.GetValues(typeof(GAME_STATE_TYPE)))
        {
            IState<CKGameManager> state = gsf.CreateGameState(gs);
            //stateGraph.AddNode(ref state);
            gameStateMap.Add(gs, state);
        }
    }

    // Get the specific game state from the Dictionary
    IState<CKGameManager> GetGameState(GAME_STATE_TYPE _gs)
    {
        return gameStateMap[_gs];
    }


    public void AdvanceState(GAME_STATE_TYPE _state)
    {
        IState<CKGameManager> st = GetGameState(_state);
        gameStateStack.AdvanceState(ref st);   
    }

    public void ReturnState()
    {
        gameStateStack.ReturnState();
    }











#region MONOBEHAVIOUR functions

    public override void Awake()
    {
        base.Awake();
        CreateGameStates();
    }

    public void Start()
    {
        IState<CKGameManager> main = gameStateMap[GAME_STATE_TYPE.MAIN_MENU];
        gameStateStack.AdvanceState(ref main);
    }

    private void Update()
    {
        gameStateStack.UpdateState();
    }

#endregion

} // end class CKGameManager
