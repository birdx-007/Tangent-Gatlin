using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(AudioSource))]
public class Bullet : MonoBehaviour
{
    protected Rigidbody2D _rigidbody2D;
    protected AudioSource _audioSource;
    public float maxExistTime = 60;
    public float hitForce = 5f;

    private void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _audioSource = GetComponent<AudioSource>();
        StartCoroutine(TimerForExist());
    }

    IEnumerator TimerForExist()
    {
        yield return new WaitForSeconds(maxExistTime);
        Destroy(gameObject);
    }

    protected virtual void OnHitGround(GameObject ground){}
}
