using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public float Damage = 1f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private static bool GetVan(Collider2D other, out Van van) {
        van = other.gameObject.GetComponent<Van>();
        return van != null;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (GetVan(other, out var van))
        {
            van.Manager.Damage(Damage);
            Destroy(gameObject);
        }
    }
}
