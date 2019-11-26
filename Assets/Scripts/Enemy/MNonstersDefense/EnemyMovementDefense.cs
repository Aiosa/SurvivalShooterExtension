using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovementDefense : EnemyMovement
{

    private BaseHealth baseHealth;
    private Transform baseObject;
    
    void Start ()
    {
        baseObject = GameObject.FindGameObjectWithTag ("Base").transform;
        baseHealth = baseObject.GetComponent<BaseHealth>();
    }
    
    
    protected override void Update()
    {
       
        
        if (enemyHealth.getHealth() > 0 && playerHealth.getHealth() > 0 &&  baseHealth.getHealth() > 0)
        {
            if (body != null && body.velocity.magnitude <= 0.1f && !nav.enabled)
            {
                body.isKinematic = false;
                nav.enabled = true;
            }

           bool playerFind =  Vector3.Distance(gameObject.transform.position, player.position) < Vector3.Distance(gameObject.transform.position, baseObject.position);

           if (playerFind)
           {
               nav.SetDestination (player.position);
           }
           else
           {
               nav.SetDestination (baseObject.position);
           }

        }
        else
        {
            nav.enabled = false;
        }
    }


}
