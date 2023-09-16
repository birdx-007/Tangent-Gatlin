using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Idle_StayStill",menuName = "EnemyBehaviorLogic/Idle/StayStill")]
public class EnemyIdle_StayStill : EnemyIdleSOBase
{
    public override void Initialize(EnemyStateController es)
    {
        base.Initialize(es);
    }

    public override void OnEnterLogic()
    {
        base.OnEnterLogic();
    }

    public override void OnExitLogic()
    {
        base.OnExitLogic();
    }

    public override void OnUpdateLogic()
    {
        base.OnUpdateLogic();
    }

    public override void OnFixedUpdateLogic()
    {
        base.OnFixedUpdateLogic();
    }

    public override void ResetValues()
    {
        base.ResetValues();
    }
}
