using UnityEngine;

public class PausableEnemyManager : EnemyManager
{
    float elapsed;
    float spawnDelay;
    float lastSpawned;
    const float updateDelay = 10f;


    protected override void Start()
    {
        //do not invoke the method, use Update()
    }

    private void Awake()
    {
        elapsed = 0f;
        spawnDelay = getSpawnDelay(elapsed);
        lastSpawned = 0f;
    }

    protected void Update()
    {
        if (!Pause.gamePaused())
        {
            elapsed += Time.deltaTime;

            if (elapsed % updateDelay < 1f)
            {
                float newSpawn = getSpawnDelay(elapsed);
                spawnDelay = Mathf.Min(newSpawn, spawnDelay);
            }

            if (lastSpawned >= spawnDelay)
            {
                Spawn();
                Debug.Log("Spawned in " + lastSpawned + ", in game time of " + elapsed);

                lastSpawned = 0f;
            }
            else
            {
                lastSpawned += Time.deltaTime;
            }
        }


    }

    private static float getSpawnDelay(float elapsedTime)
    {
        return 3 * Mathf.Sin((elapsedTime + 300f) / 200f) + 3.3f;
    }
}