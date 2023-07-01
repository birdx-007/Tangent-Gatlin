using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BomberBulletController : MonoBehaviour
{
    public TextMesh rightUpN;
    public TextMesh leftDownN;
    private int _count = 0;

    private void Awake()
    {
        SetCount(0);
    }

    public void SetCount(int count)
    {
        _count = count;
        rightUpN.text = _count.ToString();
        leftDownN.text = _count.ToString();
    }
}
