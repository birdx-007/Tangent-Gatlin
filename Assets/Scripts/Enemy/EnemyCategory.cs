using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyCategory",menuName = "CategoryData/EnemyCategory")]
public class EnemyCategory : ScriptableObject
{
    public List<GameObject> EnemyPrefabs;
}
