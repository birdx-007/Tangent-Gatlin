using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackState : EnemyState
{
    public EnemyAttackState(EnemyStateController controller, EnemyStateMachine stateMachine) : base(controller, stateMachine)
    {
    }
    public override void EnterState()
    {
        base.EnterState();
        enemyStateController.enemyAttackBaseInstance.OnEnterLogic();
    }

    public override void ExitState()
    {
        base.ExitState();
        enemyStateController.enemyAttackBaseInstance.OnExitLogic();
    }

    public override void OnStateUpdate()
    {
        base.OnStateUpdate();
        enemyStateController.enemyAttackBaseInstance.OnUpdateLogic();
    }

    public override void OnStateFixedUpdate()
    {
        base.OnStateFixedUpdate();
        enemyStateController.enemyAttackBaseInstance.OnFixedUpdateLogic();
    }
}
