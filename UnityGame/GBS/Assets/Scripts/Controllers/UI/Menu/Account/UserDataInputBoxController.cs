using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[ExecuteInEditMode]
public class UserDataInputBoxController : MonoBehaviour
{
    #region Enums

    public enum Style
    {
        Access, // name + password
        Modify, // name + oldPassword + newPassword
        Delete // confirmation password
    }

    #endregion

    #region Variables

    [Header("Rect Transforms")]
    [SerializeField] private RectTransform accessRect;
    [SerializeField] private RectTransform modifyRect;
    [SerializeField] private RectTransform deleteRect;

    [Header("Input Fields")]
    [SerializeField] private TMP_InputField accessInputFieldName;
    [SerializeField] private TMP_InputField accessInputFieldPassword;
    [SerializeField] private TMP_InputField modifyInputFieldOldPassword;
    [SerializeField] private TMP_InputField modifyInputFieldNewPassword;
    [SerializeField] private TMP_InputField deleteInputFieldPassword;

    [Header("Submit Buttons")]
    [SerializeField] private Button accessButton;
    [SerializeField] private Button modifyButton;
    [SerializeField] private Button deleteButton;

    [Header("Style Settings")]
    [SerializeField] private Style rectStyle;

    [Header("Callback Settings")]
    [SerializeField] private Button.ButtonClickedEvent onSubmit;

    private TMP_InputField.ContentType passwordContentType;

    #endregion

    #region MonoBehaviour

    void Start()
    {
        SetPasswordVisible(false);
        SetCallbacks();
    }

    void Update()
    {
        
    }

    void OnValidate()
    {
        // Stupid workaround to prevent error messages because Unity is dumb and doesn't have fucking construction scripts that only run in Editor out of the box...
        // Really finding myself tempted to make my own MonoBehaviour-like class that inherits from Unity's and adds my own custom functionality...
        // That would also make the whole pasuing implementation orders of magnitude easier to build.
        if (!Application.isPlaying)
            ShowRect(this.rectStyle);
    }

    #endregion

    #region PublicMethods

    public string GetName()
    {
        return accessInputFieldName.text;
    }

    public string GetPassword()
    {
        return this.accessInputFieldPassword.text;
    }

    public string GetOldPassword()
    {
        return this.modifyInputFieldOldPassword.text;
    }

    public string GetNewPassword()
    {
        return this.modifyInputFieldNewPassword.text;
    }

    public string GetConfirmationPassword()
    {
        return this.deleteInputFieldPassword.text;
    }

    public void SetPasswordVisible(bool visible)
    {
        this.passwordContentType = visible ? TMP_InputField.ContentType.Standard : TMP_InputField.ContentType.Password;

        this.accessInputFieldPassword.contentType = this.passwordContentType;
        this.modifyInputFieldOldPassword.contentType = this.passwordContentType;
        this.modifyInputFieldNewPassword.contentType = this.passwordContentType;
        this.deleteInputFieldPassword.contentType = this.passwordContentType;
    }

    public bool GetPasswordVisible()
    {
        return this.passwordContentType != TMP_InputField.ContentType.Password;
    }

    public void SwitchPasswordVisible()
    {
        SetPasswordVisible(!GetPasswordVisible());
    }

    #endregion

    #region PrivateMethods

    private void HideAllRects()
    {
        this.accessRect.gameObject.SetActive(false);
        this.modifyRect.gameObject.SetActive(false);
        this.deleteRect.gameObject.SetActive(false);
    }

    private void ShowRect(Style style)
    {
        HideAllRects();
        switch (style)
        {
            default:
            case Style.Access:
                this.accessRect.gameObject.SetActive(true);
                break;
            case Style.Modify:
                this.modifyRect.gameObject.SetActive(true);
                break;
            case Style.Delete:
                this.deleteRect.gameObject.SetActive(true);
                break;
        }
    }

    private void SetCallbacks()
    {
        this.accessButton.onClick = this.onSubmit;
        this.modifyButton.onClick = this.onSubmit;
        this.deleteButton.onClick = this.onSubmit;
    }

    #endregion
}
