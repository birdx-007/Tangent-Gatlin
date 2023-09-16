using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateSOBase : ScriptableObject
{
    protected EnemyStateController enemyStateController;
    protected Enemy enemy;
    protected Transform enemyTransform;
    protected static Transform playerTransform;

    public virtual void Initialize(EnemyStateController stateController)
    {
        this.enemyStateController = stateController;
        this.enemy = stateController.GetComponent<Enemy>();
        this.enemyTransform = stateController.GetComponent<Transform>();
        if (!playerTransform)
        {
             playerTransform = enemy.playerTransform;
        }
    }
    public virtual void OnEnterLogic(){}
    public virtual void OnExitLogic(){ResetValues();}

    public virtual void OnUpdateLogic(){}
    public virtual void OnFixedUpdateLogic(){}
    public virtual void ResetValues(){}
}
