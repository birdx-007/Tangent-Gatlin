using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Chase_SkyChase",menuName = "EnemyBehaviorLogic/Chase/SkyChase")]
public class EnemyChase_SkyChase : EnemyChaseSOBase
{
    [SerializeField] private float skyChasingCenterY = 3f;
    [SerializeField] private float skyChasingYRandomRange = 1f;
    [SerializeField] private float skyChasingSpeed = 4.5f;
    private float skyChasingTargetY;
    public override void Initialize(EnemyStateController es)
    {
        base.Initialize(es);
    }

    public override void OnEnterLogic()
    {
        base.OnEnterLogic();
        skyChasingTargetY = skyChasingCenterY + Random.Range(-skyChasingYRandomRange / 2, skyChasingYRandomRange / 2);
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
        Vector2 curPosition = enemyTransform.position;
        var playerPosition = playerTransform.position;
        Vector2 targetVector = new Vector2(playerPosition.x - curPosition.x,
             Mathf.Max(playerPosition.y,skyChasingTargetY) - curPosition.y);
        float speedRatio = 1f;
        if (Mathf.Abs(targetVector.x) > enemy.slowChaseRangeRadius) //fastChase
        {
            speedRatio = Mathf.Pow(Mathf.Abs(targetVector.x) / enemy.slowChaseRangeRadius, 1.6f);
        }
        enemy.Move(targetVector, skyChasingSpeed * speedRatio);
    }

    public override void ResetValues()
    {
        base.ResetValues();
    }
}
