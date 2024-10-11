using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ConsoleUIController : UIController
{
    #region Variables

    [Header("Console UI Components")]
    [SerializeField] private Image consoleBackground;
    [SerializeField] private TMP_InputField consoleInputField;

    #endregion

    #region MonoBehaviour
    
    void Start()
    {
    }

    void Update()
    {
    }

    #endregion

    #region PublicMethods

    public void RunCommandWrapper()
    {
        string cmd = this.consoleInputField.text;
        this.RunCommand(cmd);
    }

    public void RunCommand(string command)
    {
        Debug.Log($"Running command : {command}");
    }

    #endregion

    #region PrivateMethods
    #endregion
}
