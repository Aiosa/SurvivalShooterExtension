using UnityEngine;
using System.Collections;

public class EnemyAttackHU : MonoBehaviour
{
    [SerializeField]
    private float timeBetweenAttacks = 0.5f;
    [SerializeField]
    private int attackDamage = 10;

    Animator anim;
    GameObject player;
    PlayerHealthHU playerHealthHU;
    EnemyHealthHU enemyHealthHU;
    bool playerInRange;
    float timer;


    void Awake ()
    {
        player = GameObject.FindGameObjectWithTag ("Player");
        playerHealthHU = player.GetComponent <PlayerHealthHU> ();
        enemyHealthHU = GetComponent<EnemyHealthHU>();
        anim = GetComponent <Animator> ();
    }


    void OnTriggerEnter (Collider other)
    {
        if(other.gameObject == player)
        {
            playerInRange = true;
        }
    }


    void OnTriggerExit (Collider other)
    {
        if(other.gameObject == player)
        {
            playerInRange = false;
        }
    }


    void Update ()
    {
        timer += Time.deltaTime;

        if(timer >= timeBetweenAttacks && playerInRange && enemyHealthHU.getHealth() > 0)
        {
            Attack ();
        }

        if(playerHealthHU.getHealth() <= 0)
        {
            anim.SetTrigger ("PlayerDead");
        }
    }


    void Attack ()
    {
        timer = 0f;

        if(playerHealthHU.getHealth() > 0)
        {
            playerHealthHU.TakeDamage (attackDamage);
        }
    }
}
