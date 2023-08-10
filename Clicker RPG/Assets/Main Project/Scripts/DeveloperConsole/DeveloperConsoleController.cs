using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DeveloperConsoleController : MonoBehaviour
{
    [SerializeField]
    private string prefix = string.Empty;
    [SerializeField]
    private ConsoleCommand[] commands = new ConsoleCommand[0];
    private DeveloperConsole developerConsole;

    //
    public GameObject consoleObj;
    public TMP_InputField input;
    public TextMeshProUGUI outputText;
    //

    private int maxQueuedCommands = 10;
    private int browsingIndex = 0;
    private List<string> savedCommands = new List<string>();
    private List<string> savedCommandsReversed = new List<string>();

    private void Awake()
    {
        developerConsole = new DeveloperConsole(prefix, commands);
    }

    private void OnEnable()
    {
        EventManager.Debugging.PushConsoleText += SendConsoleText;
    }

    private void OnDisable()
    {
        EventManager.Debugging.PushConsoleText -= SendConsoleText;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.BackQuote))
        {
            ToggleConsole();
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            browsingIndex = Mathf.Clamp(browsingIndex, 0, savedCommands.Count-1);

            if (savedCommands.Count > 0)
            {
                input.text = savedCommands[browsingIndex];
                input.caretPosition = input.text.Length;
            }

            browsingIndex++;
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            browsingIndex = Mathf.Clamp(browsingIndex, 0, savedCommands.Count-1);

            if (savedCommands.Count > 0)
            {
                input.text = savedCommands[browsingIndex];
                input.caretPosition = input.text.Length;

                foreach (string str in savedCommandsReversed)
                {
                    Debug.Log(str);
                }
            }

            browsingIndex--;
        }
    }

    private void ToggleConsole()
    {

        consoleObj.GetComponent<DevConsoleAnim>().ToggleConsole();
        input.Select();
        input.ActivateInputField();
        input.text = string.Empty;
    }

    public void PushText()
    {
        //Pushing nothing will just reselect the input field.
        if(input.text == string.Empty) 
        {
            input.Select();
            input.ActivateInputField();
            return;
        }
        browsingIndex = 0;
        ProcessCommand(input.text);
        UpdateSavedCommand(input.text);
        input.text = string.Empty;
        input.Select();
        input.ActivateInputField();
    }

    public void ProcessCommand(string input)
    {
        developerConsole.ProcessCommand(input);
    }

    public void SendConsoleText(string text)
    {
        outputText.text += text + "\n";
    }

    private void UpdateSavedCommand(string command)
    {
        if(savedCommands.Count > maxQueuedCommands)
        {
            savedCommands.RemoveAt(savedCommands.Count-1);
        }

        savedCommands.Insert(0, command);
    }

}
