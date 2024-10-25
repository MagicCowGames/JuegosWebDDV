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
    [SerializeField] private Button buttonLanguageLeft;
    [SerializeField] private Button buttonLanguageRight;

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

    public void ButtonLanguageLeft()
    {
        LanguageSystem.SetLanguage((int)LanguageSystem.GetLanguage() - 1);
    }

    public void ButtonLanguageRight()
    {
        LanguageSystem.SetLanguage((int)LanguageSystem.GetLanguage() + 1);
    }

    public void ButtonReturn()
    {
        SceneLoadingManager.Instance?.LoadSceneMainMenu();
    }

    #endregion

    #region PrivateMethods
    #endregion
}
