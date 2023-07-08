using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shake : MonoBehaviour
{
    // Desired duration of the shake effect
    private float shakeDuration = 0f;

    // A measure of magnitude for the shake. Tweak based on your preference
    private float shakeMagnitude = 0.2f;

    // A measure of how quickly the shake effect should evaporate
    private float dampingSpeed = 1.0f;

    // The initial position of the GameObject
    Vector3 initialPosition;

    void OnEnable()
    {
        initialPosition = transform.localPosition;
    }

    void Update()
    {
        if (Time.timeScale < 1f)
        {
            return;
        }
        if (shakeDuration > 0)
        {
            transform.localPosition = initialPosition + Random.insideUnitSphere * shakeMagnitude;

            shakeDuration -= Time.deltaTime * dampingSpeed;
        }
        else
        {
            shakeDuration = 0f;
            transform.localPosition = initialPosition;
        }
    }

    public void TriggerShake()
    {
        shakeDuration = 1.0f;
    }
}
