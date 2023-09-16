using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnyState : EnemyState
{
    public EnemyAnyState(EnemyStateController controller, EnemyStateMachine stateMachine) : base(controller, stateMachine)
    {
    }
    public override void EnterState()
    {
        base.EnterState();
        enemyStateController.enemyAnyInstance.OnEnterLogic();
    }

    public override void ExitState()
    {
        base.ExitState();
        enemyStateController.enemyAnyInstance.OnExitLogic();
    }

    public override void OnStateUpdate()
    {
        base.OnStateUpdate();
        enemyStateController.enemyAnyInstance.OnUpdateLogic();
    }

    public override void OnStateFixedUpdate()
    {
        base.OnStateFixedUpdate();
        enemyStateController.enemyAnyInstance.OnFixedUpdateLogic();
    }
}
