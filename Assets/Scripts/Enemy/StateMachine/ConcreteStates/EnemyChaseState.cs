using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChaseState : EnemyState
{
    public EnemyChaseState(EnemyStateController controller, EnemyStateMachine stateMachine) : base(controller, stateMachine)
    {
    }
    public override void EnterState()
    {
        base.EnterState();
        enemyStateController.enemyChaseBaseInstance.OnEnterLogic();
    }

    public override void ExitState()
    {
        base.ExitState();
        enemyStateController.enemyChaseBaseInstance.OnExitLogic();
    }

    public override void OnStateUpdate()
    {
        base.OnStateUpdate();
        enemyStateController.enemyChaseBaseInstance.OnUpdateLogic();
    }

    public override void OnStateFixedUpdate()
    {
        base.OnStateFixedUpdate();
        enemyStateController.enemyChaseBaseInstance.OnFixedUpdateLogic();
    }
}
