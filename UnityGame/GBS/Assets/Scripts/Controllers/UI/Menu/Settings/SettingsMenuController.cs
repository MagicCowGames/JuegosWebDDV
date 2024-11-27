using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// TODO : Make the settings SCREEN its own thing so that we can reuse that prefab both in game on the pause menu's UI and on the main menu's settings menu UI.
public class SettingsMenuController : UIController
{
    #region Variables

    [Header("Settings - Language")]
    [SerializeField] private TMP_Text languageText; // The currently selected language.

    [Header("Settings - Quality")]
    [SerializeField] private TMP_Text qualityText;

    #endregion

    #region MonoBehaviour

    void Start()
    {
        
    }

    void Update()
    {
        var names = QualitySettings.names;

        /*
        foreach (var name in names)
            DebugManager.Instance.Log($"name = {name}");
        */

        var level = QualitySettings.GetQualityLevel();
        this.qualityText.text = $"{level}";
    }

    #endregion

    #region PublicMethods

    public void ButtonReturn()
    {
        SettingsManager.Instance.SaveSettings(); // Sends a message to the server to request saving the current settings.
        SceneLoadingManager.Instance?.LoadSceneMainMenu();
    }

    #endregion

    #region PublicMethods - Language

    public void ButtonLanguageLeft()
    {
        SettingsManager.Instance?.SetLanguageDecrease();
    }

    public void ButtonLanguageRight()
    {
        SettingsManager.Instance?.SetLanguageIncrease();
    }

    #endregion

    #region PublicMethods - Graphics Quality

    public void ButtonQualityLeft()
    {

    }

    public void ButtonQualityRight()
    { }

    #endregion

    #region PrivateMethods
    #endregion
}
