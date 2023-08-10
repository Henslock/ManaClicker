using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ScrollingBackgroundController : MonoBehaviour
{
    public RectTransform upgradeParentRect;
    public RectTransform containerRect;
    public Camera starryCam;
    private Vector3 defaultCamPos = new Vector3();

    public float panningSensitivity = 1.0f;

    public float minZoom = 0.2f;
    public float maxZoom = 1.8f;
    private float currZoom = 1f;
    private float defaultZoomSpeed = 5f;

    private bool isMovingToOrigin = false;
    private float movingSpeed = 1f;

    void Start()
    {
        if(starryCam != null)
        {
            defaultCamPos = starryCam.transform.position;
        }
    }

    void Update()
    {

        if(isMovingToOrigin)
        {
            containerRect.transform.localPosition = Vector3.Lerp(containerRect.transform.localPosition, Vector3.zero, Time.deltaTime * movingSpeed);
            if(Vector3.Distance(containerRect.transform.localPosition, Vector3.zero) <= 3 && Vector3.Distance(containerRect.transform.localScale, new Vector3(currZoom, currZoom, currZoom)) <= 0.02f)
            {
                isMovingToOrigin = false;
                gameObject.GetComponent<ScrollRect>().enabled = true;
            }
        }

        //Scrolling up, ZOOM IN
        if(Input.GetAxis("Mouse ScrollWheel") >0f && UIRaycast() && !isMovingToOrigin)
        {
            currZoom += 0.1f;
            UpdateRectPivot();
        }


        //Scrolling down, ZOOM OUT
        if (Input.GetAxis("Mouse ScrollWheel") < 0f && UIRaycast() && !isMovingToOrigin)
        {
            currZoom -= 0.1f;
            UpdateRectPivot();
        }

        currZoom = Mathf.Clamp(currZoom, minZoom, maxZoom);
        Vector3 currZoomVec = new Vector3(currZoom, currZoom, currZoom);

        if(isMovingToOrigin)
            containerRect.transform.localScale = Vector3.Lerp(containerRect.transform.localScale, currZoomVec, Time.deltaTime * movingSpeed);
        else
            containerRect.transform.localScale = Vector3.Lerp(containerRect.transform.localScale, currZoomVec, Time.deltaTime * defaultZoomSpeed);

        //Move starry background

        ScrollRect scrollRect = gameObject.GetComponent<ScrollRect>();

        float xPivot = gameObject.GetComponent<ScrollRect>().horizontalNormalizedPosition;
        float yPivot = gameObject.GetComponent<ScrollRect>().verticalNormalizedPosition;

        Vector3 offset = new Vector3(xPivot, yPivot, 0);
        offset *= panningSensitivity;
        
        starryCam.transform.position = (defaultCamPos + (offset*0.25f));
    }

    public void MoveToOrigin(float speed)
    {
        isMovingToOrigin = true;
        SetPivot(containerRect, new Vector2(0.5f, 0.5f));

        gameObject.GetComponent<ScrollRect>().enabled = false;
    }

    public void MoveToOrigin(float speed, float zoomLevel)
    {
        movingSpeed = speed;
        currZoom = zoomLevel;
        isMovingToOrigin = true;
        SetPivot(containerRect, new Vector2(0.5f, 0.5f));

        gameObject.GetComponent<ScrollRect>().enabled = false;
    }

    private bool UIRaycast()
    {
        float horizontalMin = upgradeParentRect.offsetMin.x;
        float horizontalMax = (Screen.width + upgradeParentRect.offsetMax.x);

        if (Input.mousePosition.x > horizontalMin && Input.mousePosition.x < horizontalMax)
            return true;


        return false;
    }

    private void UpdateRectPivot()
    {

        float xPivot = Mathf.Clamp(gameObject.GetComponent<ScrollRect>().horizontalNormalizedPosition, 0, 1);
        float yPivot = Mathf.Clamp(gameObject.GetComponent<ScrollRect>().verticalNormalizedPosition, 0, 1);

        Vector2 newPivot = new Vector2(xPivot, yPivot);

        Vector2 size = containerRect.rect.size;
        Vector2 scale = containerRect.localScale;
        Vector2 deltaPivot = containerRect.pivot - newPivot;
        Vector3 deltaPosition = new Vector3(deltaPivot.x * size.x * scale.x, deltaPivot.y * size.y * scale.y);
        containerRect.pivot = newPivot;
        containerRect.localPosition -= deltaPosition;
    }

    private void SetPivot(RectTransform rectTransform, Vector2 pivot)
    {
        Vector3 deltaPosition = rectTransform.pivot - pivot;    // get change in pivot
        deltaPosition.Scale(rectTransform.rect.size);           // apply sizing
        deltaPosition.Scale(rectTransform.localScale);          // apply scaling
        deltaPosition = rectTransform.rotation * deltaPosition; // apply rotation

        rectTransform.pivot = pivot;                            // change the pivot
        rectTransform.localPosition -= deltaPosition;           // reverse the position change
    }
}
