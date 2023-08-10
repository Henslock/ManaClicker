using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;

public class SimpleSwipeScript : MonoBehaviour
{
    public Texture alphaBlank;
    public UnitSelectionController uSelectController;
    private RenderTexture renderTexture;
    private Material swipeFXMat;
    private float timeInc = 0f;
    private bool isWiping = false;

    private void Awake()
    {
        swipeFXMat = gameObject.GetComponent<Image>().material;
    }
    private void OnEnable()
    {
        timeInc = 0f;
        if (swipeFXMat != null)
        {
            swipeFXMat.SetFloat("_MainScroll", 0f);
            swipeFXMat.SetTexture("_FakeScreen", alphaBlank);
        }
    }

    public void StartWipeSequence()
    {
        StartCoroutine(WipeSequenceCoroutine());
    }

    private IEnumerator WipeSequenceCoroutine()
    {
        CursorManager.Instance.SetCursorActive(false);
        UpgradeController uController = uSelectController.GetActiveUnitController();
        uController.gameObject.GetComponent<ScrollingBackgroundController>().MoveToOrigin(3f, 0.75f);
        yield return new WaitForSeconds(2f);
        CaptureScreen();
        yield return new WaitForEndOfFrame();
        uSelectController.ResetActiveUnit();
        isWiping = true;
        yield return new WaitForSeconds(1.5f);

        uController.gameObject.GetComponent<ScrollingBackgroundController>().MoveToOrigin(2f, 1.25f);
    }

    public void CaptureScreen()
    {
        renderTexture = new RenderTexture(Screen.width, Screen.height, 0);
        ScreenCapture.CaptureScreenshotIntoRenderTexture(renderTexture);

        swipeFXMat.SetTexture("_FakeScreen", renderTexture);
    }

    private void Update()
    {
        if(!isWiping) { return; }
        timeInc += Time.deltaTime * 0.5f;
        swipeFXMat.SetFloat("_MainScroll", timeInc);

        if(timeInc >= 1.5f)
        {
            CursorManager.Instance.SetCursorActive(true);
            isWiping = false;
            AsyncGPUReadback.Request(renderTexture, 0, TextureFormat.RGBA32, ReadbackCompleted);
            gameObject.SetActive(false);
        }
    }

    void ReadbackCompleted(AsyncGPUReadbackRequest request)
    {
        // Render texture no longer needed, it has been read back.
        DestroyImmediate(renderTexture);
    }

}
