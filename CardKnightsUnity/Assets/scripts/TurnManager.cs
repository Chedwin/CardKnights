using System.Collections.Generic;
using System;
using UnityEngine;


public sealed class TurnManager : MonoBehaviour
{
    public bool switchState = false;
    public float gameTimer;
    public int seconds = 0;

    public FiniteStateMachine<TurnManager> StateMachine { get; set; }

    Queue<State<TurnManager>> turnQueue = new Queue<State<TurnManager>>();

    void CreateTurnQueue()
    {
        PlayerTurn plyTurn = new PlayerTurn("Player");
        EnemyTurn emTurn1 = new EnemyTurn("Orange box");
        EnemyTurn emTurn2 = new EnemyTurn("Blue box");
        EnemyTurn emTurn3 = new EnemyTurn("Purple box");

        turnQueue.Enqueue(plyTurn);
        turnQueue.Enqueue(emTurn1);
        turnQueue.Enqueue(emTurn2);
        turnQueue.Enqueue(emTurn3);
    }

    public void AddTurn(ref State<TurnManager> _turn)
    {
        //turnQueue.
    }

    public void GetTurnQueue()
    {
        Debug.Log("Turn Queue:");

        int i = 1;
        foreach (State<TurnManager> st in turnQueue) {
            Debug.Log(i++ + ": " + st.stateName);
        }
    }

    private void Start()
    {
        StateMachine = new FiniteStateMachine<TurnManager>(this);
        CreateTurnQueue();
        StateMachine.ChangeState(turnQueue.Dequeue());
        gameTimer = Time.time;
    }

    public void AdvanceTurn()
    {
        StateMachine.ChangeState(turnQueue.Dequeue());

        if (IsTurnQueueEmpty()) {
            CreateTurnQueue();
            Debug.Log("Created new turn queue!");
        }
    }

    bool IsTurnQueueEmpty()
    {
        return (turnQueue.Count == 0);
    }
    public State<TurnManager> NextTurn()
    {
        if (IsTurnQueueEmpty())
            CreateTurnQueue();

        return turnQueue.Peek();
    }

    public string WhoHasNextTurn()
    {
        return NextTurn().stateName;
    }
    
    


    // Update current turn via StateMachine
    private void Update()
    {
        StateMachine.UpdateState();
    }

} // end class TurnManager