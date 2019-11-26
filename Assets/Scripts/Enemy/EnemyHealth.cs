using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField]
    private int startingHealth = 100;
    [SerializeField]
    private int currentHealth;
    [SerializeField]
    private float sinkSpeed = 2.5f;
    [SerializeField]
    protected int scoreValue = 10;
    [SerializeField]
    private AudioClip deathClip;


    Animator anim;
    AudioSource enemyAudio;
    ParticleSystem hitParticles;
    CapsuleCollider capsuleCollider;
    
    bool isDead;
    bool isSinking;


    void Awake()
    {
        anim = GetComponent<Animator>();
        enemyAudio = GetComponent<AudioSource>();
        hitParticles = GetComponentInChildren<ParticleSystem>();
        capsuleCollider = GetComponent<CapsuleCollider>();

        currentHealth = startingHealth;
    }


    void Update()
    {
        if (isSinking)
        {
            transform.Translate(-Vector3.up * sinkSpeed * Time.deltaTime);
        }
    }


    public void TakeDamage(int amount, Vector3 hitPoint)
    {
        if (isDead)
            return;

        enemyAudio.Play();

        currentHealth -= amount;

        hitParticles.transform.position = hitPoint;
        hitParticles.Play();

        if (currentHealth <= 0)
        {
            Death();
        }
    }

    public int getHealth() {
         return currentHealth;
    }


    protected virtual void Death ()
    {
        isDead = true;

        capsuleCollider.isTrigger = true;

        anim.SetTrigger ("Dead");

        enemyAudio.clip = deathClip;
        enemyAudio.Play ();
    }


    public void StartSinking ()
    {
        GetComponent <UnityEngine.AI.NavMeshAgent> ().enabled = false;
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        GetComponent <Rigidbody> ().isKinematic = true;
        isSinking = true;
        ScoreManager.add(scoreValue);
        Destroy (gameObject, 2f);
    }
}
