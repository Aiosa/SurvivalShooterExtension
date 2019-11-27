using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Sound : MonoBehaviour
{
    private bool initialized = false;

    // Start is called before the first frame update
    void Start()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Music");

        if (objs.Length > 1)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

        if (!initialized)
        {
            AudioSource audioSource = GetComponent<AudioSource>();
            audioSource.PlayDelayed(2.678f);
            initialized = true;
        }
        
    }
}
