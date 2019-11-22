using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManagerHU : MonoBehaviour
{
    [SerializeField]
    private PlayerHealthHU playerHealthHU;
    [SerializeField]
    private float restartDelay = 7f;

    Animator anim;
    float restartTimer;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }


    void Update()
    {
        if (playerHealthHU.getHealth() <= 0)
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
