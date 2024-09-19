using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleSOBase : EnemyStateSOBase
{
    public override void OnUpdateLogic()
    {
        base.OnUpdateLogic();
        if (enemy.isProvoked)
        {
            stateMachine.ChangeState(EnemyState.Chase);
        }
    }
}
