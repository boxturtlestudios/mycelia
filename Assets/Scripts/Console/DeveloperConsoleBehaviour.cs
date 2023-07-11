using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class DeveloperConsoleBehaviour : MonoBehaviour
{
    public static DeveloperConsoleBehaviour Instance { get; private set; }
    private void Awake() 
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Debug.LogAssertion("Duplicate DeveloperConsoleBehaviour destroyed!");
            Destroy(gameObject);
        }
    }

    private DeveloperConsole developerConsole;
    private DeveloperConsole DeveloperConsole
    {
        get
        {
            if (developerConsole != null) { return developerConsole; }
            return developerConsole = new DeveloperConsole(prefix, commands);
        }
    }

    private float timeScaleCache;
    public bool devEnabled { get; private set; }

    [SerializeField]
    private string prefix = string.Empty;
    [SerializeField]
    private ConsoleCommand[] commands = new ConsoleCommand[0];

    [Header("UI Elements")]
    [SerializeField]
    private GameObject uiCanvas = null;
    [SerializeField]
    private TMP_InputField inputField = null;

    [SerializeField]
    private MyceliaInputActions inputActions;


    public void Toggle(InputAction.CallbackContext context)
    {
        if(!context.action.triggered) { return; }
        
        if(uiCanvas.activeSelf)
        {
            Time.timeScale = timeScaleCache;
            uiCanvas.SetActive(false);
            devEnabled = false;
        }
        else
        {
            devEnabled = true;
            timeScaleCache = Time.timeScale;
            Time.timeScale = 0;
            uiCanvas.SetActive(true);
            inputField.ActivateInputField();
        }
    }

    public void ProcessCommand(string input)
    {
        CommandReturn result = DeveloperConsole.ProcessCommand(input);
        if (!string.IsNullOrEmpty(result.message))
        {
            Debug.Log(result.message);
        }
        
        inputField.text = string.Empty;
        inputField.ActivateInputField();
    }

}