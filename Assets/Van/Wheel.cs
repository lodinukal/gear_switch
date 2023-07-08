using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wheel : MonoBehaviour
{
    public float Speed = 1f;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0f, 0f, Speed * -360 * Time.deltaTime);
    }
}
