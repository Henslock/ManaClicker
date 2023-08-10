using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevConsoleAnim : MonoBehaviour
{
    public GameObject consoleContainer;
    private bool consoleToggle = false;
    private bool inputWorking = true;
    private void OnEnable()
    {
        gameObject.GetComponent<CanvasGroup>().alpha = 0f;
        consoleContainer.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, -500, 0);
    }

    public void ToggleConsole()
    {
        if (consoleToggle == false)
            ShowConsole();
        else
            HideConsole();
    }

    public void ShowConsole()
    {
        if (inputWorking == false) { return; }

        consoleToggle = true;
        gameObject.SetActive(true);
        CursorManager.Instance.SetCursorActive(false);
    }

    public void HideConsole()
    {
        if(inputWorking == false) { return; }
        consoleToggle = false;
        inputWorking = false;
        StartCoroutine(IFadeConsoleAway());
    }

    private void Update()
    {
        if(consoleToggle)
        {
            gameObject.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(gameObject.GetComponent<CanvasGroup>().alpha, 1f, Time.deltaTime * 8f);
            consoleContainer.GetComponent<RectTransform>().anchoredPosition = Vector3.Lerp(consoleContainer.GetComponent<RectTransform>().anchoredPosition, Vector3.zero, Time.deltaTime * 8f);
        }
        else
        {
            gameObject.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(gameObject.GetComponent<CanvasGroup>().alpha, 0f, Time.deltaTime * 8f);
            consoleContainer.GetComponent<RectTransform>().anchoredPosition = Vector3.Lerp(consoleContainer.GetComponent<RectTransform>().anchoredPosition, new Vector3(0, -500, 0), Time.deltaTime * 8f);
        }
    }

    private IEnumerator IFadeConsoleAway()
    {
        yield return new WaitForSeconds(0.3f);

        CursorManager.Instance.SetCursorActive(true);
        inputWorking = true;

        gameObject.SetActive(false);
    }
}
