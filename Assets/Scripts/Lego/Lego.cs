using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Lego : MonoBehaviour
{
    ////////////////////////
    ////  CONSTANTS    /////
    ////////////////////////

    public static float MAX_HEIGHT = 1.6f;
    protected static float OFFSET = 5f;

    protected static Matrix4x4 rotate45 = Matrix4x4.Rotate(Quaternion.Euler(0f, 45f, 0f));
    protected static Matrix4x4 rotateMinus45 = Matrix4x4.Rotate(Quaternion.Euler(0f, -45f, 0f));

    private float deltaHeight = -0.01f;
    private float squareSize = 1.56f;

    private Color32 enabledColor;
    private Color32 disabedColor;

    ////////////////////////
    //// SHARED VARS   /////
    ////////////////////////

    protected GameObject lastHit = null;
    protected Transform rotablePredecessor = null;
    protected int collides;
    protected bool outsideBounds = false;
    protected bool invalidLocation = false;
    protected Vector3 calculatedPosition = Vector3.zero;
    protected Matrix4x4 calculatedRotationMatrix = Matrix4x4.identity;

    private bool lastEnabled = false;
    private bool isPreview = true;
    private List<GameObject> dependent = new List<GameObject>();

    [SerializeField]
    private BoxCollider trigger;

    [SerializeField]
    private int value;

    [SerializeField]
    private int maxHealth = 200;
    private int health;

    private static float mapHalfLength = 22f;

    public static float getMapHalfLength()
    {
        return mapHalfLength;
    }

    protected virtual void Start()
    {
        health = maxHealth;
        enabledColor = new Color32(40, 52, 253, 90);
        disabedColor = new Color32(200, 60, 11, 90);
    }

    private void OnDestroy()
    {
        foreach (GameObject child in dependent)
        {
            if (child != null) Destroy(child);
        }
    }

    //util
    protected float roundByCellWidth(float axisVal, float add)
    {
        return ((int)(axisVal / getCubeGridCellWith())) * getCubeGridCellWith() + add;
    }
    //util
    protected static float clipToMap(float axisVal)
    {
        if (axisVal > mapHalfLength)
            return mapHalfLength;
        if (axisVal < -mapHalfLength)
            return -mapHalfLength;
        return axisVal;
    }
    protected float validateAxis(float value, float add = 0f)
    {
        return roundByCellWidth(clipToMap(value), add);
    }

    public void damage(int amount)
    {
        health -= amount;
        reddenColor(((float)(maxHealth - health)) / maxHealth);
        //Debug.Log("DAMAGED " + amount);
        if (health < 0)
        {
            Destroy(gameObject);
        }
    }

    public void setRotablePredecessor(Transform predecessor)
    {
        rotablePredecessor = predecessor;
        transform.SetParent(predecessor);
    }

    public Transform getRotablePredecessor()
    {
        return rotablePredecessor;
    }

    public void registerDepenency(GameObject child)
    {
        dependent.Add(child);
    }

    public bool isYRotated()
    {
        return transform.eulerAngles.y > 1f;
    }

    protected bool canBePlaced()
    {
        //Debug.Log("Collides " + (collides > 0) + ", outside " + outsideBounds + ", invalid Loc " + invalidLocation + ", with location of " + transform.position);
        return collides < 1 && !outsideBounds && !invalidLocation;
    }

    protected void setPreviewColor(bool enabled)
    {
        //bool isEnabled = enabled && canBePlaced();
        //if (isEnabled == lastEnabled) return;
        if (enabled && canBePlaced())
        {
            setColor(enabledColor);
        }
        else
        {
            setColor(disabedColor);
        }
        //lastEnabled = enabled;
    }

    public void setPosition(Vector3 position)
    {
        calculatedPosition = getClosest(position);
        float y = getHighestCollisionPoint();
        if (y < 0f)
        {
            //Debug.Log("invalid");
            invalidLocation = true;
            y = -y;
        }
        else
        {
            //Debug.Log("valid");
            invalidLocation = false;
        }

        transform.position = new Vector3(calculatedPosition.x, y, calculatedPosition.z);
        outsideBounds = transform.position.y > MAX_HEIGHT;
    }

    /**
     * Get highest collision point for the brick  
     * use rayCastRoutine() to implement
     * 
     * return tuple with values: 
     *    true, if the second value represents the value to ADD to the current position
     *    false, if the second value represents the value to SET to the current position
     */
    public abstract float getHighestCollisionPoint();

    protected void rayCastRoutine(Vector3[] points, EvaluateHit evaluator)
    {
        foreach (Vector3 o in points)
        {
            Ray fromTop = new Ray(o, Vector3.down);
            Debug.DrawRay(fromTop.origin, fromTop.direction * (MAX_HEIGHT + OFFSET + 1f), Color.green);
            RaycastHit hit;
            if (Physics.Raycast(fromTop, out hit, MAX_HEIGHT + OFFSET + 1f))
            {
                if (hit.collider.tag.Equals("Lego"))
                {
                    if (!evaluator(hit))
                        break;
                }
            }
        }
    }

    public delegate bool EvaluateHit(RaycastHit hit);

    ///**
    // * Evaluates the hits found by getDefinedRaysOrigins() rays
    // * returns height of the cube, or negative value if invalid location
    // * negative value is the should-be-height, it is used to update the cube value
    // * the negative only means to set the cube to disabled color and then return positive value for the height
    // */
    //public abstract float evaluateHits(List<Tuple<string, Vector3>> hits);

    public abstract Vector3 getClosest(Vector3 point);

    private void changeChildrenLayers(GameObject parent, LayerMask mask)
    {
        parent.gameObject.layer = mask;
        for (int i = 0; i < parent.transform.childCount; i++)
        {
            changeChildrenLayers(parent.transform.GetChild(i).gameObject, mask);
        }
    }

    public abstract int getLengthCount();

    public abstract float getCubeGridCellWith();

    public abstract Vector3[] getDefinedRaysOrigins();

    public abstract void setColor();

    public abstract void setColor(Color toSet);

    public abstract void setOpaque(bool opaque);

    public abstract void enableEmitting(bool enable);

    public abstract float getHeight();

    public abstract Vector3 getRayCastDirection();

    protected abstract GameObject getParent();

    protected abstract void clean();

    protected abstract void onPlacement();

    protected abstract void reddenColor(float ratio);

    public int getValue()
    {
        return value;
    }

    public void rotate(bool clockWise)
    {
        transform.Rotate(0f, clockWise ? -90f : 90f, 0f, Space.Self);
        calculatedRotationMatrix = Matrix4x4.Rotate(transform.rotation);
    }

    public static void flipXaddY(Transform parent, float x, float addY)
    {
        parent.rotation = Quaternion.Euler(x, parent.rotation.y + addY, parent.rotation.z);
    }

    void LateUpdate()
    {

        enableEmitting(Pause.gamePaused());
        if (!isPreview) return;
        //Debug.Log("collides :" + collides + " outsideBounds " + outsideBounds + " invalidLocation " + invalidLocation);           
        setPreviewColor(true); //todo setting color can create the material again and again?  
    }

    public void setPreview()
    {
        isPreview = true;
        trigger.isTrigger = true;
        setOpaque(false);
        Rigidbody body = gameObject.GetComponent<Rigidbody>();
        if (body == null)
        {
            gameObject.AddComponent<Rigidbody>();
            body = gameObject.GetComponent<Rigidbody>();
        }
        body.isKinematic = false;
        body.useGravity = false;
    }

    private static void changeLayers(Transform parent)
    {
        if (parent == null)
            return;
        parent.gameObject.layer = LayerMask.NameToLayer("Shootable");
        for (int i = 0; i < parent.childCount; i++)
        {
            changeLayers(parent.GetChild(i));
        }
    }

    public bool placeSolid()
    {
        if (canBePlaced())
        {
            //Debug.Log("SOLID");
            GameObject created = Instantiate(gameObject, gameObject.transform.position, Quaternion.identity);
            created.gameObject.tag = "LegoAttachable";
            Destroy(created.gameObject.GetComponent<Rigidbody>());
            Lego script = created.GetComponent<Lego>();
            script.isPreview = false;
            changeLayers(created.gameObject.transform); //todo layers dont work will collide
            created.transform.rotation = transform.rotation;

            script.trigger.isTrigger = false;
            script.setColor();
            //Debug.Log("FIN SOLID");

            if (getParent() != null)
            {
                Lego parentScript = getParent().GetComponentInParent<Lego>();
                parentScript.registerDepenency(created.gameObject);
                //todo count parents if more than  set cascade rotated = false
                Transform rotable = parentScript.getRotablePredecessor();
                if (rotable != null) script.setRotablePredecessor(rotable);
                script.lastHit = lastHit;
            }
            script.onPlacement();


            return true;
        }
        //Debug.Log("Cannot place");
        return false;
    }

    protected void OnTriggerEnter(Collider other)
    {
        if (!isPreview) return; //todo if ! ispreview can be damaged by enemies

        if (other.gameObject.layer == LayerMask.NameToLayer("Shootable") || other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            //Debug.Log("trigger entered");
            collides++;
        }
    }

    protected void OnTriggerExit(Collider other)
    {
        if (!isPreview) return;

        if (other.gameObject.layer == LayerMask.NameToLayer("Shootable") || other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (--collides < 1)
            {
                //Debug.Log("trigger exited");
                collides = 0;
            }
        }
    }

}