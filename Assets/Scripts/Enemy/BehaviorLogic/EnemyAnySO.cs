using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyAny",menuName = "EnemyBehaviorLogic/Any")]
public class EnemyAnySO : EnemyStateSOBase
{
    public override void OnUpdateLogic()
    {
        base.OnUpdateLogic();
        if (!enemy.isAlive)
        {
            enemyStateController.StateMachine.ChangeState(enemyStateController.DeadState);
        }
        else if (!enemy.isProvoked)
        {
            enemyStateController.StateMachine.ChangeState(enemyStateController.IdleState);
        }
    }
}
