using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthClone: EnemyHealth
{
    [SerializeField]
    private GameObject masshroomSmall;

    protected override void Death()
    {
        base.Death();
        Vector3 positionX = new Vector3(1,0,0);
        
        Instantiate(masshroomSmall, gameObject.transform.position - positionX,Quaternion.identity);
        Instantiate(masshroomSmall, gameObject.transform.position + positionX,Quaternion.identity);
        
    }
   
}
