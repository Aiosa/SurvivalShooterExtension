using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UnityEngine.EventSystems;

public class UIFocus : MonoBehaviour
{
    private int selectedCube;
    private Image center;

    private bool dragging = false;
    private bool clicking = false;
    // Start is called before the first frame update

    [SerializeField]
    private GameObject MainButton;

    [SerializeField]
    private Sprite mainSprite;

    [SerializeField]
    private GameObject[] Buttons;

    [SerializeField]
    private Sprite[] sprites;

    [SerializeField]
    private PlayerPlacingLego placer;

    GraphicRaycaster m_Raycaster;
    PointerEventData m_PointerEventData;
    EventSystem m_EventSystem;
    void Start()
    {
       // center = GetComponent<Image>();
        //Fetch the Raycaster from the GameObject (the Canvas)
        m_Raycaster = GetComponent<GraphicRaycaster>();
        //Fetch the Event System from the Scene
        m_EventSystem = GetComponent<EventSystem>();
    }

    private void setEnabledButtons(bool enabled)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(enabled);
        }
    }


    // Update is called once per frame
    void Update()
    {
        //Check if the left Mouse button is clicked
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            m_PointerEventData = new PointerEventData(m_EventSystem);
            m_PointerEventData.position = Input.mousePosition;
            List<RaycastResult> results = new List<RaycastResult>();
            m_Raycaster.Raycast(m_PointerEventData, results);


            foreach (RaycastResult result in results)
            {
                if (result.gameObject.name.Equals(MainButton.name))
                {
                    if (dragging)
                        setEnabledAll(false);
                    else
                        setEnabledAll(true);
                    dragging = true;
                    return;
                }
            }
            if (!dragging)
            {
                Pause.setPaused(false);
                MainButton.GetComponent<Image>().sprite = mainSprite;
            }
            placer.setActive(false);
        }
        else if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            if (!dragging)
                return;

            m_PointerEventData = new PointerEventData(m_EventSystem);
            m_PointerEventData.position = Input.mousePosition;
            List<RaycastResult> results = new List<RaycastResult>();
            m_Raycaster.Raycast(m_PointerEventData, results);

            foreach (RaycastResult result in results)
            {


                foreach (GameObject b in Buttons)
                {
                    if (result.gameObject.active && result.gameObject.name.Equals(b.name))
                    {
                        SelectToolButton selection = result.gameObject.GetComponent<SelectToolButton>();
                        selectedCube = selection.get();
                        placer.setActive(true, selectedCube);
                        MainButton.GetComponent<Image>().sprite = sprites[selectedCube % sprites.Length];

                        dragging = false;
                        setEnabledAll(false);
                        Pause.setPaused(true);
                    }
                }
            }
            dragging = false;
            setEnabledAll(false);
        }
    }

    private void setEnabledAll(bool enabled)
    {
        foreach (GameObject b in Buttons)
        {
            b.SetActive(enabled);
        }
    }
}

