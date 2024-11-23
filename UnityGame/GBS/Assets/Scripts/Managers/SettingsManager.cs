using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsManager : SingletonPersistent<SettingsManager>
{
    #region Variables

    private UserSettings settings;

    public UserSettings Settings { get { return this.settings; } set { this.settings = value; UpdateSettings(); } }

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

    // Updates the settings on the web server. This used to be named "UpdateUserAccountSettings()", but SaveSettings() is shorter, faster to type
    // and easier to remember, so yeah lol.
    public void SaveSettings()
    {

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

    private void UpdateSettings()
    {
        SetLanguage(this.settings.language);
        // TODO : More stuff in the future...
    }

    #endregion
}
