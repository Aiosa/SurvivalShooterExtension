using UnityEngine;
using UnityEngine.AI;

public class EnemyMovementHU : MonoBehaviour
{
    Transform player;
    PlayerHealthHU playerHealthHU;
    EnemyHealthHU enemyHealthHU;
    NavMeshAgent nav;
    Rigidbody body;

    void Awake ()
    {
        player = GameObject.FindGameObjectWithTag ("Player").transform;
        playerHealthHU = player.GetComponent <PlayerHealthHU> ();
        enemyHealthHU = GetComponent <EnemyHealthHU> ();
        nav = GetComponent<NavMeshAgent>();
        body = GetComponent<Rigidbody>();
    }


    void Update ()
    {
       
        if (enemyHealthHU.getHealth() > 0 && playerHealthHU.getHealth() > 0)
        {
            if (body != null && body.velocity.magnitude <= 0.1f && !nav.enabled)
            {
                body.isKinematic = false;
                nav.enabled = true;
            }

            nav.SetDestination (player.position);

        }
        else
        {
            nav.enabled = false;
        }
    }
}
