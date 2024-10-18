using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RegisterMenuController : MonoBehaviour
{
    #region Variables

    [SerializeField] private TMP_InputField inputFieldName;
    [SerializeField] private TMP_InputField inputFieldPassword;

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

    public bool GetPasswordVisible()
    {
        return this.inputFieldPassword.contentType == TMP_InputField.ContentType.Password;
    }

    public void Button_Back()
    {
        SceneLoadingManager.Instance?.LoadScene("MS_Account");
    }

    public void Button_SwitchPasswordVisibility()
    {
        if (GetPasswordVisible())
        {
            ShowPassword();
        }
        else
        {
            HidePassword();
        }
    }

    public void Button_Submit()
    {
        var name = this.inputFieldName.text;
        var password = this.inputFieldPassword.text;
        var callbacks = new ConnectionManager.RequestCallbacks();
        callbacks.OnSuccess = (ans) => {
            DebugManager.Instance?.Log($"Successfully created the account : {ans}");
        };
        callbacks.OnError = (err) => {
            DebugManager.Instance?.Log($"Could not create the account : {err}");
        };
        callbacks.OnConnectionError = () => {
            DebugManager.Instance?.Log("Connection Error");
        };
        AccountManager.Instance?.RegisterAccount(name, password, callbacks);
    }

    #endregion

    #region PrivateMethods

    private void ShowPassword()
    {
        this.inputFieldPassword.contentType = TMP_InputField.ContentType.Standard;
    }

    private void HidePassword()
    {
        this.inputFieldPassword.contentType = TMP_InputField.ContentType.Password;
    }

    #endregion
}
