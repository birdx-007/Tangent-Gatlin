using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class EnemyStateController : MonoBehaviour
{
    public EnemyStateMachine StateMachine;
    public EnemyAnyState AnyState;
    public EnemyIdleState IdleState;
    public EnemyChaseState ChaseState;
    public EnemyAttackState AttackState;
    public EnemyDeadState DeadState;

    [SerializeField] private EnemyAnySO enemyAny;
    [SerializeField] private EnemyIdleSOBase enemyIdleBase;
    [SerializeField] private EnemyChaseSOBase enemyChaseBase;
    [SerializeField] private EnemyAttackSOBase enemyAttackBase;
    [SerializeField] private EnemyDeadSOBase enemyDeadBase;
    [NonSerialized] public EnemyAnySO enemyAnyInstance;
    [NonSerialized] public EnemyIdleSOBase enemyIdleBaseInstance;
    [NonSerialized] public EnemyChaseSOBase enemyChaseBaseInstance;
    [NonSerialized] public EnemyAttackSOBase enemyAttackBaseInstance;
    [NonSerialized] public EnemyDeadSOBase enemyDeadBaseInstance;

    void Awake()
    {
        enemyAnyInstance = Instantiate(enemyAny);
        enemyIdleBaseInstance = Instantiate(enemyIdleBase);
        enemyChaseBaseInstance = Instantiate(enemyChaseBase);
        enemyAttackBaseInstance = Instantiate(enemyAttackBase);
        enemyDeadBaseInstance = Instantiate(enemyDeadBase);
        
        StateMachine = new EnemyStateMachine();
        AnyState = new EnemyAnyState(this, StateMachine);
        IdleState = new EnemyIdleState(this, StateMachine);
        ChaseState = new EnemyChaseState(this, StateMachine);
        AttackState = new EnemyAttackState(this, StateMachine);
        DeadState = new EnemyDeadState(this, StateMachine);
    }

    void Start()
    {
        enemyAnyInstance.Initialize(this);
        enemyIdleBaseInstance.Initialize(this);
        enemyChaseBaseInstance.Initialize(this);
        enemyAttackBaseInstance.Initialize(this);
        enemyDeadBaseInstance.Initialize(this);
        
        StateMachine.Initialize(IdleState,AnyState);
    }

    private void Update()
    {
        StateMachine.EnemyAnyState.OnStateUpdate();
        StateMachine.EnemyCurrentState.OnStateUpdate();
    }

    private void FixedUpdate()
    {
        StateMachine.EnemyAnyState.OnStateFixedUpdate();
        StateMachine.EnemyCurrentState.OnStateFixedUpdate();
    }
}
