
public class PausablePlayerShooting : PlayerShooting
{
    protected override void Update()
    {
        if (!Pause.gamePaused())
        {
            base.Update();
        } else
        {
            DisableEffects();
        }
    }

}
