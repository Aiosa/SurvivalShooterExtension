using UnityEngine;
using UnityEngine.AI;

public class GunShooting : MonoBehaviour
{
    [SerializeField]
    private int damagePerShot = 2;
    [SerializeField]
    private float timeBetweenBullets = 0.1f;
    [SerializeField]
    private MetalImpactScript shootMetalAnimation;

    private float timer;
    private RaycastHit shootHit;
    private int shootableMask;
    private LineRenderer gunLine;
    private Gun1x1 origin;

    private AudioSource gunAudio;

    void Awake()
    {
        shootableMask = LayerMask.GetMask("Shootable");
        gunLine = GetComponent<LineRenderer>();
        gunLine.useWorldSpace = false;
        gunAudio = GetComponent<AudioSource>();
        origin = GetComponent<Gun1x1>();
    }


    protected virtual void Update()
    {
        if (!Pause.gamePaused())
        {
            gunLine.enabled = false;
            if (!origin.isActive() || Pause.gamePaused()) return;

            timer += Time.deltaTime;

            if (timer >= timeBetweenBullets && Time.timeScale != 0)
            {
                Shoot();
            }
        }
    }


    void Shoot()
    {
        timer = 0f;

        gunAudio.Play();

        Ray shoot = origin.getRay();

        gunLine.enabled = true;
        gunLine.useWorldSpace = true;
        gunLine.SetPosition(0, shoot.origin);


        if (Physics.Raycast(shoot, out shootHit, origin.getRange(), shootableMask))
        {
            if (shootHit.collider.gameObject.tag.Equals("Enemy"))
            {
                EnemyHealth enemyHealth = shootHit.collider.GetComponent<EnemyHealth>();
                if (enemyHealth != null)
                {
                    enemyHealth.TakeDamage(damagePerShot, shootHit.point);
                }
            }
            else
            {
                Instantiate(shootMetalAnimation, shootHit.point, Quaternion.identity);
            }
            gunLine.SetPosition(1, shootHit.point);
        }
        else
        {
            gunLine.SetPosition(1, shoot.origin + shoot.direction * origin.getRange());
        }
    }
}
