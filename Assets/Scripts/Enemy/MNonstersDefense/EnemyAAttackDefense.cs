using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAAttackDefense : MonoBehaviour
{
   
    [SerializeField]
    private float timeBetweenAttacks = 0.5f;
    [SerializeField]
    protected int attackDamage = 10;

    Animator anim;
    GameObject player;
    PlayerHealth playerHealth;
    GameObject baseObject;
    BaseHealth baseHealth;
    EnemyHealth enemyHealth;
    private bool attackToPlayer;
    private bool attackToBase;
    float timer;


    void Awake ()
    {
        player = GameObject.FindGameObjectWithTag ("Player");
        playerHealth = player.GetComponent <PlayerHealth> ();
        baseObject =  GameObject.FindGameObjectWithTag ("Base");
        baseHealth = baseObject.GetComponent <BaseHealth>();
        enemyHealth = GetComponent<EnemyHealth>();
        anim = GetComponent <Animator> ();
    }


    protected virtual void OnTriggerEnter (Collider other)
    {
        if(other.gameObject == player)
        {
            attackToPlayer = true;
        }
        if(other.gameObject == baseObject)
        {
            attackToBase = true;
        }
    }


    protected virtual void OnTriggerExit (Collider other)
    {
        if(other.gameObject == player)
        {
            attackToPlayer = false;
        }
        if(other.gameObject == baseObject)
        {
            attackToBase = false;
        }
    }


    void Update ()
    {
        timer += Time.deltaTime;

        if(timer >= timeBetweenAttacks  && enemyHealth.getHealth() > 0 && baseHealth.getHealth() > 0)
        {
            if (attackToPlayer)
            {
                Attack (true);
            }

            if (attackToBase)
            {
                Attack (false);
            }
        }
           
     

        if(playerHealth.getHealth() <= 0 || baseHealth.getHealth() <= 0)
        {
            anim.SetTrigger ("PlayerDead");
        }
    }


    void Attack (bool isPlayer)
    {
        timer = 0f;

        if(playerHealth.getHealth() > 0 && isPlayer)
        {
            playerHealth.TakeDamage (attackDamage);
        }
        else
        {
            baseHealth.TakeDamage (attackDamage);
        }
    }
}
