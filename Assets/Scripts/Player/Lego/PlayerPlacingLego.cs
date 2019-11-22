using UnityEngine;

public class PlayerPlacingLego : MonoBehaviour
{
    [SerializeField]
    private GameObject[] prefabs;
    int count;

    private GameObject lego;
    private Lego script;
    private bool isActive;
    private float timeout;
    private bool scrollIgnored = false;


    private Vector3 mousePreviousPosition;

    void Start()
    {
        //drawTheMatrix();
        isActive = false;
    }

    protected Vector3 getMouseInputMappedToWorld(Vector3 defaultValue)
    {
        Ray screenRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit[] hits;
        hits = Physics.RaycastAll(screenRay, 100.0F);
        for (int i = 0; i < hits.Length; i++)
        {
            RaycastHit hit = hits[i];
            GameObject found = hit.collider.gameObject;
            if (found.tag.Equals("LegoAttachable"))
            {
                return new Vector3(hit.point.x, 0f, hit.point.z);
            }
        }
        return defaultValue;
    }

    protected void FixedUpdate()
    {
        if (!isActive || gameObject == null)
            return;

        handleRotation();

        script.setPosition(getMouseInputMappedToWorld(lego.transform.position));

        if (Input.GetMouseButtonDown(1) && timeout <= 0)
        {
            placeBrick();
        }
        timeout -= Time.deltaTime;
    }

    protected void handleRotation()
    {
        int scroll = (int)Input.mouseScrollDelta.y;
       
        if (scroll < 0)
        {
            if (!scrollIgnored)
            {
                //Debug.Log("scrolled: " + scroll);
                //Debug.Log("scrolled really: " + Input.mouseScrollDelta.y);
                script.rotate(true);
            }
            scrollIgnored = !scrollIgnored;
        }
        else if (scroll > 0)
        {
            if (!scrollIgnored)
            {
                //Debug.Log("scrolled: " + scroll);
                //Debug.Log("scrolled really: " + Input.mouseScrollDelta.y);
                script.rotate(false);
            }
            scrollIgnored = !scrollIgnored;
        }
    }

    protected void placeBrick()
    {
        if (ButtonsManager.getAmount() < script.getValue())
        {
            ButtonsManager.flash();
            return;
        }

        timeout = 0.5f;
        if (script.placeSolid())
        {
            lego.transform.position += Vector3.up * script.getHeight();
            ButtonsManager.spend(script.getValue());
        }
    }

    protected void setPreviewFromTemplate(GameObject o)
    {
        Vector3 pos;
        if (o == null)
        {
            o = prefabs[0];
        }

        if (lego == null)
        {
            pos = new Vector3(0f, 0f, 0f);
        }
        else
        {
            pos = lego.transform.position;
            Destroy(lego);
            lego = null;  
        }
        lego = Instantiate(o, getMouseInputMappedToWorld(pos + Vector3.up * Lego.MAX_HEIGHT), Quaternion.identity);
        script = lego.GetComponent<Lego>();
        script.setPreview();
    }

    public void setActive(bool enabled, int cubePreview = 0)
    {
        isActive = enabled;
        if (!enabled && lego != null)
        {
            Destroy(lego);
        }
        if (enabled)
        {
            setPreviewFromTemplate(prefabs[cubePreview % prefabs.Length]);
        }
    }
}
