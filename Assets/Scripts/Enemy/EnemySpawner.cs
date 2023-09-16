using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    public Enemy initEnemy; //开场敌人 不计入生成器
    public bool enableSpawn = false;
    public EnemyCategory enemyCategory;
    [SerializeField] private Transform leftSpawnPoint;
    [SerializeField] private Transform rightSpawnPoint;
    private int maxAliveEnemyNumber = 1;
    private int aliveEnemyNumber = 0;
    private List<Enemy> aliveEnemies = new List<Enemy>();
    public int totalDeadEnemyNumber = 0;

    private float timer = 0;
    private float spawnInterval = 5f;

    void Update()
    {
        if (!enableSpawn)
        {
            timer = Mathf.Max(timer, spawnInterval * 0.6f);
            return;
        }
        if (aliveEnemyNumber < maxAliveEnemyNumber)
        {
            timer += Time.deltaTime;
            if (timer >= spawnInterval)
            {
                bool spawnOnTheLeft = Random.Range(-1, 1) >= 0;
                int spawnIndex = Random.Range(0, enemyCategory.EnemyPrefabs.Count-1);
                SpawnEnemy(spawnIndex,spawnOnTheLeft);
                timer = 0;
            }
        }
        else
        {
            timer = Mathf.Max(timer, spawnInterval * 0.8f);
        }
        UpdateSpawnPointsPosition();
        UpdateSpawnMode();
    }

    private void UpdateSpawnPointsPosition()
    {
        float distance = (transform.position - Camera.main.transform.position).z;
        float leftScreenBorder = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, distance)).x;
        float rightScreenBorder = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, distance)).x;
        var positionLeft = leftSpawnPoint.position;
        positionLeft = new Vector3(leftScreenBorder - 10, positionLeft.y, positionLeft.z);
        leftSpawnPoint.position = positionLeft;
        var positionRight = rightSpawnPoint.position;
        positionRight = new Vector3(rightScreenBorder + 10, positionRight.y, positionRight.z);
        rightSpawnPoint.position = positionRight;
    }

    private void UpdateSpawnMode()
    {
        if (totalDeadEnemyNumber <= 2)
        {
            maxAliveEnemyNumber = 1;
            spawnInterval = 5f;
        }
        else if (totalDeadEnemyNumber <= 6)
        {
            maxAliveEnemyNumber = 2;
            spawnInterval = 4f;
        }
        else if (totalDeadEnemyNumber <= 10)
        {
            maxAliveEnemyNumber = 3;
            spawnInterval = 3f;
        }
        else if (totalDeadEnemyNumber <= 16)
        {
            maxAliveEnemyNumber = 4;
            spawnInterval = 2f;
        }
        else
        {
            maxAliveEnemyNumber = 5;
            spawnInterval = 1f;
        }
    }

    public void CancelAllEnemiesProvoked()
    {
        foreach (var enemy in aliveEnemies)
        {
            enemy.isProvoked = false;
        }
    }

    public void OnAliveEnemyDeath(Enemy enemy)
    {
        if (enemy == initEnemy)
        {
            enableSpawn = true;
            totalDeadEnemyNumber++;
            initEnemy = null;
            AudioManager.instance.PlayBGM(BackgroundMusicType.TenseBGM);
            EventCenter.Broadcast(EventType.FightStart);
            return;
        }
        aliveEnemies.Remove(enemy);
        aliveEnemyNumber--;
        totalDeadEnemyNumber++;
        timer = spawnInterval;
    }

    private void SpawnEnemy(int index, bool onTheLeft)
    {
        Vector3 spawnPosition = onTheLeft ? leftSpawnPoint.position : rightSpawnPoint.position;
        GameObject newEnemy = Instantiate(enemyCategory.EnemyPrefabs[index], spawnPosition, Quaternion.identity);
        newEnemy.transform.SetParent(transform);
        Enemy controller = newEnemy.GetComponent<Enemy>();
        controller.isProvoked = true;
        aliveEnemies.Add(controller);
        aliveEnemyNumber++;
    }
}
