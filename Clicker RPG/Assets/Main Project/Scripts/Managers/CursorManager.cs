using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CursorManager : MonoBehaviour
{
    public static CursorManager Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType(typeof(CursorManager)) as CursorManager;

            return instance;
        }
        set
        {
            instance = value;
        }
    }
    private static CursorManager instance;

    public GameObject cursorObj;
    public GameObject cursorAnchorObj;
    public CanvasGroup cursorCanvas;
    public bool isCursorActive { get; protected set; }

    private bool cursorGraphicActive = true; 

    private float idleTime;

    //If our mouse is idle for this amount of time, fade it away
    public float timeToFade = 3f;

    private void Start()
    {
        isCursorActive = true;

        if (cursorGraphicActive)
        {
            Cursor.visible = false;
            cursorCanvas.alpha = 1.0f;
        }
        else
        {
            Cursor.visible = true;
            cursorCanvas.alpha = 0f;
        }
    }

    public void SetCursorActive(bool state)
    {
        if(state == true)
        {
            cursorGraphicActive = true;
            isCursorActive = true;
            cursorCanvas.alpha = 1.0f;
            Cursor.lockState = CursorLockMode.None;

            cursorAnchorObj.GetComponent<RectTransform>().position = Input.mousePosition;
        }
        else if(state == false)
        {
            cursorGraphicActive = false;
            isCursorActive = false;
            cursorCanvas.alpha = 0f;
            Cursor.lockState = CursorLockMode.Locked;

            cursorAnchorObj.GetComponent<RectTransform>().position = Input.mousePosition;
        }
    }

    private void Update()
    {
        if (cursorGraphicActive && isCursorActive)
        {
            cursorAnchorObj.GetComponent<RectTransform>().position = Input.mousePosition;

            if (Input.GetAxis("Mouse X") == 0 && Input.GetAxis("Mouse Y") == 0)
            {
                //If you are moving your mouse, increment the idle time
                idleTime += Time.deltaTime;
            }
            else
            {
                idleTime = 0f;
                cursorCanvas.alpha = 1.0f;
            }
            if(Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
            {
                //Clicking also resets our idle timer
                idleTime = 0f;
                cursorCanvas.alpha = 1.0f;
            }

            if (Input.GetMouseButtonDown(0))
            {
                cursorObj.GetComponent<RectTransform>().localScale = Vector3.one * 1.25f;
            }

            cursorObj.GetComponent<RectTransform>().localScale = Vector3.Lerp(cursorObj.GetComponent<RectTransform>().localScale, Vector3.one, Time.deltaTime * 5f);

            if (idleTime >= timeToFade)
            {
                cursorCanvas.alpha = Mathf.Lerp(cursorCanvas.alpha, 0f, Time.deltaTime * 4f);
            }
        }
    }

    public List<GameObject> CursorRaycastResults()
    {
        List<GameObject> UIObjects = new List<GameObject>();

        if (!isCursorActive) { return UIObjects; } //Return empty list if the cursor is disabled

        if (EventSystem.current.IsPointerOverGameObject())
        {
            List<RaycastResult> UIObj = HoveringUIElement;
            if (UIObj == null) { return UIObjects; }
            for (int i = 0; i < UIObj.Count; i++)
            {
                //If the list contains a gameobject with the raycast block, we're going to add it and break out of the loop
                //We don't want anything beneath that raycast block
                if (UIObj[i].gameObject.GetComponent<CursorRaycastBlock>())
                {
                    UIObjects.Add(UIObj[i].gameObject);
                    break;
                }
                UIObjects.Add(UIObj[i].gameObject);
            }

            return UIObjects;
        }
        return UIObjects;
    }

    private static List<RaycastResult> HoveringUIElement
    {
        get
        {
            PointerEventData pointerData = new PointerEventData(EventSystem.current)
            {
                pointerId = -1,
            };

            pointerData.position = Input.mousePosition;

            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerData, results);

            if (results.Count > 0)
            {
                if (results[0].gameObject.layer == 5) // 5 is Unity's UI layer
                { return results; }

            }

            return null;
        }
    }
}
