using UnityEngine;

public class Pause : MonoBehaviour
{
    private static bool isPaused = false;

    public static bool gamePaused()
    {
        return isPaused;
    }

    public static void setPaused(bool paused)
    {
        isPaused = paused;
    }
}
