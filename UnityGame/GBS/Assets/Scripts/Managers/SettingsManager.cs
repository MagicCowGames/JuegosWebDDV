using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsManager : SingletonPersistent<SettingsManager>
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
    #endregion

    #region PublicMethods - Language Settings

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

    #region PrivateMethods
    #endregion
}
