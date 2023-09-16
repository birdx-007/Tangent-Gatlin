using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Attack_StraightShoot",menuName = "EnemyBehaviorLogic/Attack/StraightShoot")]
public class EnemyAttack_StraightShoot : EnemyAttackSOBase
{
    [SerializeField] private float timeBetweenShoots = 0.5f;
    [SerializeField] private GameObject bullet;
    [SerializeField] private float stopShootTimeAfterOutOfAttackRange = 0.25f;
    private float _shootTimer;
    private float _stopShootTimer;
    public override void Initialize(EnemyStateController stateController)
    {
        base.Initialize(stateController);
    }

    public override void OnEnterLogic()
    {
        base.OnEnterLogic();
        _shootTimer = timeBetweenShoots;
        _stopShootTimer = 0;
        enemy.bulletPrefab = bullet;
    }

    public override void OnExitLogic()
    {
        base.OnExitLogic();
    }

    public override void OnUpdateLogic()
    {
        base.OnUpdateLogic();
        if (enemy.isPlayerInAttackRange)
        {
            _stopShootTimer = 0;
            _shootTimer += Time.deltaTime;
            if (_shootTimer >= timeBetweenShoots)
            {
                _shootTimer = 0;
                enemy.Shoot();
            }
        }
        else
        {
            _stopShootTimer += Time.deltaTime;
            if (_stopShootTimer >= stopShootTimeAfterOutOfAttackRange)
            {
                if (enemy.isProvoked)
                {
                    enemyStateController.StateMachine.ChangeState(enemyStateController.ChaseState);
                }
                else
                {
                    enemyStateController.StateMachine.ChangeState(enemyStateController.IdleState);
                }
            }
        }
    }

    public override void OnFixedUpdateLogic()
    {
        base.OnFixedUpdateLogic();
    }

    public override void ResetValues()
    {
        base.ResetValues();
        _shootTimer = timeBetweenShoots;
        _stopShootTimer = 0;
    }
}
