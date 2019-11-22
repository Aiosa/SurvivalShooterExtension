using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float speed = 6f;

    private Vector3 movement; 
    private Animator animator;
    private Rigidbody rigidBody;
    private int floorMask;
    private float camRayLength = 100f;

    void Awake()
    {
        floorMask = LayerMask.GetMask("Floor");
        animator = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        float height = Input.GetAxisRaw("Horizontal");
        float width = Input.GetAxisRaw("Vertical");

        Move(height, width);
        Turning();
        Animating(height, width);
    }

    protected virtual void Move(float h, float v)
    {
        movement.Set(h, 0f, v);
        movement = movement.normalized * speed * Time.deltaTime;
        rigidBody.MovePosition(transform.position + movement);
    }

    private void Turning()
    {
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitData;
        if (Physics.Raycast(camRay, out hitData, camRayLength, floorMask))
        {
            Vector3 playerToMouse = hitData.point - transform.position;
            playerToMouse.y = 0f;

            Quaternion newRotation = Quaternion.LookRotation(playerToMouse);
            rigidBody.MoveRotation(newRotation);
        }
    }

    private void Animating(float h, float v)
    {
        animator.SetBool("IsWalking", h != 0f || v != 0f);
    }
}
