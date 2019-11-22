using UnityEngine;

public class Rotator2x2 : Lego
{
    private Vector3 p1 = new Vector3(0f, MAX_HEIGHT + OFFSET, 0f);
    private Vector3 p2 = new Vector3(0.4454f, MAX_HEIGHT + OFFSET, 0f);
    private Vector3 p3 = new Vector3(-0.4454f, MAX_HEIGHT + OFFSET, 0f);
    private Vector3 p4 = new Vector3(0f, MAX_HEIGHT + OFFSET, 0.4454f);
    private Vector3 p5 = new Vector3(0f, MAX_HEIGHT + OFFSET, -0.4454f);

    [SerializeField]
    private Color discColor;
    [SerializeField]
    private SkinnedMeshRenderer discRenderer;

    [SerializeField]
    private Color cubeColor;
    [SerializeField]
    private SkinnedMeshRenderer cubeRenderer;

    protected override void Start()
    {
        rotablePredecessor = transform;
        base.Start();
    }

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

    public override float getHighestCollisionPoint()
    {
        float result = 0f;
        bool invalid = false;
        bool wasHit = false;

        rayCastRoutine(getDefinedRaysOrigins(), delegate (RaycastHit hit) {
            wasHit = true;
            if (hit.collider.name.Equals("TriggerBody"))
            {
                if (hit.point.x == (p1 + calculatedPosition).x && hit.point.z == (p1 + calculatedPosition).z)
                {
                    result = hit.point.y;
                    invalid = false;
                    lastHit = hit.collider.gameObject;
                    return false;
                }
                else
                {
                    result = hit.point.y;
                    invalid = true;
                    lastHit = hit.collider.gameObject;
                }
            }

            if (result < hit.point.y)
            {
                result = hit.point.y;
                lastHit = hit.collider.gameObject;
            }

            return true;
        });
        if (!wasHit) lastHit = null;
        return invalid ? -result : result;
    }

    public override void setColor()
    {
        discRenderer.material.color = discColor;
        cubeRenderer.material.color = cubeColor;
        cubeRenderer.material.SetColor("_EmissionColor", cubeColor * 0.6f);
        discRenderer.material.SetColor("_EmissionColor", discColor * 0.6f);
    }

    public override void setColor(Color toSet)
    {
        discRenderer.material.color = toSet;
        cubeRenderer.material.color = toSet;
        cubeRenderer.material.SetColor("_EmissionColor", toSet * 0.6f);
        discRenderer.material.SetColor("_EmissionColor", toSet * 0.6f);
    }

    public override void setOpaque(bool opaque)
    {
        if (opaque)
        {
            StandardShaderUtils.ChangeRenderMode(cubeRenderer.material, StandardShaderUtils.BlendMode.Opaque);
            StandardShaderUtils.ChangeRenderMode(discRenderer.material, StandardShaderUtils.BlendMode.Opaque);
            cubeRenderer.material.DisableKeyword("_EMISSION");
            discRenderer.material.DisableKeyword("_EMISSION");

        }
        else
        {
            StandardShaderUtils.ChangeRenderMode(cubeRenderer.material, StandardShaderUtils.BlendMode.Transparent);
            StandardShaderUtils.ChangeRenderMode(discRenderer.material, StandardShaderUtils.BlendMode.Transparent);
            cubeRenderer.material.EnableKeyword("_EMISSION");
            discRenderer.material.EnableKeyword("_EMISSION");
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
        GetComponent<Rotable>().enable();
        cubeRenderer.material.SetColor("_EmissionColor", cubeColor * 0.2f);
        discRenderer.material.SetColor("_EmissionColor", discColor * 0.2f);
    }

    protected override void reddenColor(float ratio)
    {
        cubeRenderer.material.color = new Color(cubeColor.r + (1f - cubeColor.r) * ratio, cubeColor.g, cubeColor.b);
        discRenderer.material.color = new Color(discColor.r + (1f - discColor.r) * ratio, discColor.g, discColor.b);
    }

    public override int getLengthCount()
    {
        return 2;
    }

    public override void enableEmitting(bool enable)
    {
        if (enable)
        {
            cubeRenderer.material.EnableKeyword("_EMISSION");
            discRenderer.material.EnableKeyword("_EMISSION");
        }
        else
        { 
            
            cubeRenderer.material.DisableKeyword("_EMISSION");
            discRenderer.material.DisableKeyword("_EMISSION");
        }
    }
}
