using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;

public class ConsoleUIController : UIController
{
    #region Structs

    public struct Cmd
    {
        public string command;
        public string description;
        public string arguments;
        public int numArguments;
        public Action<string[], int> function;

        public Cmd(string cmd, string desc, string arg, int num, Action<string[], int> fn)
        {
            this.command = cmd;
            this.description = desc;
            this.arguments = arg;
            this.numArguments = num;
            this.function = fn;
        }
    }

    #endregion

    #region Variables

    [Header("Console UI Components")]
    [SerializeField] private Image consoleBackground;
    [SerializeField] private TMP_InputField consoleInputField;

    private Cmd[] commands = {
        new Cmd("help", "Display all commands", "", 0, CmdHelp),
        new Cmd("map", "Load the specified map by name", "<name>", 1, CmdMap)
    };

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
        this.consoleInputField.text = "";
        this.RunCommand(cmd);
        this.consoleInputField.ActivateInputField();
    }

    public void RunCommand(string command)
    {
        Debug.Log($"Running command : {command}");
        this.CmdRun(command);
    }

    #endregion

    #region PrivateMethods

    private void CmdRun(string str)
    {
        string[] args = str.Split();

        string cmdName = args[0];

        foreach (var cmd in this.commands)
        {
            if (cmd.command == cmdName)
            {
                int foundArgs = args.Length - 1;
                int expectedArgs = cmd.numArguments;
                if (foundArgs == expectedArgs)
                {
                    // The start index corresponds to the one of the base command itself, so args[startIndex + 1] is the first argument for this command.
                    cmd.function(args, 0);
                }
                else
                {
                    string foundArgsStr = foundArgs == 1 ? "argument" : "arguments";
                    string expectedArgsStr = expectedArgs == 1 ? "argument" : "arguments";
                    CmdError($"Command \"{cmdName}\" received {foundArgs} {foundArgsStr}, but it takes {expectedArgs} {expectedArgsStr}!");
                    CmdUsage(cmd);
                }
                return;
            }
        }
        CmdError($"Command \"{cmdName}\" could not be found!");
    }

    #endregion

    #region Cmd

    private void CmdPrint(string message)
    {
        Debug.Log(message);
    }

    private void CmdError(string message)
    {
        CmdPrint($"ERROR : {message}");
    }

    private void CmdUsage(Cmd cmd)
    {
        CmdPrint($"Usage : {cmd.command} {cmd.arguments}");
    }

    private static void CmdHelp(string[] args, int startIndex)
    {
        // TODO : Implement
    }

    private static void CmdMap(string[] args, int startIndex)
    {
        string mapname = args[startIndex + 1];
        SceneManager.LoadScene(mapname); // TODO : Add error handling by iterating over the scenes that exist in the build settings.
    }

    #endregion
}
