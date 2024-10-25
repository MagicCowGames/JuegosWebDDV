using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[ExecuteInEditMode]
public class LocalizedTextController : MonoBehaviour
{
    #region Variables

    [SerializeField] private string localizationString;

    LanguageSystem.Language currentLanguage; // The language this localized text currently has

    #endregion

    #region MonoBehaviour

    void Start()
    {
        UpdateLocalizedText();
    }

    void Update()
    {
        // Update the language if it has been changed.
        // NOTE : In BV:DF this used to be hacked in by just reloading the entire scene when the language was changed so that the equivalent of
        // OnValidate() on phaser (which is preload()) would be called, causing the menu text to change. This time around we can set the lang from the console, so
        // we want to allow the game to reload the text instantly, and using delegates would be a pain in the ass for this.
        if (this.currentLanguage != LanguageSystem.GetLanguage())
            UpdateLocalizedText();
    }

    void OnValidate()
    {
        UpdateLocalizedText();
    }

    #endregion

    #region PublicMethods
    #endregion

    #region PrivateMethods

    private void UpdateLocalizedText()
    {
        // Update the displayed text of this localized text
        var obj = this.GetComponent<TMP_Text>();
        if (obj == null)
            return;
        obj.text = LanguageSystem.GetLocalizedString(localizationString);

        // Update the current language value of this localized text
        this.currentLanguage = LanguageSystem.GetLanguage();
    }

    #endregion
}
