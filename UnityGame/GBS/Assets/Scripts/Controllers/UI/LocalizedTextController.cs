using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[ExecuteInEditMode]
public class LocalizedTextController : MonoBehaviour
{
    #region Variables

    [SerializeField] private string localizationString;

    #endregion

    #region MonoBehaviour

    void Start()
    {
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
        var obj = this.GetComponent<TMP_Text>();
        if (obj == null)
            return;
        obj.text = LanguageSystem.GetLocalizedString(localizationString);
    }

    #endregion
}
