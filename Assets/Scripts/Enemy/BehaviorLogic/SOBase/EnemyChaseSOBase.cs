using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChaseSOBase : EnemyStateSOBase
{
    public override void OnUpdateLogic()
    {
        base.OnUpdateLogic();
        if (enemy.isPlayerInAttackRange)
        {
            enemyStateController.StateMachine.ChangeState(enemyStateController.AttackState);
        }
    }
}
