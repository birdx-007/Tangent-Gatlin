using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageSource : MonoBehaviour
{
    public struct DamageMapping
    {
        public LayerMask layerMask;
        public Damage damage;
    }

    public List<DamageMapping> damageCategory;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        foreach(var mapping in damageCategory)
        {
            if (LayerMaskHelper.CompareLayerMask(collision.gameObject, mapping.layerMask))
            {
                collision.GetComponent<IHurtable>()?.GetHurt(mapping.damage);
                break;
            }
        }
    }
}
