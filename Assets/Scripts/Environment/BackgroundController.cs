using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    private float _originY;
    private BackgroundElement[] _backgroundElements;
    private const float destoryTime = 4f;
    private float _destoryTimer;
    private bool _isTense = false;

    private void OnEnable()
    {
        EventCenter.AddListener(EventType.FightStart,BecomeTense);
    }

    private void OnDisable()
    {
        EventCenter.RemoveListener(EventType.FightStart,BecomeTense);
    }

    private void Start()
    {
        _originY = transform.position.y;
        _backgroundElements = transform.GetComponentsInChildren<BackgroundElement>();
    }

    private void Update()
    {
         transform.position = new Vector3(transform.position.x,_originY, transform.position.z);
        if (_isTense)
        {
            _destoryTimer -= Time.deltaTime;
            if( _destoryTimer < 0 )
            {
                _isTense = false;
                foreach (var element in _backgroundElements)
                {
                    Destroy(element.gameObject);
                }
            }
        }
    }

    private void BecomeTense()
    {
        _destoryTimer = destoryTime;
        _isTense = true;
        foreach (var element in _backgroundElements)
        {
            element.ChangeState(new ElementTenseState());
        }
    }
}
