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

    // private UserSettings settings;

    // public UserSettings Settings { get { return this.settings; } set { this.settings = value; } }

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
    /*
    public void UpdateSettings()
    {
        UpdateSettings_Graphics();
        UpdateSettings_Language();
        UpdateSettings_Sound();
        UpdateSettings_Extra();
        UpdateSettings_Cosmetic();
    }
    */

    // Saves the settings (persistence)
    // In local builds, this should save data to the disk (not yet implemented). On WebGL builds, it updates the settings on the web server.
    // This used to be named "UpdateUserAccountSettings()", but SaveSettings() is shorter, faster to type and easier to remember, so yeah lol.
    public void SaveSettings()
    {
        #region WebGL

        // Can't save settings if we're not logged in yet!
        if (!AccountManager.Instance.IsLoggedIn)
            return;

        var settings = GetSettings(); // Construct a settings object from the data within the real/"physical" systems of the game.
        var id = AccountManager.Instance.Account.id;
        var password = AccountManager.Instance.Account.password;
        var settingsStr = JsonUtility.ToJson(settings);

        ConnectionManager.Instance.MakeRequestToServer("GET", $"/users/update/settings/{id}/{password}/{settingsStr}"); // TODO : Update this string to be users/set instead of /update for consistency. Update on the serverside too.

        #endregion

        #region Local
        
        // TODO : Implement

        #endregion
    }

    // TODO : Make a LoadSettings() function and move the settings loading code from the other classes to here. That way, under the hood, this could all be
    // replaced with simpler code to actually just save data to files on the disk when making builds other than WebGL.
    // NOTE : Yes, I know it's kinda dumb to divide the settings getting into a 2 step thing... but at least it makes the code generic enough to make it possible
    // to easily implement settings loading for builds that run on local devices rather than WebGL builds, which will come in handy later on...
    public void LoadSettings()
    {
        #region WebGL

        var callbacks = new ConnectionManager.RequestCallbacks();
        callbacks.OnSuccess += (ans) => {
            var settings = JsonUtility.FromJson<UserSettings>(ans);
            SetSettings(settings); // Apply the data within this DTO to the real physical systems of the game by actually loading the settings.
        };
        var id = AccountManager.Instance.Account.id;
        var password = AccountManager.Instance.Account.password;
        ConnectionManager.Instance.MakeRequestToServer("GET", $"/users/get/settings/{id}/{password}"); // TODO : Adjust this string to fit whatever is implemented on the serverside later on.

        #endregion

        #region Local
        
        // TODO : Implement

        #endregion
    }

    #endregion

    #region PrivateMethods

    // Gets the real settings data from the real systems that contain the actual variables that contain the data currently being used by the program.
    // It then stores it within an UserSettings object and returns it.
    // Basically, a fetching function of sorts that updates the DTO's contents when needed.
    private UserSettings GetSettings()
    {
        UserSettings settings = new UserSettings();

        settings.graphicsSettings.quality = QualitySettings.GetQualityLevel();
        settings.languageSettings.language = LanguageSystem.GetLanguage();
        // settings.soundSettings.etc... // TODO : Implement
        settings.extraSettings.displayVersion = UIManager.Instance.GetInfoUI().DisplayVersion;
        settings.extraSettings.displayFps = UIManager.Instance.GetInfoUI().DisplayFPS;
        settings.extraSettings.consoleEnabled = UIManager.Instance.GetConsoleUI().ConsoleEnabled;
        // settings.cosmeticSettings.color = etc...;

        return settings;
    }

    // Update the settings using the data stored within an UserSettings object.
    private void SetSettings(UserSettings settings)
    {
        UpdateSettings_Graphics(settings.graphicsSettings);
        UpdateSettings_Language(settings.languageSettings);
        UpdateSettings_Sound(settings.soundSettings);
        UpdateSettings_Extra(settings.extraSettings);
        UpdateSettings_Cosmetic(settings.cosmeticSettings);
    }

    private void UpdateSettings_Graphics(GraphicsSettings settings)
    {
        QualitySettings.SetQualityLevel(settings.quality);
    }

    private void UpdateSettings_Language(LanguageSettings settings)
    {
        LanguageSystem.SetLanguage(settings.language);
    }

    private void UpdateSettings_Sound(SoundSettings settings)
    {
        // TODO : Implement
    }

    private void UpdateSettings_Extra(ExtraSettings settings)
    {
        UIManager.Instance.GetInfoUI().DisplayFPS = settings.displayFps;
        UIManager.Instance.GetInfoUI().DisplayVersion = settings.displayVersion;
        UIManager.Instance.GetConsoleUI().ConsoleEnabled = settings.consoleEnabled;
    }

    private void UpdateSettings_Cosmetic(CosmeticSettings settings)
    {
        // TODO : Implement
    }

    #endregion
}
