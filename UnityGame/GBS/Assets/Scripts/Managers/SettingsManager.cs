using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// NOTE : Maybe it would have been a smarter idea to keep all settings within this class, and then allowing all the other managers to access those values from here,
// but I feel like those properties should be also stored within the managers that correspond to each system, otherwise it can all get dirty and messy pretty quickly.

// NOTE : Maybe it would make more sense if users had access to a save settings and a load settings buttons in the logged-in account screen rather than this being
// handled automatically, but it's also a pretty nice feature to have...

public class SettingsManager : SingletonPersistent<SettingsManager>
{
    #region Variables

    public UserSettings Settings { get { return GetSettings(); } set { SetSettings(value); } }

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

    // Updates the settings on the web server.
    // This used to be named "UpdateUserAccountSettings()", but SaveSettings() is shorter, faster to type and easier to remember, so yeah lol.
    public void SaveSettings()
    {
        // Can't save settings if we're not logged in yet!
        if (!AccountManager.Instance.IsLoggedIn)
            return;

        var id = AccountManager.Instance.Account.id;
        var password = AccountManager.Instance.Account.password;
        var settingsStr = JsonUtility.ToJson(this.Settings);

        string msg = $"/users/update/settings/{id}/{password}/{settingsStr}";
        ConnectionManager.Instance.MakeRequest("GET", ConnectionManager.Instance.ServerAddress.http, msg);
    }

    #endregion

    #region PublicMethods - Settings - Language

    public void SetLanguage(string language)
    {
        LanguageSystem.SetLanguage(language);
    }

    public void SetLanguage(LanguageSystem.Language language)
    {
        LanguageSystem.SetLanguage(language);
    }

    public void SetLanguageIncrease()
    {
        LanguageSystem.SetLanguage((int)LanguageSystem.GetLanguage() + 1);
    }

    public void SetLanguageDecrease()
    {
        LanguageSystem.SetLanguage((int)LanguageSystem.GetLanguage() - 1);
    }

    #endregion

    #region PublicMethods - Settings - Other
    #endregion

    #region PrivateMethods

    // Generates the settings DTO that will be used for network serialization
    private UserSettings GetSettings()
    {
        var settings = new UserSettings();
        settings.language = LanguageSystem.GetLanguage();
        return settings;
    }

    // Take an input UserSettings DTO and load the data into the actual classes that contain the real data for these settings
    private void SetSettings(UserSettings settings)
    {
        LanguageSystem.SetLanguage(settings.language);
    }

    #endregion
}
