using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RegisterMenuController : MonoBehaviour
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
        DebugManager.Instance?.Log("Submitting Request to Register Account...");

        var name = this.inputBox.GetName();
        var password = this.inputBox.GetPassword();
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
    #endregion
}
