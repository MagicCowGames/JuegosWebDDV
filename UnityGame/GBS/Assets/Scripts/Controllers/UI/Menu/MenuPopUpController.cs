using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Build.Content;
using UnityEngine;
using static TreeEditor.TreeEditorHelper;

public class MenuPopUpController : UIController
{
    #region Enums

    public enum TextType
    {
        Raw = 0,
        Localized
    }

    #endregion

    #region Variables

    [Header("Menu PopUp Controller")]
    [SerializeField] private LocalizedTextController titleController;
    [SerializeField] private LocalizedTextController messageController;
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text messageText;
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

    public void Open(string titleStr, TextType titleType, string bodyStr, TextType bodyType)
    {
        SetTitle(titleStr, titleType);
        SetMessage(bodyStr, bodyType);
        Open();
    }

    public void SetTitle(string str, TextType type)
    {
        SetText_Internal(this.titleText, this.titleController, str, type);
    }

    public void SetMessage(string str, TextType type)
    {
        SetText_Internal(this.messageText, this.messageController, str, type);
    }

    #endregion

    #region PrivateMethods

    private void SetText_Internal(TMP_Text text, LocalizedTextController controller, string str, TextType type)
    {
        switch (type)
        {
            default:
            case TextType.Raw:
                controller.enabled = false;
                text.text = str;
                break;
            case TextType.Localized:
                controller.enabled = true;
                controller.LocalizationString = str;
                break;
        }
    }

    #endregion
}
