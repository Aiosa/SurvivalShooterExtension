using UnityEngine;

public class Cube1x1 : Lego
{
    private Vector3 p1 = new Vector3(0f, MAX_HEIGHT + OFFSET, 0f);

    [SerializeField]
    SkinnedMeshRenderer rend;

    [SerializeField]
    Color color;

    public override float getCubeGridCellWith()
    {
        return 0.63f;
    }

    public override Vector3[] getDefinedRaysOrigins()
    {
        return new Vector3[] { p1 + calculatedPosition };
    }

    public override Vector3 getClosest(Vector3 point)
    {
        Vector3 toNormal = rotateMinus45 * point;
        toNormal = new Vector3(validateAxis(toNormal.x, 0.315f), 0f, validateAxis(toNormal.z, 0.315f));
        return rotate45 * toNormal;
    }


    public override float getHighestCollisionPoint()
    {
        float result = 0f;
        bool invalid = false;
        bool wasHit = false;

        rayCastRoutine(getDefinedRaysOrigins(), delegate (RaycastHit hit) {
            wasHit = true;
            if (hit.collider.name.Equals("TriggerBody"))
            {
                //cube1x1 can be on trigger only in middle, outside of its defined grid
                RaycastHit triggerHit;
                if (Physics.Raycast(new Ray(hit.collider.transform.position + Vector3.up * (MAX_HEIGHT + OFFSET), Vector3.down), out triggerHit))
                {
                    result = triggerHit.point.y;
                    lastHit = hit.collider.gameObject;
                }
                calculatedPosition = hit.collider.transform.position;
                return true;
            }

            if (result < hit.point.y)
            {
                lastHit = hit.collider.gameObject;
                result = hit.point.y;
            }

            return true;
        });
        if (!wasHit) lastHit = null;
        return invalid ? -result : result;
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

    protected override void clean()
    {
        lastHit = null;
    }

    protected override GameObject getParent()
    {
        return lastHit;
    }

    protected override void onPlacement()
    {
        rend.material.SetColor("_EmissionColor", color * 0.2f);
        setOpaque(true);
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
        if (enable)
        {
            rend.material.EnableKeyword("_EMISSION");
        }
        else
        {
            rend.material.DisableKeyword("_EMISSION");
        }
    }
}