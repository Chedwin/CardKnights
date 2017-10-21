using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTurn : State<TurnManager>
{
    float enemyTurnTime = 0.0f;
    int seconds = 0;

    public EnemyTurn(string _name)
    {
        stateName = _name;
    }

    public override void EnterState(TurnManager _owner)
    {
        Debug.Log(stateName + "'s turn");
        enemyTurnTime = 0.0f;
        seconds = 0;
    }

    public override void ExitState(TurnManager _owner)
    {

    }

    public override void UpdateState(TurnManager _owner)
    {
        enemyTurnTime += Time.deltaTime;

        if (enemyTurnTime >= (seconds + 1))
        {
            seconds++;
            Debug.Log(seconds);
        }

        if (enemyTurnTime >= 5.0f)
            _owner.AdvanceTurn();
    }

} // end class EnemyTurn