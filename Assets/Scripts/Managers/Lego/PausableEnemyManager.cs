
public class PausableEnemyManager : EnemyManager
{
    protected override void Start()
    {
        InvokeRepeating("Spawn", spawnTime, spawnTime);
    }

    protected override void Spawn()
    {
        if (!Pause.gamePaused())
        {
            base.Spawn();
        }
    }
}
