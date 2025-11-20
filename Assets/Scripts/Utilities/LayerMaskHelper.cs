using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerMaskHelper
{
    /// <summary>
    /// Is game object in layermask ?
    /// </summary>
    public static bool CompareLayerMask(GameObject gameObject, LayerMask layerMask)
    {
        if((layerMask.value & (1 << gameObject.layer)) > 0)
        {
            return true;
        }
        return false;
    }
    public static LayerMask LayersToLayerMask(params int[] layers)
    {
        LayerMask result = 0;
        foreach(int layer in layers)
        {
            result |= (1 << layer);
        }
        return result;
    }
}
