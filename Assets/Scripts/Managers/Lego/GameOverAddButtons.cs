using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public class GameOverAddButtons : GameOverManager
{
    [SerializeField]
    private int multiplyButtons = 2;
    [SerializeField]
    private Text addValue;

    private float sumDelay = 2.0f;
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

    public bool isGameOver()
    {
        return playerHealth.getHealth() <= 0;
    }

    protected override void Update()
    {
        if (isGameOver())
        {
            if (restartTimer >= sumDelay)
            {
                addScore();
            }
            base.Update();
        }
    }
}
