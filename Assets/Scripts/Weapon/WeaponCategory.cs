using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponCategory",menuName = "CategoryData/WeaponCategory")]
public class WeaponCategory : ScriptableObject
{
    public List<GameObject> WeaponPrefabs;
}
