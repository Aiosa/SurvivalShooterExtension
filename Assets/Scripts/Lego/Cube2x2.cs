using System;
using UnityEngine;

public class Cube2x2 : Lego
{
    private Vector3 p1 = new Vector3(0f, MAX_HEIGHT + 5f, 0f);
    private Vector3 p2 = new Vector3(0.4454f, MAX_HEIGHT + 5f, 0f);
    private Vector3 p3 = new Vector3(-0.4454f, MAX_HEIGHT + 5f, 0f);
    private Vector3 p4 = new Vector3(0f, MAX_HEIGHT + 5f, 0.4454f);
    private Vector3 p5 = new Vector3(0f, MAX_HEIGHT + 5f, -0.4454f);

    [SerializeField]
    SkinnedMeshRenderer rend;

    [SerializeField]
    Color color;

    public override float getCubeGridCellWith()
    {
        return 0.63f;
    }


    public override Vector3 getClosest(Vector3 point)
    {
        Vector3 toNormal = rotateMinus45 * point;
        toNormal = new Vector3(validateAxis(toNormal.x), 0f, validateAxis(toNormal.z));

        return rotate45 * toNormal;
    }

    public override Vector3[] getDefinedRaysOrigins()
    {
        return new Vector3[] { p1 + calculatedPosition, p2 + calculatedPosition, p3 + calculatedPosition, p4 + calculatedPosition, p5 + calculatedPosition };
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

    public override void enableEmitting(bool enable)
    {
        if (enable) rend.material.EnableKeyword("_EMISSION");
        else rend.material.DisableKeyword("_EMISSION");
    }

    public override float getHeight()
    {
        return getCubeGridCellWith() * 2f;
    }

    public override float getHighestCollisionPoint()
    {
        float result = 0f;
        int counter = 0;
        bool invalid = false;


        bool wasHit = false;

        rayCastRoutine(getDefinedRaysOrigins(), delegate (RaycastHit hit) {
            wasHit = true;

            if (hit.collider.name.Equals("TriggerBody"))
            {
                lastHit = hit.collider.gameObject;
                if (hit.point.x == (p1 + calculatedPosition).x && hit.point.z == (p1 + calculatedPosition).z)
                {
                    result = hit.point.y;
                    invalid = false;
                    return false;
                }
                else
                {
                    result = hit.point.y;
                    invalid = true;
                }
            }

            if (result < hit.point.y)
            {
                lastHit = hit.collider.gameObject;
                result = hit.point.y;
                counter = 1;
            }
            else if (result == hit.point.y)
            {
                counter++;
            }
            return true;
        });
        if (!wasHit)
        {
            lastHit = null;
        }

        //Debug.Log(counter);
        return invalid ? -result : result;
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
    }

    protected override void reddenColor(float ratio)
    {
        rend.material.color = new Color(color.r + (1f - color.r) * ratio, color.g, color.b);
    }

    public override int getLengthCount()
    {
        return 2;
    }
}