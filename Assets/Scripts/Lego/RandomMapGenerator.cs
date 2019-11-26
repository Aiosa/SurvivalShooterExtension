using UnityEngine;

public class RandomMapGenerator : MonoBehaviour
{
    [SerializeField]
    private GameObject[] prefabs;

    [SerializeField]
    private int lengthLimit = 20;

    [SerializeField]
    private float mapHalfLength;

    int count = 0;

    private GameObject lego;

    private Vector3[] spawns = new Vector3[]
    {
        new Vector3(5f, 0f, -19f), new Vector3(0f, 0f, 13f), new Vector3(-12f, 0f, -6f), new Vector3(14f, 0f, 1f)
    };


    private void Awake()
    {
        Direction lastD = randomFrom2(Direction.N, Direction.W, 0.6f);
        count = 0;
        foreach (Vector3 spawn in spawns)
        {
            Vector3 adjacent = spawn + toVector(lastD) * 5f *randomizeLength();

            lastD = rightAngle(lastD);
            placeBricks(spawn, lastD, 3, 3);
            placeBricks(spawn, opposite(lastD), 3, 3);
            placeBricks(adjacent, lastD, 3, 1);
            placeBricks(adjacent, opposite(lastD), 3, 3);

            lastD = tryIf(0.3f) ? lastD : rightAngle(lastD);
        }

        Destroy(lego);
    }

    private static bool tryIf(float ratio)
    {
        return Random.Range(0f, 1f) < ratio;
    }
         
    // ratio == 1 -> a, ratio == 0 -> b
    public static T randomFrom2<T>(T a, T b, float ratio)
    {
        return Random.Range(0f, 1f) < ratio ? a : b;
    }

    private Direction randomDirection(Direction from)
    {
        switch (from)
        {
            case Direction.N:
                return randomFrom2(Direction.E, Direction.W, 0.4f);
            case Direction.E:
                return randomFrom2(Direction.S, Direction.N, 0.4f);
            case Direction.W:
                return randomFrom2(Direction.N, Direction.S, 0.4f);
            case Direction.S:
                return randomFrom2(Direction.W, Direction.E, 0.4f);
            default:
                return Direction.N;
        }
    }

    private Lego swapBrickMaybe(float ratio, Vector3 at, Direction d, Lego defaultScript)
    {
        if (tryIf(ratio))
        {
            setPreviewFromTemplate(prefabs[count++ % prefabs.Length], at);
            defaultScript = lego.gameObject.GetComponent<Lego>();
            rotateBrickByDirection(d, defaultScript);
        }
        defaultScript.setPosition(at);
        return defaultScript;
    }


    private void placeBricks(Vector3 origin, Direction direction, int gridLimit, int steps = 3)
    {
        if (steps <= 0) return;
        setPreviewFromTemplate(prefabs[count++ % prefabs.Length], origin);
        Lego script = lego.gameObject.GetComponent<Lego>();

        script.setPosition(origin);
        script.rotate(true);
        rotateBrickByDirection(direction, script);
        int tries = 0;

        Vector3 from = origin;

        while (script.placeSolid() && tries < gridLimit)
        {

            from = from + (toVector(direction) * script.getCubeGridCellWith() * script.getLengthCount() * randomizeLength());

            if (tryIf(0.2f))
            {
                placeBricks(from, randomFrom2(rightAngle(direction), leftAngle(direction), 7f), gridLimit, steps - 1);
            }

            if (tryIf(0.18f) || outsideMap(from))
            {
                return;
            }
            script = swapBrickMaybe(0.4f, from, direction, script);
            tries++;
        }
    }

    private bool outsideMap(Vector3 p)
    {
        return p.x > Lego.getMapHalfLength() || p.x < -Lego.getMapHalfLength() || p.z > Lego.getMapHalfLength() || p.z < -Lego.getMapHalfLength();
    }

    private float randomizeLength()
    {
        if (tryIf(0.1f)) return 0.5f;
        if (tryIf(0.3f)) return 1.5f;
        return 1f;
    }

    private void rotateBrickByDirection(Direction d, Lego script)
    {
        switch (d)
        {
            case Direction.N:
            case Direction.S:
                if (lego.transform.rotation.eulerAngles.y % 180f == 90f)
                    script.rotate(true);
                return;
            case Direction.E:
            case Direction.W:
            default:
                if (lego.transform.rotation.eulerAngles.y % 180f == 0f)
                    script.rotate(true);
                return;
        }

    }

    protected void setPreviewFromTemplate(GameObject o, Vector3 position)
    {
        if (o == null)
        {
            o = prefabs[0];
        }
        if (lego != null)
        {
            Destroy(lego);
        }
        lego = Instantiate(o, position, Quaternion.identity);
        lego.gameObject.GetComponent<Lego>().setPreview();
    }

    private Vector3 toVector(Direction d)
    {
        switch (d)
        {
            case Direction.N:
                return (Vector3.forward + Vector3.right).normalized;
            case Direction.E:
                return (Vector3.right + Vector3.back).normalized;
            case Direction.W:
                return (Vector3.left + Vector3.forward).normalized;
            case Direction.S:
                return (Vector3.back + Vector3.left).normalized;
            default:
                return Vector3.zero;
        }
    }

    private static Direction opposite(Direction d)
    {
         switch(d)
        {
            case Direction.N:
                return Direction.S;
            case Direction.E:
                return Direction.W;
            case Direction.W:
                return Direction.E;
            default:
                return Direction.N;
        }
    }

    private static string named(Direction d)
    {
        switch (d)
        {
            case Direction.N:
                return "NORTH";
            case Direction.E:
                return "EAST";
            case Direction.W:
                return "WEST";
            default:
                return "SOUTH";
        }
    }

    private static Direction rightAngle(Direction d)
    {
        switch (d)
        {
            case Direction.N:
                return Direction.E;
            case Direction.E:
                return Direction.S;
            case Direction.W:
                return Direction.N;
            default:
                return Direction.W;
        }
    }

    private static Direction leftAngle(Direction d)
    {
        return opposite(rightAngle(d));
    }

    private enum Direction
    {
        N, E, W, S
    }
}
