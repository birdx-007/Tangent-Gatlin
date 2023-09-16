using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Idle_RandomWander",menuName = "EnemyBehaviorLogic/Idle/RandomWander")]
public class EnemyIdle_RandomWander : EnemyIdleSOBase
{
    [SerializeField] private float randomMoveRange = 5f;
    [SerializeField] private float randomMoveSpeed = 4f;
    private Vector3 _targetPosition;
    private Vector3 _direction;

    public override void OnEnterLogic()
    {
        base.OnEnterLogic();
        _targetPosition = GetRandomPointInRange();
    }

    public override void OnUpdateLogic()
    {
        base.OnUpdateLogic();
        _direction = (_targetPosition - enemy.transform.position).normalized;
        enemy.Move(_direction,randomMoveSpeed);
        if ((enemyTransform.position - _targetPosition).magnitude <= 0.01f * randomMoveSpeed)
        {
            _targetPosition = GetRandomPointInRange();
        }
    }
    private Vector3 GetRandomPointInRange()
    {
        return enemy.transform.position + (Vector3)Random.insideUnitCircle * randomMoveRange;
    }
}
