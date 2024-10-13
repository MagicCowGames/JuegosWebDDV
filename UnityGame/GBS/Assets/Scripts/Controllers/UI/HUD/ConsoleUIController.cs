using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;
using System.IO;

// NOTE : Maybe the console's backend should be a static class, cause right now the entire console system is handled by the UIController class, and it doesn't feel
// right tbh. I mean, it works, and allows for infinitely many consoles to run their own logic and stuff, but after all, the logic is just the same everywhere for most
// operations... idk, we'll see, for now this works great and actually allows the console printing to be pretty straight forward, so it might be best to leave it as is.
public class ConsoleUIController : UIController
{
    #region Structs

    public class Cmd
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
    [SerializeField] private TMP_Text consoleText;

    private Cmd[] commands;

    #endregion

    #region MonoBehaviour
    
    void Start()
    {
        // We instantiate it here rather than assigning it on the variable declaration because we want the methods to be non static so that they can reference
        // The console instance's text for CmdPrint to work without having to go through the UI Manager to ask for permission.
        // Basically, we're doing this shit in case we want to have infinite consoles in the future, and because we don't want to have daddy issues and rely
        // on some global fucking manager to babysit us when we can just access to our own resources directly without blowing a fuse.
        this.commands = new Cmd[] {
            new Cmd("help", "Display all commands", "", 0, CmdHelp),
            new Cmd("map", "Load the specified map by name", "<name>", 1, CmdMap),
            new Cmd("quit", "Return to main menu", "", 0, CmdQuit),
            new Cmd("iamvip", "Show the credits menu", "", 0, CmdIAmVip),
            new Cmd("clear", "Clear the console", "", 0, CmdClear),
            new Cmd("maplist", "Display a list of all of the available maps", "", 0, CmdMapList),
            new Cmd("delete", "Removes the specified GameObject", "<name>", 1, CmdDelete)
        };

        this.SetConsoleOpen(false);
        RegisterEvents();
    }

    void Update()
    {
    }

    void OnDestroy()
    {
        UnregisterEvents();
    }

    #endregion

    #region PublicMethods

    public void RunCommandWrapper()
    {
        string cmd = this.consoleInputField.text;
        this.consoleInputField.text = "";
        this.RunCommand(cmd);
        this.SelectConsoleInputField();
    }

    public void RunCommand(string command)
    {
        string str = command.Trim(); // Trim whitespace on the left and right sides of the input command string.
        Debug.Log($"Running command : {str}");
        if(str.Length > 0) // only run the command if it contains at least 1 single character.
            this.CmdRun(str);
        // With the trimming and length check, commands that are entirely made up of whitespace will be ignored, preventing errors from being displayed when pressing enter with no commands written on the input field.
    }

    #endregion

    #region PrivateMethods

    private void RegisterEvents()
    {
        if (InputManager.Instance == null)
            return;

        InputManager.Instance.OnSwitchConsole += SwitchConsole;
    }

    private void UnregisterEvents()
    {
        if (InputManager.Instance == null)
            return;

        InputManager.Instance.OnSwitchConsole -= SwitchConsole;
    }

    private void SwitchConsole()
    {
        SetConsoleOpen(!GetConsoleOpen());
    }

    private void SelectConsoleInputField()
    {
        this.consoleInputField.ActivateInputField();
    }

    private void SetConsoleOpen(bool isOpen)
    {
        // The 'º' character is added to the input field when closing the console.
        // When the console is closed, the input field is flushed, thus it would run the command string "º"
        // This is why we set the text field to an empty string before closing or opening the console.
        this.consoleInputField.text = "";
        this.UI_SetVisible(!this.UI_GetVisible());

        this.SelectConsoleInputField(); // always select the input field when the console is opened to make it faster to type in commands.
    }

    private bool GetConsoleOpen()
    {
        return this.UI_GetVisible();
    }

    private void CmdRun(string str)
    {
        CmdPrint($"> {str}"); // Always print first the command that we just ran, doesn't matter if it works correctly or not. Even if it's just trash, print it out to the console so that the user can see what they typed before.

        string[] args = str.Split();

        string cmdName = args[0].ToLower(); // Bring to lower case to allow commands to work regardless of whether they are capitalized or not.
        // NOTE : Lowercase works for map names because Unity does not give a fuck about capitalization for that stuff...
        // BUT, Unity DOES give a fuck about other stuff being property capitalized.
        // That's the reason why we only pass to lower the cmd and we don't do the same for the rest of the string, because some other parts would be case sensitive.
        // Also because that would fuck the echo command.

        foreach (var cmd in this.commands)
        {
            // We pass the cmd.command string to lowercase too, even tho this should not be needed because they should be in lowercase anyways
            // from the very moment they are stored on the commands array, but just in case a typo is made at some point...
            if (cmd.command.ToLower() == cmdName)
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

    #region Cmd - Print

    // Print a line to the console
    private void CmdPrint(string message)
    {
        DebugManager.Instance?.Log(message);
        this.consoleText.text += $"{message}\n";
    }

    // Clear the entire console text
    private void CmdClear(string[] args, int startIndex)
    {
        this.consoleText.text = "";
    }

    // Print a line to the console to display an error message
    private void CmdError(string message)
    {
        CmdPrint($"ERROR : {message}");
    }

    // Print a line to the console to display the usage of a command
    private void CmdUsage(Cmd cmd)
    {
        CmdPrint($"Usage : {cmd.command} {cmd.arguments}");
    }

    // Print the help message to the console
    private void CmdHelp(string[] args, int startIndex)
    {
        CmdPrint("Commands:");
        foreach (var cmd in this.commands)
            CmdPrint($" {cmd.command} {cmd.arguments} : {cmd.description}");
    }

    #endregion

    #region Cmd - Other

    private void CmdMap(string[] args, int startIndex)
    {
        string mapname = args[startIndex + 1];
        // TODO : Add error handling by iterating over the scenes that exist in the build settings to report when user attemps to load map that does not exist.
        SceneLoadingManager.Instance?.LoadScene(mapname);
    }

    private void CmdQuit(string[] args, int startIndex)
    {
        SceneLoadingManager.Instance?.LoadSceneMainMenu();
    }

    private void CmdIAmVip(string[] args, int startIndex)
    {
        SceneLoadingManager.Instance?.LoadSceneCredits();
    }

    private void CmdMapList(string[] args, int startIndex)
    {
        CmdPrint("Maps:");
        // NOTE : We avoid using the unity editor build settings API because that's for editor-level code, which can be cool to make tools, but that dll is not
        // packaged in builds for users, so that wont work. Instead, we can use the SceneUtility thing. Also, can't use Scene.name because for some dumb reason
        // they thought it would make more sense for all Scene structs to contain their own data and... a name field which references the currently loaded scene,
        // rather than the scene's own name. Bruh moment indeed, but it is what it is.
        for(int i = 0; i < SceneManager.sceneCountInBuildSettings; ++i)
        {
            string sceneName = Path.GetFileName(SceneUtility.GetScenePathByBuildIndex(i));
            CmdPrint($" {sceneName}");
        }
    }

    private void CmdDelete(string[] args, int startIndex)
    {
        string name = args[startIndex + 1];
        var obj = GameObject.Find(name);
        
        if (obj == null)
        {
            CmdError($"No GameObject named \"{name}\" could be found!");
        }
        else
        {
            CmdPrint($"Removing GameObject named \"{name}\"");
            GameObject.Destroy(obj, 0.0f);
        }
    }

    #endregion
}
