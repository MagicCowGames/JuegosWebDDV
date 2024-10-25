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
