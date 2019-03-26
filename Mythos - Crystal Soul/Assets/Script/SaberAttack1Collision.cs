using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaberAttack1Collision : MonoBehaviour
{
    [SerializeField] float damage = 50f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D otherCollider)
    {
        Enemy enemy = otherCollider.gameObject.GetComponent<Enemy>();
        if (!enemy) { return; }
        enemy.TakeDamage(damage);
    }
}
