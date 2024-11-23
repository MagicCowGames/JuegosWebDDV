using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MenuPopUpController : UIController
{
    #region Variables

    [Header("Menu PopUp Controller")]
    [SerializeField] private TMP_Text text;
    [SerializeField] private CanvasGroup canvasGroup;

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

    public void OpenPopUp()
    {
        this.canvasGroup.blocksRaycasts = true;
        this.UI_SetVisible(true);
    }

    public void ClosePopUp()
    {
        this.canvasGroup.blocksRaycasts = false;
        this.UI_SetVisible(false);
    }

    public void SetTextRawString(string str)
    {
        this.text.text = str;
    }

    public void SetTextLocalizationString(string locString)
    {
        text.GetComponent<LocalizedTextController>().LocalizationString = locString;
    }

    #endregion

    #region PrivateMethods
    #endregion
}
