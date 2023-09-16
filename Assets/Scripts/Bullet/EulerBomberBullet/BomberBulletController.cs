using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class BomberBulletController : EnemyBullet
{
    public TextMesh rightUpN;
    public TextMesh leftDownN;
    public TextMesh explosion;
    [SerializeField] private Collider2D _collider2D;
    private SpriteRenderer _spriteRenderer;
    private int _count = 0;
    private float _numericValue = 1;
    private string _valueString;
    private float _valueStringAppearTime;
    private float _valueStringStayTime;
    private float _valueStringDisappearTime;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        SetCount(0);
        //explosion.color = new Color(1, 1, 1, 0);
        transform.localScale = new Vector3(0, 0, 1);
    }

    public void SetCount(int count)
    {
        _count = count;
        SetupValueAndValueString(count);
        rightUpN.text = _count.ToString();
        leftDownN.text = _count.ToString();
    }

    void SetupValueAndValueString(int count)
    {
        _numericValue = 1;
        for (int i = 1; i <= count; i++)
        {
            _numericValue *= Mathf.PI / count;
        }
        if (count != 0)
        {
            _valueString = _numericValue.ToString("F27");
            if (count % 4 == 1 || count % 4 == 3)
            {
                _valueString += "i";
            }
            _valueString=_valueString.Insert(5, "\n");
            _valueString=_valueString.Insert(11+1, "\n");
            _valueString=_valueString.Insert(19+2, "\n");
            _valueString=_valueString.Insert(25+3, "\n");
            if (count % 4 == 2 || count % 4 == 3)
            {
                _numericValue *= -1;
                _valueString = "-" + _valueString;
            }
            _valueStringAppearTime = 0.2f;
            _valueStringStayTime = 0.3f;
            _valueStringDisappearTime = 0.25f;
        }
        else
        {
            _valueString = "1";
            explosion.fontSize *= 3;
            _valueStringAppearTime = 0.1f;
            _valueStringStayTime = 0.4f;
            _valueStringDisappearTime = 0.25f;
        }
    }

    protected override void OnHitPlayer(PlayerController player)
    {
        if (player.isAlive)
        {
            player.Die();
            player.Movement.BeHitAway(_rigidbody2D.velocity.normalized * hitForce);
        }
        DisplayExplosionThenDestroy();
    }

    protected override void OnHitPlayerBullet(WeaponBullet bullet)
    {
        DisplayExplosionThenDestroy();
        GameLogicManager.instance.ShakeCamera(15,1,0.2f);
    }

    protected override void OnHitGround(GameObject ground)
    {
        DisplayExplosionThenDestroy();
        GameLogicManager.instance.ShakeCamera(5,1,0.2f);
    }

    private void DisplayExplosionThenDestroy()
    {
        _rigidbody2D.velocity = new Vector2(0, 0);
        _collider2D.isTrigger = true;
        _spriteRenderer.sprite = null;
        rightUpN.text = "";
        leftDownN.text = "";
        explosion.transform.rotation = Quaternion.identity;
        // explosion animation
        Sequence sequence = DOTween.Sequence();
        sequence.Append(DOTween.To(() => explosion.text, value => explosion.text = value, _valueString,
            _valueStringAppearTime));
        sequence.AppendInterval(_valueStringStayTime);
        sequence.Append(DOTween.To(() => explosion.color, value => explosion.color = value, new Color(1, 1, 1, 0),
            _valueStringDisappearTime));
        sequence.AppendCallback(() => Destroy(gameObject));
    }
}
