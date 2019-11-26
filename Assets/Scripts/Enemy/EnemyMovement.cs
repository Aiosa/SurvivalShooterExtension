using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    protected Transform player;
    protected PlayerHealth playerHealth;
    protected EnemyHealth enemyHealth;
    protected NavMeshAgent nav;
    protected Rigidbody body;

    void Awake ()
    {
        player = GameObject.FindGameObjectWithTag ("Player").transform;
        playerHealth = player.GetComponent <PlayerHealth> ();
        enemyHealth = GetComponent <EnemyHealth> ();
        nav = GetComponent<NavMeshAgent>();
        body = GetComponent<Rigidbody>();
    }


    protected virtual void Update ()
    {
       
        if (enemyHealth.getHealth() > 0 && playerHealth.getHealth() > 0)
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
