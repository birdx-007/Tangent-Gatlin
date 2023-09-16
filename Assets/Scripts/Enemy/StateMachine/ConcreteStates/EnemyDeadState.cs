using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeadState : EnemyState
{
    public EnemyDeadState(EnemyStateController controller, EnemyStateMachine stateMachine) : base(controller, stateMachine)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        enemyStateController.enemyDeadBaseInstance.OnEnterLogic();
    }

    public override void ExitState()
    {
        base.ExitState();
        enemyStateController.enemyDeadBaseInstance.OnExitLogic();
    }

    public override void OnStateUpdate()
    {
        base.OnStateUpdate();
        enemyStateController.enemyDeadBaseInstance.OnUpdateLogic();
    }

    public override void OnStateFixedUpdate()
    {
        base.OnStateFixedUpdate();
        enemyStateController.enemyDeadBaseInstance.OnFixedUpdateLogic();
    }
}
