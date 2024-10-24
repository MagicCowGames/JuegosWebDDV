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
        
    }

    void Update()
    {
        
    }

    void OnValidate()
    {
        var obj = this.GetComponent<TMP_Text>();
        if (obj == null)
            return;
        obj.text = LanguageSystem.GetLocalizedString(localizationString);
    }

    #endregion

    #region PublicMethods
    #endregion

    #region PrivateMethods
    #endregion
}
