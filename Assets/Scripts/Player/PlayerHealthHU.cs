using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class PlayerHealthHU : MonoBehaviour
{
    [SerializeField]
    private int startingHealth = 100;
    [SerializeField]
    private int currentHealth;
    [SerializeField]
    private Slider healthSlider;
    [SerializeField]
    private Image damageImage;
    [SerializeField]
    private AudioClip deathClip;
    [SerializeField]
    private float flashSpeed = 5f;
    [SerializeField]
    private Color flashColour = new Color(1f, 0f, 0f, 0.1f);

    private int maxHealth;

    Animator anim;
    AudioSource playerAudio;
    PlayerMovement playerMovement;
    PlayerShootingHU playerShootingHU;
    bool isDead;
    bool damaged;


    void Awake ()
    {
        anim = GetComponent <Animator> ();
        playerAudio = GetComponent <AudioSource> ();
        playerMovement = GetComponent <PlayerMovement> ();
        playerShootingHU = GetComponentInChildren <PlayerShootingHU> ();
        currentHealth = startingHealth;
        maxHealth = startingHealth;
    }


    void Update ()
    {
        if(damaged)
        {
            damageImage.color = flashColour;
        }
        else
        {
            damageImage.color = Color.Lerp (damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
        }
        damaged = false;
    }


    public void TakeDamage (int amount)
    {
        damaged = true;

        currentHealth -= amount;

        healthSlider.value = currentHealth;

        playerAudio.Play ();

        if(currentHealth <= 0 && !isDead)
        {
            Death ();
        }
    }

    public int getHealth()
    {
        return currentHealth;
    }

    public void heal(int amount)
    {
        if (isDead)
        {
            return;
        }
        currentHealth = Mathf.Min(maxHealth, currentHealth + amount);
        healthSlider.value = currentHealth;
    }

    public void upgradeHealth(int amount)
    {
        if (isDead)
        {
            return;
        }
        maxHealth += amount;
        healthSlider.maxValue = maxHealth;
    }

    void Death ()
    {
        isDead = true;

        healthSlider.gameObject.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = Color.gray;
        healthSlider.gameObject.transform.Find("Background").GetComponent<Image>().color = Color.gray;

        playerShootingHU.DisableEffects ();

        anim.SetTrigger ("Die");

        playerAudio.clip = deathClip;
        playerAudio.Play ();

        playerMovement.enabled = false;
        playerShootingHU.enabled = false;
    }


    public void RestartLevel ()
    {
        SceneManager.LoadScene (0);
    }
}
