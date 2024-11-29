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

    void Start()
    {
        this.consoleEnabledToggle.isOn = UIManager.Instance.GetConsoleUI().ConsoleEnabled;
        this.versionEnabledToggle.isOn = UIManager.Instance.GetInfoUI().DisplayVersion;
        this.fpsEnabledToggle.isOn = UIManager.Instance.GetInfoUI().DisplayFPS;
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
