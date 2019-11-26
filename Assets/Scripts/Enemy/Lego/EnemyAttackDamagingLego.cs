using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackDamagingLego : EnemyAttack
{
    [SerializeField]
    private float delay = 5f;

    private float damageDelay = 10f;

    private void resetTimer()
    {
        damageDelay = delay;
    }

    void OnTriggerStay(Collider other)
    {
        //Debug.Log("stay");
        if (other.gameObject.tag.Equals("Lego"))
        {
            if (damageDelay <= 0f)
            {
                other.gameObject.GetComponentInParent<Lego>().damage(attackDamage);
                resetTimer();
            }
            damageDelay -= Time.deltaTime;
        }
        
        base.OnTriggerEnter(other);
    }


}
