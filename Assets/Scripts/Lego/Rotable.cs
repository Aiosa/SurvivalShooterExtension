using UnityEngine;

public class Rotable : MonoBehaviour
{
    [SerializeField]
    float speed = 1f;

    [SerializeField]
    private Transform staticChild;


    private bool active = false;
    private Quaternion lastRotation = Quaternion.identity;

    public void enable()
    {
        Debug.Log("Enabled rotable");
        active = true;
    }

    private void FixedUpdate()
    {
        if (!active) return;

        if (Pause.gamePaused())
        {
            transform.rotation = Quaternion.identity;
        }
        else
        {
            transform.rotation = lastRotation;
            lastRotation = Quaternion.Euler(0f, (transform.rotation.eulerAngles.y + speed) % 360f, 0f);
        }
    }

    private void LateUpdate()
    {
        staticChild.rotation = Quaternion.Euler(0f, -transform.rotation.eulerAngles.z + 45f, 0f);
    }
}