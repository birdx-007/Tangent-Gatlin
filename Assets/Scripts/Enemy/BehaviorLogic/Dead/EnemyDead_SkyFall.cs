using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Dead_SkyFall",menuName = "EnemyBehaviorLogic/Dead/SkyFall")]
public class EnemyDead_SkyFall : EnemyDeadSOBase
{
    [SerializeField] private float fallGravity = 8f;
    [SerializeField] private float fallTime = 12f;
    public override void Initialize(EnemyStateController es)
    {
        base.Initialize(es);
    }

    public override void OnEnterLogic()
    {
        base.OnEnterLogic();
        Rigidbody2D enemyBody = enemy.GetComponent<Rigidbody2D>();
        Collider2D enemyCollider = enemy.GetComponent<Collider2D>();
        enemyBody.gravityScale = fallGravity;
        enemyCollider.isTrigger = true;
        enemy.StartCoroutine(TimerForFall());
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

    IEnumerator TimerForFall()
    {
        yield return new WaitForSeconds(fallTime);
        Destroy(enemy.gameObject);
    }
}
