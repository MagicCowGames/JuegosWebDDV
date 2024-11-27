using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// A simpler version of the localized text controller, it always localizes texts to whatever the specified language is rather than the chosen language from settings.
// TODO : In the future, this could be merged with the basic localized text controller, but I don't want to risk fucking up all of my localized texts by accidentally
// making their loc_string fields reset...
[ExecuteInEditMode]
public class LocalizedTextFixedController : MonoBehaviour
{
    #region Variables

    [SerializeField] private string localizationString;
    [SerializeField] private LanguageSystem.Language currentLanguage; // The language this localized text currently has

    public string LocalizationString { get { return this.localizationString; } set { this.localizationString = value; UpdateLocalizedText(); } }

    #endregion

    #region MonoBehaviour

    void Start()
    {
        UpdateLocalizedText();
    }

    void Update()
    {

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
        obj.text = LanguageSystem.GetLocalizedString(this.currentLanguage, this.localizationString);
    }

    #endregion
}
