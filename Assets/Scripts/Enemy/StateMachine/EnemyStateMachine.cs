using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class EnemyStateMachine : MonoBehaviour
{
    public EnemyState enemyCurrentState;

    [SerializeField] private EnemyState startState;
    [SerializeField] private SerializableDictionary<EnemyState, EnemyStateSOBase> enemyStateDict;
    private Dictionary<EnemyState, EnemyStateSOBase> enemyStateInstanceDict;
    [SerializeField] private EnemyAnySO enemyAnyStateLogic;
    private EnemyAnySO enemyAnyStateLogicInstance;

    private EnemyStateSOBase GetStateLogicInstance(EnemyState state)
    {
        return enemyStateInstanceDict[state];
    }

    void Awake()
    {
        enemyStateInstanceDict = new Dictionary<EnemyState, EnemyStateSOBase>();
        foreach(var pair in enemyStateDict)
        {
            enemyStateInstanceDict.Add(pair.Key, Instantiate(pair.Value));
        }
        enemyAnyStateLogicInstance = Instantiate(enemyAnyStateLogic);
    }

    void Start()
    {
        foreach(var stateInstance in enemyStateInstanceDict.Values)
        {
            stateInstance.Initialize(this);
        }
        enemyAnyStateLogicInstance.Initialize(this);

        enemyCurrentState = startState;
        GetStateLogicInstance(enemyCurrentState).OnEnterLogic();
        enemyAnyStateLogicInstance.OnEnterLogic();
    }

    private void Update()
    {
        enemyAnyStateLogicInstance.OnUpdateLogic();
        GetStateLogicInstance(enemyCurrentState).OnUpdateLogic();
    }

    private void FixedUpdate()
    {
        enemyAnyStateLogicInstance.OnFixedUpdateLogic();
        GetStateLogicInstance(enemyCurrentState).OnFixedUpdateLogic();
    }

    public void ChangeState(EnemyState newState)
    {
        GetStateLogicInstance(enemyCurrentState).OnExitLogic();
        enemyCurrentState = newState;
        GetStateLogicInstance(enemyCurrentState).OnEnterLogic();
    }
}
