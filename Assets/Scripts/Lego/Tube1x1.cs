using UnityEngine;

public class Tube1x1 : Lego
{
    private static float halfWidth = 0.21f;

    private Vector3 middle = new Vector3(0f, MAX_HEIGHT + OFFSET, 0f);

    [SerializeField]
    SkinnedMeshRenderer rend;

    [SerializeField]
    Color color;

    private float lastRotationAmount;
    private bool needsRotate = false;

    public override float getCubeGridCellWith()
    {
        return 0.63f;
    }

    public override Vector3 getClosest(Vector3 point)
    {
        Vector3 rotated = rotateMinus45 * point;
        Vector3 normalGrid  = new Vector3(validateAxis(rotated.x), 0f, validateAxis(rotated.z));
        Vector3 shiftedGrid = new Vector3(validateAxis(rotated.x, 0.315f), 0f, validateAxis(rotated.z, 0.315f));

        if (Vector3.Distance(rotated, normalGrid) < Vector3.Distance(shiftedGrid, normalGrid))
        {
            return rotate45 * normalGrid;
        }
        return rotate45 * shiftedGrid;
    }

    public override Vector3[] getDefinedRaysOrigins()
    {
        return new Vector3[] { middle + calculatedPosition };
    }

    private Vector3 getMiddle()
    {
        return middle + transform.position;
    }


    public override float getHighestCollisionPoint()
    {
        float result = 0f;
        bool wasHit = false;
        bool gunHit = false;

        rayCastRoutine(getDefinedRaysOrigins(), delegate (RaycastHit hit) {
            wasHit = true;
            if (hit.collider.name.Equals("GunBody"))
            {
                gunHit = true;
                //Debug.Log("hit gun rotated ");

                needsRotate = lastRotationAmount == 0f;
                calculatedPosition = hit.collider.GetComponentInParent<Gun1x1>().getSidePoint();
                result = calculatedPosition.y;
                lastHit = hit.collider.gameObject;
                return true; //!important
            }

            if (hit.collider.name.Equals("TubeBody"))
            {
                Debug.Log("TUBEE " + hit.collider.GetComponentInParent<Tube1x1>().isYRotated());

                if (hit.collider.GetComponentInParent<Tube1x1>().isYRotated())
                {
                    result = -hit.point.y;
                    lastHit = hit.collider.gameObject;
                    return false;
                } 
            }


            if (hit.collider.name.Equals("TriggerBody"))
            {
                result = (hit.collider.transform.position.x == hit.point.x && hit.collider.transform.position.z == hit.point.z) ? hit.point.y : -hit.point.y;
            }
            else
            {
                result = hit.point.y;
            }
            lastHit = hit.collider.gameObject;
            return true;
        });


        if (!wasHit) lastHit = null;

        if (!gunHit && lastRotationAmount != 0f)
        {
            needsRotate = true;
        }
        if (needsRotate) //reset the rotation 
        {
            if (lastHit != null)
            {
                lastRotationAmount = -90f + lastHit.transform.eulerAngles.y;
                flipXaddY(transform, 90f, lastRotationAmount); 
            }
            else
            {
                flipXaddY(transform, 0f, -lastRotationAmount);
                lastRotationAmount = 0f;
            }
        }
        needsRotate = false;
        return result;
    }

    public override void setColor()
    {
        rend.material.color = color;
        rend.material.SetColor("_EmissionColor", color * 0.6f);
    }

    public override void setColor(Color toSet)
    {
        rend.material.color = toSet;
        rend.material.SetColor("_EmissionColor", toSet * 0.6f);
    }

    public override void setOpaque(bool opaque)
    {
        if (opaque)
        {
            StandardShaderUtils.ChangeRenderMode(rend.material, StandardShaderUtils.BlendMode.Opaque);
            rend.material.DisableKeyword("_EMISSION");
        }
        else
        {
            StandardShaderUtils.ChangeRenderMode(rend.material, StandardShaderUtils.BlendMode.Transparent);
            rend.material.EnableKeyword("_EMISSION");
        }
    }

    public override float getHeight()
    {
        return getCubeGridCellWith() * 2f;
    }

    public override Vector3 getRayCastDirection()
    {
        return Vector3.down;
    }

    protected override GameObject getParent()
    {
        return lastHit;
    }

    protected override void clean()
    {
        lastHit = null;
    }

    protected override void onPlacement()
    {
        setOpaque(true);
        rend.material.SetColor("_EmissionColor", color * 0.2f);
        if (lastHit != null)
        {
            Debug.Log("PLACED");
            Lego script = lastHit.GetComponentInParent<Lego>();
            if (script != null && script.GetType() == typeof(Gun1x1))
            {
                Debug.Log("INCREASED");
                Gun1x1 gun = (Gun1x1)script;
                gun.increaseGunHeadLength();
                gun.addGunHead(gameObject);
            }
        }
    }

    protected override void reddenColor(float ratio)
    {
        rend.material.color = new Color(color.r + (1f - color.r) * ratio, color.g, color.b);
    }

    public override int getLengthCount()
    {
        return 1;
    }

    public override void enableEmitting(bool enable)
    {
        if (enable) rend.material.EnableKeyword("_EMISSION");
        else rend.material.DisableKeyword("_EMISSION");
    }
}
