using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class BulletPool : MonoBehaviour
{
    [SerializeField]
    private GameObject _bullet;
    [SerializeField]
    private int PoolInit = 100;
    [SerializeField]
    private AudioSource _audioSource;

    private IObjectPool<Bullet> _pool;

    public IObjectPool<Bullet> Pool
    {
        get
        {
            if (_pool == null)
                _pool = new ObjectPool<Bullet>(CreatePooledItem, OnTakeFromPool, OnReturnToPool, OnDestroyPoolObject, false, PoolInit);
            return _pool;
        }
    }

    Bullet CreatePooledItem()
    {
        var bullet = Instantiate(_bullet, transform);
        bullet.SetActive(false);
        var bulletComponent = bullet.GetComponent<Bullet>();
        bulletComponent.Pool = this;
        return bulletComponent;
    }

    void OnTakeFromPool(Bullet bullet)
    {
        bullet.gameObject.SetActive(true);
        bullet.Reset();
        _audioSource.PlayOneShot(_audioSource.clip);
    }

    void OnReturnToPool(Bullet bullet)
    {
        bullet.gameObject.SetActive(false);
    }

    void OnDestroyPoolObject(Bullet bullet)
    {
        Destroy(bullet);
    }
}
