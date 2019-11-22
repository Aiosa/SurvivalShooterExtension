using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PausablePlayerMovement : PlayerMovement
{
    protected override void Move(float h, float v)
    {
        if (!Pause.gamePaused())
        {
            base.Move(h, v);
        }
    }
}
