using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public class GameOverAddButtons : GameOverManager
{
    [SerializeField]
    private int multiplyButtons = 2;
    [SerializeField]
    private Text addValue;

    private float sumDelay = 2.5f;
    private bool added = false;

    private void addScore()
    {
        if (added) return;
        int buttons = ButtonsManager.getAmount();

        if (buttons > 0)
        {
            addValue.text = "+" + (buttons * multiplyButtons);
            ButtonsManager.spend(buttons);
            ScoreManager.add(buttons * multiplyButtons);
        }
        added = true;
    }

    protected override void Update()
    {
        if (playerHealth.getHealth() <= 0)
        {
            if (restartTimer >= sumDelay)
            {
                Debug.Log("ADDED");
                addScore();
            }
            base.Update();
        }
    }
}
