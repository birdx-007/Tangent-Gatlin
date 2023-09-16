using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundController : MonoBehaviour
{
    public Transform followTransform;
    private Transform groundTransform;

    private void Awake()
    {
        groundTransform = transform;
    }

    private void LateUpdate()
    {
        Vector3 oldPosition = groundTransform.position;
        transform.position=new Vector3(followTransform.position.x,oldPosition.y,oldPosition.z);
    }
}
