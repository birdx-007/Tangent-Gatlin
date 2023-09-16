using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState
{
    protected EnemyStateController enemyStateController;
    protected Enemy enemy;
    protected EnemyStateMachine enemyStateMachine;

    public EnemyState(EnemyStateController controller, EnemyStateMachine stateMachine)
    {
        enemyStateController = controller;
        enemy = controller.GetComponent<Enemy>();
        enemyStateMachine = stateMachine;
    }

    public virtual void EnterState()
    {
        
    }

    public virtual void ExitState()
    {
        
    }

    public virtual void OnStateUpdate()
    {
        
    }

    public virtual void OnStateFixedUpdate()
    {
        
    }
}
