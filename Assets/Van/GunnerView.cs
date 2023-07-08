using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunnerView : View
{
    [SerializeField]
    private GameObject _turretMount;
    [SerializeField]
    private BulletPool _bulletPool;

    public float RotationAngle = 80f;
    public float RotationSpeed = 1f;
    public float AutoRotationSpeed = 1f;
    public float BulletInterval = 0.1f;

    private float _nextBullet = 0f;
    private float _angle = 0f;
    private bool _down = false;

    public override void Enter()
    {
    }

    public override void Exit()
    {
    }

    public override void UpdateView(float delta)
    {
        if (_turretMount == null)
            _turretMount = Van.transform.Find("turret_mount").gameObject;

        if (InView)
        {
            UserTurret(delta);
            if (Input.GetMouseButton(0))
            {
                Fire();
            }
        }
        else
        {
            AutoTurret(delta);
        }
        UpdateTurretDirection(delta);
    }

    void UpdateTurretDirection(float delta)
    {
        var nextRotation = Quaternion.AngleAxis(_angle, Vector3.forward);
        var currentRotation = _turretMount.transform.rotation;
        var difference = Quaternion.Angle(currentRotation, nextRotation);

        _turretMount.transform.rotation = Quaternion.Lerp(currentRotation, nextRotation, delta * RotationSpeed * difference);
    }

    void UserTurret(float delta)
    {
        var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var direction = _turretMount.transform.position - mousePosition;
        _angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        _angle = Mathf.Clamp(_angle, -RotationAngle, RotationAngle);
    }

    void AutoTurret(float delta)
    {
        if (_down)
        {
            _angle -= AutoRotationSpeed * 360 * delta;
            if (_angle < -RotationAngle)
            {
                _angle = -RotationAngle;
                _down = false;
            }
        }
        else
        {
            _angle += AutoRotationSpeed * 360 * delta;
            if (_angle > RotationAngle)
            {
                _angle = RotationAngle;
                _down = true;
            }
        }
    }

    void FireBullet()
    {
        var bullet = _bulletPool.Pool.Get();
        bullet.transform.position = _turretMount.transform.position;
        bullet.transform.rotation = _turretMount.transform.rotation;
    }

    void Fire()
    {
        if (_nextBullet > 0f)
        {
            _nextBullet -= Time.deltaTime;
            return;
        }

        FireBullet();
        _nextBullet = BulletInterval;
    }

    public override string GetName()
    {
        return "Gunner";
    }
}
