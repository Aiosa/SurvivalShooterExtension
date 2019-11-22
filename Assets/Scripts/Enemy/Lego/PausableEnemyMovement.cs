using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PausableEnemyMovement : EnemyMovement
{
    protected override void Update()
    {
        if (!Pause.gamePaused())
        {
            nav.enabled = true;
            base.Update();
        } else
        {
            nav.enabled = false;

        }
    }
}
