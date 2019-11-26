using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverDefense : GameOverManager
{
    [SerializeField]
    protected BaseHealth baseHealth;

    protected virtual void Update()
    {
        if (playerHealth.getHealth() <= 0 && baseHealth.getHealth() <= 0)
        {
            anim.SetTrigger("GameOver");
            restartTimer += Time.deltaTime;
            if (restartTimer >= restartDelay)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
    }
}
