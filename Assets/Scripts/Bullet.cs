using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    public float lifetime = 5f; // Time before the bullet is destroyed if it doesn't hit anything

    void Start()
    {
        // Destroy the bullet after a certain time if it doesn't hit anything
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Enemy"))
        {
            Destroy(other.gameObject);
        }
    }
}
