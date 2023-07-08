using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public BulletPool Pool;
    public float Speed = 1f;
    public float Damage = 1f;
    public float Range = 1f;

    private float _elapsed = 0f;

    // Update is called once per frame
    void Update()
    {
        transform.position += -transform.right * Speed * Time.deltaTime;
        _elapsed += Time.deltaTime;

        if (_elapsed > Range)
        {
            Pool.Pool.Release(this);
        }
    }

    private static bool GetEnemy(Collider2D other, out Enemy enemy)
    {
        enemy = other.gameObject.GetComponent<Enemy>();
        return enemy != null;
    }

    public void Reset() {
        _elapsed = 0f;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (GetEnemy(other, out var enemy))
        {
            enemy.Damage(Damage);
            Pool.Pool.Release(this);
        }
    }
}
