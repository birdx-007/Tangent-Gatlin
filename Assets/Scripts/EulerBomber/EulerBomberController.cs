using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EulerBomberController : MonoBehaviour
{
    public float fireForce = 10f;
    public float recoilForce = 1f;
    public float frictionForce = 1f;
    public Transform bomberHead;
    public Transform bomberTail;
    public TextMesh bomberCount;
    private int _count = 0;
    public GameObject bomberBullet;
    public static Transform aimTarget;
    private Rigidbody2D _rigidbody2D;
    private Animator _animator;
    private float _bomberTailHorizontalOffset = 2;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        SetCount(0);
        aimTarget ??= GameObject.FindWithTag("Player").transform;
        bomberTail.localPosition = new Vector3(_bomberTailHorizontalOffset, 0, 0);
    }

    private void Update()
    {
        if (aimTarget is not null)
        {
            Aim();
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Fire();
            }
        }

        float v = Input.GetAxis("Vertical");
        float h = Input.GetAxis("Horizontal");
        _rigidbody2D.AddForce(new Vector2(h,v)*5f,ForceMode2D.Force);
        _rigidbody2D.AddForce(_rigidbody2D.velocity * (-frictionForce), ForceMode2D.Force); // 阻力
    }

    private void SetCount(int count)
    {
        _count = count;
        bomberCount.text = _count.ToString();
    }

    private void Aim()
    {
        var vectorToTarget = aimTarget.position - bomberHead.position;
        bomberHead.rotation = Quaternion.FromToRotation(Vector3.right, vectorToTarget);
        bomberTail.localPosition =
            new Vector3(vectorToTarget.x < 0 ? _bomberTailHorizontalOffset : -_bomberTailHorizontalOffset,
                0, 0);
    }

    private void Fire()
    {
        _animator.SetTrigger("fire");
        GameObject bullet =
            Instantiate(bomberBullet, bomberHead.position + 2f * bomberHead.right,
                Quaternion.Euler(0, 0, 90) * bomberHead.rotation);
        var bulletController = bullet.GetComponent<BomberBulletController>();
        if (bulletController != null)
        {
            bulletController.SetCount(_count);
        }

        var bulletBody = bullet.GetComponent<Rigidbody2D>();
        if (bulletBody != null)
        {
            bulletBody.AddForce(bomberHead.right * (fireForce * bulletBody.mass), ForceMode2D.Impulse);
        }
        _rigidbody2D.AddForce(bomberHead.right * (-recoilForce * _rigidbody2D.mass), ForceMode2D.Impulse);//后坐力
        SetCount(_count + 1);
    }
}
