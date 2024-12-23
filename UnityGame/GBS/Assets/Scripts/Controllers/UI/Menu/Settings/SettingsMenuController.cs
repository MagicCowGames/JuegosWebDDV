using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// TODO : Make the settings SCREEN its own thing so that we can reuse that prefab both in game on the pause menu's UI and on the main menu's settings menu UI.
public class SettingsMenuController : UIController
{
    #region Variables
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

    public void ButtonReturn()
    {
        SettingsManager.Instance.SaveSettings(); // Sends a message to the server to request saving the current settings.
        SceneLoadingManager.Instance?.LoadSceneMainMenu();
    }

    public void ButtonLanguage()
    {
        SceneLoadingManager.Instance?.LoadScene("MenuScene_Settings_Language");
    }

    public void ButtonQuality()
    {
        SceneLoadingManager.Instance?.LoadScene("MenuScene_Settings_Quality");
    }

    public void ButtonSound()
    {
        SceneLoadingManager.Instance?.LoadScene("MenuScene_Settings_Sound");
    }

    public void ButtonExtra()
    {
        SceneLoadingManager.Instance?.LoadScene("MenuScene_Settings_Extra");
    }

    #endregion

    #region PrivateMethods
    #endregion
}
