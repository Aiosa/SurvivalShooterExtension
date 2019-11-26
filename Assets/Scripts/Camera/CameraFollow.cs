
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraFollow : MonoBehaviour
{
    [SerializeField]
    private Transform target;
    [SerializeField]
    private float smoothing = 5f;

    Vector3 offset;

    private void Start()
    {
        offset = transform.position - target.position;
    }

    void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("Main");
            return;
        }


        Vector3 targetPosition = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothing * Time.deltaTime);
    }
}
