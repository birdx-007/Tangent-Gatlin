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
            stateMachine.ChangeState(EnemyState.Dead);
        }
        else if (!enemy.isProvoked)
        {
            stateMachine.ChangeState(EnemyState.Idle);
        }
    }
}
