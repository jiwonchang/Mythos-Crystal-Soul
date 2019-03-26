using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] float health = 50f;
    [SerializeField] float collisionDamage = 10f;

    [SerializeField] GameObject deathVFX;
    [SerializeField] float deathVFXDuration = 2f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        GameObject deathExplosion = Instantiate(deathVFX, transform.position, transform.rotation);
        Destroy(gameObject);
        Destroy(deathExplosion, deathVFXDuration);
    }

    public float GetCollisionDamage()
    {
        return collisionDamage;
    }
}
