using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField]
    private PlayerHealth playerHealth;
    [SerializeField]
    private GameObject enemy;
    [SerializeField]
    protected float spawnTime = 3f;
    [SerializeField]
    private Transform[] spawnPoints;


    protected virtual void Start ()
    {
       InvokeRepeating ("Spawn", spawnTime, spawnTime);
    }

    protected virtual void Spawn ()
    {
        if(playerHealth.getHealth() <= 0f)
        {
            return;
        }

        int spawnPointIndex = Random.Range (0, spawnPoints.Length);

        Instantiate (enemy, spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation);
    }
}
