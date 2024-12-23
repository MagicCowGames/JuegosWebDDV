using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;
using System.IO;
using System.Globalization;

// NOTE : Maybe the console's backend should be a static class, cause right now the entire console system is handled by the UIController class, and it doesn't feel
// right tbh. I mean, it works, and allows for infinitely many consoles to run their own logic and stuff, but after all, the logic is just the same everywhere for most
// operations... idk, we'll see, for now this works great and actually allows the console printing to be pretty straight forward, so it might be best to leave it as is.

// NOTE : Or, maybe we should have a CommandManager class that handles all of the commands logic globally, and then we can configure it there, add aliases, etc...
// so then the ConsoleUIController instances are the ones that handle the logic of specific console windows and access the actual command logic through the manager.

public class ConsoleUIController : UIController
{
    #region Structs

    // TODO : Clean this shit up since it's not really all that usefull... or maybe clean the rest of the code to adapt to it, cause it COULD be useful lol.
    public enum CmdErrorType
    {
        Default = 0,
        ArgType,
        ArgUnexpected
    }

    public class Cmd
    {
        public string command;
        public string description;
        public string arguments;
        public int numArguments;
        public Action<string[], int> function;
        public bool isCheat;

        public Cmd(string cmd, string desc, string arg, int num, Action<string[], int> fn, bool cheat = false)
        {
            this.command = cmd;
            this.description = desc;
            this.arguments = arg;
            this.numArguments = num;
            this.function = fn;
            this.isCheat = cheat;
        }
    }

    #endregion

    #region Variables

    [Header("Console UI Components")]
    [SerializeField] private Image consoleBackground;
    [SerializeField] private TMP_InputField consoleInputField;
    [SerializeField] private TMP_Text consoleText;
    [SerializeField] private ScrollRect consoleScrollRect;

    private Cmd[] commands;

    private bool cheatsEnabled = false; // TODO : Move this to a game data manager so that it can be globally accessed. Maybe also move the stuff in GameUtility for pausing to said manager. Obviously make its name clearly different enough from the GameManager, which should be the one in charge of managing matches, or at least that's the plan for now. Also, having a single CheatManager just for this purpose sounds too far fetched, but maybe makes sense once you factor in all the code required to make sure that cheats have not been enabled when passing data to the scoreboard server, etc...
    private bool consoleEnabled = false;

    public bool CheatsEnabled { get { return this.cheatsEnabled; } set { this.cheatsEnabled = value; } }
    public bool ConsoleEnabled { get { return this.consoleEnabled; } set { this.consoleEnabled = value; } }

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
            new Cmd("cls", "Clear the console", "", 0, CmdClear),
            new Cmd("maplist", "Display a list of all of the available maps", "", 0, CmdMapList),
            new Cmd("delete", "Removes the specified GameObject", "<name>", 1, CmdDelete, true),
            new Cmd("debug", "Enable or disable debug logging and visualization", "<enabled>", 1, CmdDebug),
            new Cmd("info", "Display information about a given category", "<category>", 1, CmdInfo),
            new Cmd("sethealth", "Set the health of the player to the specified value", "<value>", 1, CmdSetHealth, true),
            new Cmd("heal", "Set the health of the player to the max value", "", 0, CmdHeal, true),
            new Cmd("color_fg", "Set the color of the foreground", "<red> <green> <blue>", 3, CmdColorFG),
            new Cmd("color_bg", "Set the color of the background", "<red> <green> <blue>", 3, CmdColorBG),
            new Cmd("cheats", "Enable or disable cheats", "<enabled>", 1, CmdCheats, false),
            new Cmd("lang", "Set the current language", "<language>", 1, CmdLang),
            new Cmd("langlist", "Display a list of all the available languages", "", 0, CmdLangList),
            new Cmd("popmeup", "Display a popup on screen with a funny message", "", 0, CmdPopMeUp),
            new Cmd("popmeup2", "Display a popup on screen with the specified title and message", "<title> <message>", 2, CmdPopMeUp2),
            new Cmd("timescale", "Change the game's timescale", "<scale>", 1, CmdTimeScale),
            new Cmd("showfps", "Change the visibility of the FPS display", "<display>", 1, CmdShowFps),
            new Cmd("gtfo", "Instantly finish the current level", "", 0, CmdGtfo, true),
            new Cmd("mkreq", "Make an HTTP request", "<type> <url> <message>", 3, CmdMkReq),
            new Cmd("triggermeelmo", "Enable or disable player trigger activation", "<enabled>", 1, CmdTriggerMeElmo, true),
            new Cmd("leavemealone", "Make all NPCs forget about the player", "", 0, CmdLeaveMeAlone, true),
            new Cmd("volume", "Set the global volume of the application", "<volume>", 1, CmdVolumeGlobal),
            new Cmd("volumesfx", "Set the SFX volume", "<volume>", 1, CmdVolumeSFX),
            new Cmd("volumemusic", "Set the music volume", "<volume>", 1, CmdVolumeMusic),
            new Cmd("volumevoice", "Set the voice volume", "<volume>", 1, CmdVolumeVoice),
            new Cmd("volumeui", "Set the UI volume", "<volume>", 1, CmdVolumeUI),
            new Cmd("playsound", "Play an SFX sound", "<name>", 1, CmdPlaySound),
            // TODO : Add commands to get the current map name and the current language or something...
        };
        // TODO : Make an alias system of sorts, or maybe make it so that we can have a dict / list system to have multiple overloads for the same command
        // with different parameters (eg: tp <pos>, tp <name> <pos>, tp <name> <target>, etc...) or different cmd names for the same underlying cmd (eg: clear and cls)

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
        this.ScrollConsoleToBottom(); // NOTE : Even just pressing enter with an empty string will scroll to the bottom of the console's text, which is the intended behaviour and is just the way I want it to work.
    }

    public void RunCommand(string command)
    {
        string str = command.Trim(); // Trim whitespace on the left and right sides of the input command string.
        // DebugManager.Instance?.Log($"Running command : {str}");
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
        SetConsoleOpen(this.consoleEnabled ? !GetConsoleOpen() : false);
    }

    private void SelectConsoleInputField()
    {
        this.consoleInputField.ActivateInputField();
    }

    private void ScrollConsoleToBottom()
    {
        this.consoleScrollRect.verticalNormalizedPosition = 0;
    }

    private void SetConsoleOpen(bool isOpen)
    {
        // The '�' character is added to the input field when closing the console.
        // When the console is closed, the input field is flushed, thus it would run the command string "�"
        // This is why we set the text field to an empty string before closing or opening the console.
        this.consoleInputField.text = "";
        if (isOpen)
        {
            this.UI_SetVisible(true);
            this.SelectConsoleInputField(); // always select the input field when the console is opened to make it faster to type in commands.
        }
        else
        {
            this.UI_SetVisible(false);
        }
    }

    private bool GetConsoleOpen()
    {
        return this.UI_GetVisible();
    }

    private void CmdRun(string str)
    {
        CmdPrintln($"> {str}"); // Always print first the command that we just ran, doesn't matter if it works correctly or not. Even if it's just trash, print it out to the console so that the user can see what they typed before.

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
                    if (!cmd.isCheat || (cmd.isCheat && this.cheatsEnabled))
                        cmd.function(args, 0); // The start index corresponds to the one of the base command itself, so args[startIndex + 1] is the first argument for this command.
                    else
                        CmdErrorCheats();
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

    // Print a message to the console
    public void CmdPrint(string message = "")
    {
        // This is a fucking hack, and if you look at the implementation within DebugManager you will understand why...
        // a fucking intertwined and mangled piece of shit of coupled calls between this controller instance and the DebugManager,
        // having to expose internal functions to the public interface, etc etc...
        // A more logical solution would be creating a .ConsoleLog() function within the DebugManager and enforcing its use instead of the regular Log,
        // or adding a parameter to .Log() to determine whether you want the output to appear on the console or not... for now, this works flawlessly, but
        // it makes me uncomfortable...
        // Also logs on loops will become annoying as fuck, maybe I should implement some debug level command rather than it being a boolean value...
        // It would still act as a global kill switch setting it to 0, so yeah.
        DebugManager.Instance?.LogInternal(message);
        this.consoleText.text += $"{message}";
    }

    // Print a line to the console
    public void CmdPrintln(string message = "")
    {
        CmdPrint($"{message}\n");
    }

    // Clear the entire console text
    private void CmdClear(string[] args, int startIndex)
    {
        this.consoleText.text = "";
    }

    // Print a line to the console to display an error message
    private void CmdError(string message = "", CmdErrorType type = CmdErrorType.Default)
    {
        string msgBase = CmdGetColorString("ERROR", Color.red) + " : ";
        string msgBody = "";
        switch(type)
        {
            default:
            case CmdErrorType.Default:
                msgBody = $"{message}";
                break;
            case CmdErrorType.ArgType:
                msgBody = $"Argument \"{message}\" has incorrect type";
                break;
            case CmdErrorType.ArgUnexpected:
                msgBody = $"Unexpected argument \"{message}\"";
                break;
        }
        CmdPrintln($"{msgBase}{msgBody}");
    }

    private void CmdErrorCheats()
    {
        CmdError("Cheats are not enabled!");
    }

    // Print a line to the console to display the usage of a command
    private void CmdUsage(Cmd cmd)
    {
        CmdPrintln($"Usage : {cmd.command} {cmd.arguments}");
    }

    // Print the help message to the console
    private void CmdHelp(string[] args, int startIndex)
    {
        CmdPrintln("Commands:");
        foreach (var cmd in this.commands)
            CmdPrintln($" {cmd.command} {cmd.arguments} : {cmd.description}");
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
        CmdPrintln("Maps:");
        // NOTE : We avoid using the unity editor build settings API because that's for editor-level code, which can be cool to make tools, but that dll is not
        // packaged in builds for users, so that wont work. Instead, we can use the SceneUtility thing. Also, can't use Scene.name because for some dumb reason
        // they thought it would make more sense for all Scene structs to contain their own data and... a name field which references the currently loaded scene,
        // rather than the scene's own name. Bruh moment indeed, but it is what it is.
        for(int i = 0; i < SceneManager.sceneCountInBuildSettings; ++i)
        {
            string sceneName = Path.GetFileName(SceneUtility.GetScenePathByBuildIndex(i));
            CmdPrintln($" {sceneName}");
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
            CmdPrintln($"Removing GameObject named \"{name}\"");
            GameObject.Destroy(obj, 0.0f);
        }
    }

    private void CmdDebug(string[] args, int startIndex)
    {
        string arg = args[startIndex + 1];
        bool b = CmdParseBool(arg);
        CmdPrintln($"Debug Logging Enabled : {b}");
        DebugManager.Instance?.SetDebugEnabled(b);
    }

    private void CmdInfo(string[] args, int startIndex)
    {
        string arg = args[startIndex + 1];
        switch (arg.ToLower())
        {
            case "elements":
                CmdPrint("Elements : [ ");
                for (int i = 0; i < (int)Element.COUNT; ++i)
                    CmdPrint($"{(Element)i} ");
                CmdPrintln("]");

                for (int i = 0; i < (int)Element.COUNT; ++i)
                {
                    var element = (Element)i;
                    var opposites = MagicManager.Instance?.GetCombinableElements(element, 0);
                    CmdPrint($"opp({element}) : [ ");
                    foreach(var opp in opposites)
                        CmdPrint($"{opp} ");
                    CmdPrintln("]");
                }

                break;
            case "npc":
                CmdPrintln("No NPC info yet.");
                break;
            default:
                CmdError($"Unknown category \"{arg}\"");
                break;
        }
    }

    private void CmdSetHealth(string[] args, int startIndex)
    {
        string arg = args[startIndex + 1];
        float hp = CmdParseFloat(arg);
        PlayerDataManager.Instance?.GetPlayerHealth()?.ForceSetHealth(hp);
    }

    private void CmdHeal(string[] args, int startIndex)
    {
        PlayerDataManager.Instance?.GetPlayerHealth()?.Heal();
    }

    private void CmdColorFG(string[] args, int startIndex)
    {
        var rstr = args[startIndex + 1];
        var gstr = args[startIndex + 2];
        var bstr = args[startIndex + 3];

        float r = CmdParseFloat(rstr);
        float g = CmdParseFloat(gstr);
        float b = CmdParseFloat(bstr);
        float a = 1.0f;

        this.consoleText.color = new Color(r, g, b, a);
    }

    private void CmdColorBG(string[] args, int startIndex)
    {
        var rstr = args[startIndex + 1];
        var gstr = args[startIndex + 2];
        var bstr = args[startIndex + 3];

        float r = CmdParseFloat(rstr);
        float g = CmdParseFloat(gstr);
        float b = CmdParseFloat(bstr);
        float a = this.consoleBackground.color.a;

        this.consoleBackground.color = new Color(r, g, b, a);
    }

    private void CmdCheats(string[] args, int startIndex)
    {
        var arg = args[startIndex + 1];
        this.cheatsEnabled = CmdParseBool(arg);
        CmdPrintln($"Cheats have been {(this.cheatsEnabled ? CmdGetColorString("Enabled", Color.green) : CmdGetColorString("Disabled", Color.red))}!");
    }

    private void CmdLang(string[] args, int startIndex)
    {
        var arg = args[startIndex + 1];
        LanguageSystem.SetLanguage(arg);
        SettingsManager.Instance.SaveSettings(); // NOTE : Cmds ALWAYS save commands on execution because it's easier to implement like that, unlike the settings menu which saves it in a single packet when the user leaves the menu.
        CmdPrintln($"Language set to \"{LanguageSystem.GetLanguage()}\"");
    }

    private void CmdLangList(string[] args, int startIndex)
    {
        CmdPrint("Languages : [ ");
        for (int i = 0; i < (int)LanguageSystem.Language.COUNT; ++i)
            CmdPrint($"{(LanguageSystem.Language)i} ");
        CmdPrintln("]");
    }

    private void CmdPopMeUp(string[] args, int startIndex)
    {
        UIManager.Instance?.GetPopUpUIController().Open("PopMeUp", PopUpUIController.TextType.Raw, "loc_popmeup", PopUpUIController.TextType.Localized);
    }

    private void CmdPopMeUp2(string[] args, int startIndex)
    {
        string title = args[startIndex + 1];
        string msg = args[startIndex + 2];
        UIManager.Instance?.GetPopUpUIController().Open(title, PopUpUIController.TextType.Raw, msg, PopUpUIController.TextType.Raw);
    }

    private void CmdTimeScale(string[] args, int startIndex)
    {
        string arg = args[startIndex + 1];
        float scale = CmdParseFloat(arg);
        Time.timeScale = scale;
    }

    private void CmdShowFps(string[] args, int startIndex)
    {
        string arg = args[startIndex + 1];
        bool show = CmdParseBool(arg);
        UIManager.Instance.GetInfoUI().DisplayFPS = show;
    }

    private void CmdGtfo(string[] args, int startIndex)
    {
        GameManager.Instance?.FinishGame();
    }

    private void CmdMkReq(string[] args, int startIndex)
    {
        string type = args[startIndex + 1];
        string url = args[startIndex + 2];
        string message = args[startIndex + 3];
        
        var callbacks = new ConnectionManager.RequestCallbacks(
            // OnSuccess
            (ans) => {
                CmdPrintln($"OnSuccess : {ans}");
            },
            // OnError
            (err) => {
                CmdPrintln($"OnError : {err}");
            },
            // OnConnectionSuccess
            () => {
                CmdPrintln("OnConnectionSuccess : Successfully connected");
            },
            // OnConnectionError
            () => {
                CmdPrintln("OnConnectionError : Could not connect");
            }
        );

        ConnectionManager.Instance.MakeRequest(type, url, message, callbacks);
    }

    private void CmdTriggerMeElmo(string[] args, int startIndex)
    {
        string arg = args[startIndex + 1];
        bool b = CmdParseBool(arg);
        PlayerDataManager.Instance.GetPlayer().CanActivateTriggers = b;
        CmdPrintln($"Trigger activation has been {(b ? CmdGetColorString("Enabled", Color.green) : CmdGetColorString("Disabled", Color.red))}!");
    }

    private void CmdLeaveMeAlone(string[] args, int startIndex)
    {
        NPCController[] npcs = GameObject.FindObjectsOfType<NPCController>();
        foreach (var npc in npcs)
        {
            npc.detectionProgress.Value = 0.0f;
            npc.Target = null;
            npc.GetComponent<NPCBehaviourController>()?.ResetBehaviour();
        }
    }

    private void CmdVolumeGlobal(string[] args, int startIndex)
    {
        float volume = CmdParseFloat(args[startIndex + 1]);
        SoundManager.Instance.VolumeGlobal = volume;
        CmdPrintln($"Global Volume has been set to {SoundManager.Instance.VolumeGlobal}");
    }

    private void CmdVolumeSFX(string[] args, int startIndex)
    {
        float volume = CmdParseFloat(args[startIndex + 1]);
        SoundManager.Instance.VolumeSFX = volume;
        CmdPrintln($"Music Volume has been set to {SoundManager.Instance.VolumeSFX}");
    }

    private void CmdVolumeMusic(string[] args, int startIndex)
    {
        float volume = CmdParseFloat(args[startIndex + 1]);
        SoundManager.Instance.VolumeMusic = volume;
        CmdPrintln($"Music Volume has been set to {SoundManager.Instance.VolumeMusic}");
    }

    private void CmdVolumeVoice(string[] args, int startIndex)
    {
        float volume = CmdParseFloat(args[startIndex + 1]);
        SoundManager.Instance.VolumeVoice = volume;
        CmdPrintln($"Music Volume has been set to {SoundManager.Instance.VolumeVoice}");
    }

    private void CmdVolumeUI(string[] args, int startIndex)
    {
        float volume = CmdParseFloat(args[startIndex + 1]);
        SoundManager.Instance.VolumeUI = volume;
        CmdPrintln($"Music Volume has been set to {SoundManager.Instance.VolumeUI}");
    }

    private void CmdPlaySound(string[] args, int startIndex)
    {
        string soundName = args[startIndex + 1];
        SoundManager.Instance.PlaySoundSFX(soundName, false, 1.0f, 1.0f);
    }

    #endregion

    #region Cmd - Parsing

    // Functions for parsing values from the input strings on the commands.
    // These are specific to the Cmd UI class because their result is not what one would expect on a programming language like C#, but something more aching to
    // what one could expect from a console in a game like Q3 or JK2 / JK3.

    private int CmdParseInt(string str)
    {
        int ans;
        try
        {
            ans = int.Parse(str);
        }
        catch
        {
            ans = 0;
        }
        return ans;
    }

    private bool CmdParseBool(string str)
    {
        bool ans;
        try
        {
            // bool.Parse() can't take a CultureInfo for some reason.
            // After running Convert.ToBoolean through a decompiler, we can see that under the hood it only implements "true" / "false",
            // so it's not like it matters in any way tbh cause the commands should be in english anyway, which is precisely why we're using the
            // stupid CultureInfo.InvariantCulture bullshit in the first place.
            ans = bool.Parse(str);
        }
        catch
        {
            try
            {
                // Only God knows why someone would input an integer with separators, but here we are...
                ans = int.Parse(str, CultureInfo.InvariantCulture) > 0;
            }
            catch
            {
                try
                {
                    ans = float.Parse(str, CultureInfo.InvariantCulture) > 0.0f;
                }
                catch
                {
                    ans = false;
                }
            }
        }
        return ans;
    }

    private float CmdParseFloat(string str)
    {
        float ans;
        try
        {
            // Gone are the days where floats are fucked!
            ans = float.Parse(str, CultureInfo.InvariantCulture);
        }
        catch
        {
            ans = 0.0f;
        }
        return ans;
    }

    #endregion

    #region Cmd - Color

    // NOTE : The color tags <color=#FF0000>Text</color> work through tag scopes, which means that the following example will look as follows:
    // DEFAULT COLOR <color=#FF0000> RED TEXT <color=#00FF00> GREEN TEXT </color> RED TEXT </color> DEFAULT COLOR
    // This is great for us cause it makes implementing colored text a hell of a lot easier, since we dont need to do any weird patchwork...
    // Also note that the "default color" is whatever color has been chosen on the inspector for the vertex color of the TMP Text.
    
    // NOTE : BTW, the color tags also work when displaying debug logs in unity, which is wild and fucking awesome tbh.

    private string CmdGetColorHexString(float r, float g, float b)
    {
        r = Mathf.Clamp01(r);
        g = Mathf.Clamp01(g);
        b = Mathf.Clamp01(b);

        int ri = (int)(255 * r);
        int gi = (int)(255 * g);
        int bi = (int)(255 * b);

        string rstr = $"{ri:X2}";
        string gstr = $"{gi:X2}";
        string bstr = $"{bi:X2}";

        string ans = $"#{rstr}{gstr}{bstr}";
        return ans;
    }

    private string CmdGetColorHexString(Color color)
    {
        return CmdGetColorHexString(color.r, color.g, color.b);
    }

    private string CmdGetColorString(string message, float r, float g, float b)
    {
        return $"<color={CmdGetColorHexString(r, g, b)}>{message}</color>";
    }

    private string CmdGetColorString(string message, Color color)
    {
        return $"<color={CmdGetColorHexString(color)}>{message}</color>";
    }

    #endregion
}

// TODO : Find a way to allow the user to put a custom sprite as the console background.Taking ricing to a whole new level...

// TODO : Fix issue where deselecting the input field makes the code send a OnEditEnd call, which is coded to also re-select the field...
// This is an issue because it forces the screen to scroll to the bottom of the console each time you try to scroll upward by using more than a single swipe lol.
