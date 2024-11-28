using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsTogglesMenuController : MonoBehaviour
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

    // NOTE : This looks kinda funky, and yes, it should be possible to do this with just 1 function that takes the language as an input, but sadly it is not...
    // Unity's buttons on click events use UnityEvents, which don't seem to support enum inputs... XD

    public void SetLanguageEnglish()
    {
        LanguageSystem.SetLanguage(LanguageSystem.Language.English);
    }

    public void SetLanguageSpanish()
    {
        LanguageSystem.SetLanguage(LanguageSystem.Language.Spanish);
    }

    public void Button_Return()
    {
        SceneLoadingManager.Instance?.LoadSceneSettings();
    }

    #endregion

    #region PrivateMethods
    #endregion
}
