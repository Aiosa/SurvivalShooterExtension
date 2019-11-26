using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    [SerializeField]
    protected PlayerHealth playerHealth;
    [SerializeField]
    private float restartDelay = 7f;

    Animator anim;
    protected float restartTimer;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }


    protected virtual void Update()
    {
        if (playerHealth.getHealth() <= 0)
        {
            anim.SetTrigger("GameOver");
            restartTimer += Time.deltaTime;
            if (restartTimer >= restartDelay)
            {
                SceneManager.LoadScene("Main");
            }
        }
    }
}
