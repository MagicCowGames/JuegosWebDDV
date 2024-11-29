using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsExtraMenuController : MonoBehaviour
{
    #region Variables

    // [SerializeField] private Toggle debugEnabledToggle;
    [SerializeField] private Toggle consoleEnabledToggle;
    [SerializeField] private Toggle versionEnabledToggle;
    [SerializeField] private Toggle fpsEnabledToggle;

    #endregion

    #region MonoBehaviour

    void Awake()
    {
        // This init code that sets the toggles to the correct initial state takes place on Awake() since we don't want to see the buttons on the possibly wrong
        // default state for 1 frame before adopting the correct value. Calling this on Awake() makes the update take place before anything is rendered to the
        // screen, so we always get to see the correct state for the toggles when we open the menu... now, if the user changes stuff through the console, that's
        // a whole other story we ain't tackled yet lol...
        this.consoleEnabledToggle.isOn = UIManager.Instance.GetConsoleUI().ConsoleEnabled;
        this.versionEnabledToggle.isOn = UIManager.Instance.GetInfoUI().DisplayVersion;
        this.fpsEnabledToggle.isOn = UIManager.Instance.GetInfoUI().DisplayFPS;
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    #endregion

    #region PublicMethods

    /*
    public void SetDebugEnabled(bool enabled)
    { }
    */

    public void SetConsoleEnabled(bool enabled)
    {
        UIManager.Instance.GetConsoleUI().ConsoleEnabled = enabled;
    }

    public void SetVersionEnabled(bool enabled)
    {
        UIManager.Instance.GetInfoUI().DisplayVersion = enabled;
    }

    public void SetFpsEnabled(bool enabled)
    {
        UIManager.Instance.GetInfoUI().DisplayFPS = enabled;
    }

    public void Button_Return()
    {
        SceneLoadingManager.Instance?.LoadSceneSettings();
    }

    #endregion

    #region PrivateMethods
    #endregion
}
