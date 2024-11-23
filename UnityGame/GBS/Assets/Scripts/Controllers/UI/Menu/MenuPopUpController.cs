using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MenuPopUpController : UIController
{
    #region Variables

    [Header("Menu PopUp Controller")]
    [SerializeField] private LocalizedTextController localizedTextController;
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

    public void Open()
    {
        this.canvasGroup.blocksRaycasts = true;
        this.UI_SetVisible(true);
    }

    public void Close()
    {
        this.canvasGroup.blocksRaycasts = false;
        this.UI_SetVisible(false);
    }

    public void OpenRaw(string str)
    {
        SetTextRaw(str);
        Open();
    }

    public void OpenLoc(string loc)
    {
        SetTextLoc(loc);
        Open();
    }

    public void SetTextRaw(string str)
    {
        this.localizedTextController.enabled = false;
        this.text.text = str;
    }

    public void SetTextLoc(string loc)
    {
        this.localizedTextController.enabled = true;
        this.localizedTextController.LocalizationString = loc;
    }

    #endregion

    #region PrivateMethods
    #endregion
}
