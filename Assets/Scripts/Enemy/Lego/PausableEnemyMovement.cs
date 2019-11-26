using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PausableEnemyMovement : EnemyMovement
{
    EnemyHealth health;

    private void Start()
    {
        health = GetComponent<EnemyHealth>();
    }


    protected override void Update()
    {
        if (!Pause.gamePaused() && health.getHealth() > 0f)
        {
            NavMeshHit closestHit;
            if (NavMesh.SamplePosition(gameObject.transform.position, out closestHit, 500, 1))
            {
                gameObject.transform.position = closestHit.position;
            }

            nav.enabled = true;
            base.Update();
        } else
        {
            nav.enabled = false;

        }
    }
}
