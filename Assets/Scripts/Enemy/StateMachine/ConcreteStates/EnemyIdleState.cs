using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState : EnemyState
{
    public EnemyIdleState(EnemyStateController controller, EnemyStateMachine stateMachine) : base(controller, stateMachine)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        enemyStateController.enemyIdleBaseInstance.OnEnterLogic();
    }

    public override void ExitState()
    {
        base.ExitState();
        enemyStateController.enemyIdleBaseInstance.OnExitLogic();
    }

    public override void OnStateUpdate()
    {
        base.OnStateUpdate();
        enemyStateController.enemyIdleBaseInstance.OnUpdateLogic();
    }

    public override void OnStateFixedUpdate()
    {
        base.OnStateFixedUpdate();
        enemyStateController.enemyIdleBaseInstance.OnFixedUpdateLogic();
    }
}
