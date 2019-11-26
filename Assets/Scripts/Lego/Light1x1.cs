using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Light1x1 : Lego
{
    private static float halfWidth = 0.21f;
    //private Vector3 p1 = new Vector3(halfWidth, MAX_HEIGHT, 0f);
    //private Vector3 p2 = new Vector3(-halfWidth, MAX_HEIGHT, 0f);

    //private Vector3 p4 = new Vector3(0f, MAX_HEIGHT, -halfWidth);
    //private Vector3 p3 = new Vector3(0f, MAX_HEIGHT, halfWidth);

    private Vector3 middle = new Vector3(0f, MAX_HEIGHT + OFFSET, 0f);

    [SerializeField]
    SkinnedMeshRenderer rend;

    [SerializeField]
    Color color;

    public override int getLengthCount()
    {
        return 1;
    }

    public override float getCubeGridCellWith()
    {
        return 0.63f;
    }

    public override Vector3 getClosest(Vector3 point)
    {
        Vector3 rotated = rotateMinus45 * point;
        Vector3 normalGrid = new Vector3(validateAxis(rotated.x), 0f, validateAxis(rotated.z));
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

        rayCastRoutine(getDefinedRaysOrigins(), delegate (RaycastHit hit) {
            wasHit = true;
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
        rend.material.SetColor("_EmissionColor", color);
        gameObject.AddComponent<Light>();
        Light l = gameObject.GetComponent<Light>();
        l.color = color;
        l.range = 5.5f;
        l.intensity = 3f;
    }

    protected override void reddenColor(float ratio)
    {
        rend.material.color = new Color(color.r + (1f - color.r) * ratio, color.g, color.b);
    }

    public override void enableEmitting(bool enable)
    {
        //do nothing it emmites all the time
    }
}