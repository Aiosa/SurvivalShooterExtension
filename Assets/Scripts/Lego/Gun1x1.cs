using UnityEngine;

public class Gun1x1 : Cube1x1
{
    [SerializeField]
    private float minRange = 2f;
    [SerializeField]
    private float smallRange = 8f;
    [SerializeField]
    private float longRange = 32f;

    private Vector3 sidePoint = new Vector3(-0.135f, 0.43f, 0.135f);
    private int counter = 0;

    private GameObject head1;
    private GameObject head2;

    private Vector3 shootPosition = new Vector3(-0.145f, 0.43f, 0.145f);

    private bool playerNear = false;
    private bool onTrigger = false;

    public Ray getRay()
    {
        Debug.Log("POINT " + sidePoint);
      
        return new Ray(transfromWithoutCounterDetection(shootPosition), toV3(Matrix4x4.Rotate(transform.rotation) * new Vector3(-1f, 0f, 1f)).normalized);
    }

    private Vector3 toV3(Vector4 from)
    {
        return new Vector3(from.x, from.y, from.z);
    }

    public bool hasLongRange()
    {
        return head2 != null;
    }

    public float getRange()
    {
        if (head1 == null) return minRange;
        return hasLongRange() ? longRange : smallRange;
    }

    public bool isActive()
    {
        return onTrigger;
    }


    public void addGunHead(GameObject head)
    {
        if (head1 == null)
        {
            head1 = head;
        }
        else
        {
            head2 = head;
        }
    }

    public void setOnTrigger()
    {
        onTrigger = true;

    }

    public Vector3 getSidePoint()
    {
        return transformPoint(sidePoint);
    }

    private Vector3 transfromWithoutCounterDetection(Vector3 p)
    {
        Vector4 res = Matrix4x4.Rotate(transform.rotation) * p;
        return new Vector3(res.x, res.y, res.z) + transform.position;
    }

    private Vector3 transformPoint(Vector3 p)
    {
        //Debug.Log("Counter " + counter + ", side " + sidePoint + ", rotation " + transform.rotation);
        Vector4 res = Matrix4x4.Rotate(transform.rotation) * p;
        return new Vector3(res.x, (counter > 1) ? -res.y : res.y, res.z) + transform.position;
    }

    public void increaseGunHeadLength()
    {
        counter++;
        sidePoint.x -= 0.54f;
        sidePoint.z += 0.54f;

        shootPosition.x -= 0.6f;
        shootPosition.z += 0.6f;
    }

    protected override void clean()
    {
        head1 = null;
        head2 = null;
        counter = 0;
        base.clean();
    }

    protected override void onPlacement()
    {
        //Debug.Log("parrent " + getParent().name);
        if (getParent() != null && getParent().GetComponentInParent<Trigger2x2>().GetType() == typeof(Trigger2x2))
        {
            setOnTrigger();
        }

    }
}
