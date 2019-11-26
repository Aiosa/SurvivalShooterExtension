using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthExplode : EnemyHealth
{
    [SerializeField]
    private GameObject explosion;
    private float radius = 3.5F;
    private int damage = 30;

    protected override void Death()
    {
        base.Death();
        Instantiate(explosion, gameObject.transform.position,Quaternion.identity);
        Vector3 explosionPos = transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPos, radius);
        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();

            if (rb != null) {
            if (rb.gameObject.tag == "Player")
            {
                rb.gameObject.GetComponent<PlayerHealth>().TakeDamage(damage);
            } else if ( rb.gameObject.tag == "Enemy")
            {
                rb.gameObject.GetComponent<EnemyHealth>().TakeDamage(damage, Vector3.zero);
            }

            }
            }
        
        }
   
}
