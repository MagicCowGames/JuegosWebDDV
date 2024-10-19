using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoginMenuController : MonoBehaviour
{
    #region Variables

    [SerializeField] UserDataInputBoxController inputBox;

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

    public void Button_Back()
    {
        SceneLoadingManager.Instance?.LoadScene("MS_Account");
    }

    public void Button_Submit()
    {
        DebugManager.Instance?.Log("Submitting Request to Log into Account...");

        var name = this.inputBox.GetName();
        var password = this.inputBox.GetPassword();
        
        var callbacks = new ConnectionManager.RequestCallbacks();
        callbacks.OnSuccess = (ans) => {
            DebugManager.Instance?.Log($"Successfully logged into account : {ans}");
            Button_Back();
        };
        callbacks.OnError = (err) => {
            DebugManager.Instance?.Log($"Could not log into account : {err}");
        };
        callbacks.OnConnectionError = () => {
            DebugManager.Instance?.Log("Connection Error");
        };

        AccountManager.Instance?.AccessAccount(name, password, callbacks);
    }

    #endregion

    #region PrivateMethods
    #endregion
}
