using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine
{
    public EnemyState EnemyCurrentState;
    public EnemyState EnemyAnyState;

    public void Initialize(EnemyState startState,EnemyState anyState)
    {
        EnemyCurrentState = startState;
        EnemyCurrentState.EnterState();
        EnemyAnyState = anyState;
        EnemyAnyState.EnterState();
    }

    public void ChangeState(EnemyState newState)
    {
        EnemyCurrentState.ExitState();
        EnemyCurrentState = newState;
        EnemyCurrentState.EnterState();
    }
}
