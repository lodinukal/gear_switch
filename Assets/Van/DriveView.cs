using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Acceleration
{
    public Acceleration(float forward = 1f, float sideway = 1f, float backward = 1f)
    {
        this.Forward = forward;
        this.Sideway = sideway;
        this.Backward = backward;
    }

    public float Forward;
    public float Sideway;
    public float Backward;
}


public class DriveView : View
{
    [SerializeField]
    private RoadManager _road;

    [SerializeField]
    private Vector2 _vanVelocity = new Vector2(0f, 0f);
    [SerializeField]
    private Vector2 _vanAcceleration = new Vector2(0.0f, 0.0f);

    public Acceleration Acceleration = new Acceleration(2f, 1f, 0.5f);
    public Vector2 MaxVelocity = new Vector2(2.0f, 2.0f);

    [SerializeField]
    private float _inset = 1f;

    // Dry roads have a friction of 0.7
    // Wet - 0.4
    public float FrictionCoefficient = 0.7f;

    public override void Enter()
    {

    }

    public override void Exit()
    {

    }

    public override void UpdateView(float delta)
    {
        MoveVan(delta);
    }


    Vector2 GetMovementVector()
    {
        var movement = Vector2.zero;
        if (!InView)
        {
            return movement;
        }

        if (Input.GetKey(KeyCode.W))
        {
            movement.y = 1f;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            movement.y = -1f;
        }
        if (Input.GetKey(KeyCode.A))
        {
            movement.x = -1f;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            movement.x = 1f;
        }

        return movement.normalized;
    }

    void MoveVan(float delta)
    {
        var movement = GetMovementVector();
        movement *= delta;

        _vanAcceleration = movement;
        _vanAcceleration.y *= Acceleration.Sideway;
        _vanAcceleration.x *= movement.y > 0 ? Acceleration.Forward : Acceleration.Backward;

        _vanVelocity = _vanVelocity + _vanAcceleration * delta;
        _vanVelocity.x = Mathf.Clamp(_vanVelocity.x, -MaxVelocity.x, MaxVelocity.x);
        _vanVelocity.y = Mathf.Clamp(_vanVelocity.y, -MaxVelocity.y, MaxVelocity.y);

        // Apply friction
        var friction = -_vanVelocity * FrictionCoefficient * delta;
        _vanVelocity += friction;

        var roadPosition = _road.transform.position;
        var roadExtent = _road.RoadExtent;

        var vanPosition = Van.transform.position;

        var boundXMin = roadPosition.x + _inset;
        var boundXMax = roadPosition.x + roadExtent - _inset;
        var boundYMin = roadPosition.y - _road.RoadRange;
        var boundYMax = roadPosition.y + _road.RoadRange;

        var newPosition = _road.Clamp(vanPosition + new Vector3(_vanVelocity.x, _vanVelocity.y, 0) * delta, _inset);

        // if it goes out of bounds then we should reduce the speed.
        if (newPosition.x == boundXMin || newPosition.x == boundXMax)
        {
            _vanVelocity.x = 0;
        }

        if (newPosition.y == boundYMin || newPosition.y == boundYMax)
        {
            _vanVelocity.y = 0;
        }

        Van.transform.position = newPosition;
    }

    public override string GetName()
    {
        return "Driver";
    }
}
