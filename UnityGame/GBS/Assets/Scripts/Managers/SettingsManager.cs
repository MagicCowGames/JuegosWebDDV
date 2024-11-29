using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// NOTE : Maybe it would have been a smarter idea to keep all settings within this class, and then allowing all the other managers to access those values from here,
// but I feel like those properties should be also stored within the managers that correspond to each system, otherwise it can all get dirty and messy pretty quickly.

// NOTE : Maybe it would make more sense if users had access to a save settings and a load settings buttons in the logged-in account screen rather than this being
// handled automatically, but it's also a pretty nice feature to have...

// NOTE : This could actually probably be a static class rather than a singleton... it does not require any sort of config that has to be performed on the
// inspector panel, so it's kinda pointless to make this a singleton.
public class SettingsManager : SingletonPersistent<SettingsManager>
{
    #region Variables

    private UserSettings settings;

    public UserSettings Settings { get { return this.settings; } set { this.settings = value; } }

    #endregion

    #region MonoBehaviour

    // TODO : Maybe should move the init scene #if UNITY_EDITOR logic here? or maybe not, not sure. We'll see what makes more sense in the long run.
    // If we made this change, then this class would start to make sense as a singleton, as it would require having some sort of initialization logic...

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    #endregion

    #region PublicMethods

    // Update the settings on all of the systems that make use of them.
    // NOTE : Calling this is kind of a bad idea if you are going to update only one single setting, since internally this forces all settings to update,
    // and that can be kinda heavy for graphics-related settings.
    public void UpdateSettings()
    {
        UpdateSettings_Graphics();
        UpdateSettings_Language();
        UpdateSettings_Sound();
        UpdateSettings_Extra();
        UpdateSettings_Cosmetic();
    }

    // Saves the settings (persistence)
    // In local builds, this should save data to the disk (not yet implemented). On WebGL builds, it updates the settings on the web server.
    // This used to be named "UpdateUserAccountSettings()", but SaveSettings() is shorter, faster to type and easier to remember, so yeah lol.
    public void SaveSettings()
    {
        // Can't save settings if we're not logged in yet!
        if (!AccountManager.Instance.IsLoggedIn)
            return;

        var id = AccountManager.Instance.Account.id;
        var password = AccountManager.Instance.Account.password;
        var settingsStr = JsonUtility.ToJson(this.Settings);

        ConnectionManager.Instance.MakeRequestToServer("GET", $"/users/update/settings/{id}/{password}/{settingsStr}");
    }

    // TODO : Make a LoadSettings() function and move the settings loading code from the other classes to here. That way, under the hood, this could all be
    // replaced with simpler code to actually just save data to files on the disk when making builds other than WebGL.

    #endregion

    #region PrivateMethods

    private void UpdateSettings_Graphics()
    {
        QualitySettings.SetQualityLevel(this.settings.graphicsSettings.quality);
    }

    private void UpdateSettings_Language()
    {
        LanguageSystem.SetLanguage(this.settings.languageSettings.language);
    }

    private void UpdateSettings_Sound()
    {
        // TODO : Implement
    }

    private void UpdateSettings_Extra()
    {
        UIManager.Instance.GetInfoUI().DisplayFPS = this.settings.extraSettings.displayFps;
        UIManager.Instance.GetInfoUI().DisplayVersion = this.settings.extraSettings.displayVersion;
        UIManager.Instance.GetConsoleUI().ConsoleEnabled = this.settings.extraSettings.consoleEnabled;
    }

    private void UpdateSettings_Cosmetic()
    {
        // TODO : Implement
    }

    #endregion
}
