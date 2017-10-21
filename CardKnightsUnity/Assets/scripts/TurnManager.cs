using CKInterface;

using System;
using System.Collections.Generic;
using DequeUtility;
using UnityEngine;


public sealed class TurnManager
{
    public FiniteStateMachine<TurnManager> StateMachine { get; set; }

    Queue<IState<TurnManager>> turnQueue = new Queue<IState<TurnManager>>();


    Deque<IState<TurnManager>> turnDequeue = new Deque<IState<TurnManager>>();

    public TurnManager()
    {
        StateMachine = new FiniteStateMachine<TurnManager>(this);
    }

    //void ResetTurnQueue()
    //{
    //    PlayerTurn plyTurn = new PlayerTurn("Player");
    //    EnemyTurn emTurn1 = new EnemyTurn("Orange box");
    //    EnemyTurn emTurn2 = new EnemyTurn("Blue box");
    //    EnemyTurn emTurn3 = new EnemyTurn("Purple box");

    //    turnQueue.Enqueue(plyTurn);
    //    turnQueue.Enqueue(emTurn1);
    //    turnQueue.Enqueue(emTurn2);
    //    turnQueue.Enqueue(emTurn3);
    //}


    public void EnqueueTurn(ref IState<TurnManager> _state)
    {
        turnDequeue.AddBack(_state);
    }

    public void AddTurnToFront(ref IState<TurnManager> _state)
    {
        turnDequeue.AddFront(_state);  
    }




    //private void Start()
    //{
    //    StateMachine = new FiniteStateMachine<TurnManager>(this);


    //    turnQueue.Enqueue(new StartBattleTurn());
    //    ResetTurnQueue();
    //    StateMachine.ChangeState(turnQueue.Dequeue());
    //}

    //public void AdvanceTurn()
    //{

    //    StateMachine.ChangeState(turnQueue.Dequeue());
    //}

    //bool IsTurnQueueEmpty()
    //{
    //    return (turnQueue.Count == 0);
    //}
    //public State<TurnManager> NextTurn()
    //{
    //    if (IsTurnQueueEmpty())
    //        ResetTurnQueue();

    //    return turnQueue.Peek();
    //}

    //public string WhoHasNextTurn()
    //{
    //    return NextTurn().stateName;
    //}

    public void UpdateTurnManager()
    {
        StateMachine.UpdateState();        
    }

} // end class TurnManager