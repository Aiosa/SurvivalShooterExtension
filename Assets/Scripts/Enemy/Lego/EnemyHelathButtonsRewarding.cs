using UnityEngine;

public class EnemyHelathButtonsRewarding : EnemyHealth
{

    protected override void Death()
    {
        if (Random.Range(0f, Mathf.Max(scoreValue + 2f, 10f)) > scoreValue)
        {
            ButtonsManager.add((int)(scoreValue / 2f));
        }
        base.Death();
    }
}
