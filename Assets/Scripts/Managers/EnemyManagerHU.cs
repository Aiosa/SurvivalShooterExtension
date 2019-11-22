using UnityEngine;

public class EnemyManagerHU : MonoBehaviour
{
    [SerializeField]
    private PlayerHealthHU playerHealthHU;
    [SerializeField]
    private GameObject enemy;
    [SerializeField]
    private float spawnTime = 3f;
    [SerializeField]
    private Transform[] spawnPoints;

    [SerializeField]
    private bool isActive = true;


    void Start ()
    {
       if (isActive) InvokeRepeating("Spawn", spawnTime, spawnTime);
    }


    void Spawn ()
    {
        if(playerHealthHU.getHealth() <= 0f)
        {
            return;
        }

        int spawnPointIndex = Random.Range (0, spawnPoints.Length);

        Instantiate (enemy, spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation);
    }
}
