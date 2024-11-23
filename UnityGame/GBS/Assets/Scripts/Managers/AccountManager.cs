using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccountManager : SingletonPersistent<AccountManager>
{
    #region Variables

    public Account UserAccount { get; private set; }
    public bool IsLoggedIn { get { return this.UserAccount.isLoggedIn; } }

    #endregion

    #region MonoBehaviour

    public override void Awake()
    {
        base.Awake();
        this.UserAccount = new Account(); // Initialize the user's account with an empty user account that is marked as not logged in.
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    #endregion



    #region PublicMethods - Account Requests

    // NOTE: All of the functions here can stand on their own without any custom callbacks, but an overload with a callbacks parameter exists to allow the
    // callers to define their own callbacks in case they want something else to happen on the call site.

    // TODO: Maybe the code for scene loading should be moved to each scene rather than it being here, since logging in could happen through a command
    // or on other scenes, maybe even during init by using cookies, who knows! For now, it'll stay as it is until it needs to be changed...

    public void RegisterAccount(string name, string password)
    {
        var callbacks = new ConnectionManager.RequestCallbacks();
        RegisterAccount(name, password, callbacks);
    }

    public void RegisterAccount(string name, string password, ConnectionManager.RequestCallbacks callbacks)
    {
        DebugManager.Instance?.Log("Submitting Request to Register Account...");
        var popUp = UIManager.Instance?.GetPopUpUIController();

        if (name.Length <= 0)
        {
            popUp.Open("loc_error_validation_title", "loc_error_validation_message_name");
            return;
        }

        if (password.Length <= 0)
        {
            popUp.Open("loc_error_validation_title", "loc_error_validation_message_password");
            return;
        }

        callbacks.OnSuccess += (ans) => {
            DebugManager.Instance?.Log($"Successfully created the account : {ans}");
            // TODO : Figure out how to handle errors in Unity where the received JSON doesn't actually match the requested class to deserialize to...
            this.UserAccount = JsonUtility.FromJson<Account>(ans);
            this.UserAccount.isLoggedIn = true;
            SceneLoadingManager.Instance?.LoadSceneAccount();
        };
        callbacks.OnError += (err) => {
            DebugManager.Instance?.Log($"Could not create the account : {err}");
            popUp.Open("loc_error_register_title", "loc_error_register_message");
        };
        callbacks.OnConnectionError += () => {
            DebugManager.Instance?.Log("Connection Error");
            popUp.Open("loc_error_connection_title", "loc_error_connection_message");
        };

        ConnectionManager.Instance.MakeRequest("GET", ConnectionManager.Instance.ServerAddress.http, $"/users/add/{name}/{password}", callbacks);
    }

    public void AccessAccount(string name, string password)
    {
        var callbacks = new ConnectionManager.RequestCallbacks();
        AccessAccount(name, password, callbacks);
    }

    public void AccessAccount(string name, string password, ConnectionManager.RequestCallbacks callbacks)
    {
        DebugManager.Instance?.Log("Submitting Request to Log into Account...");
        var popUp = UIManager.Instance?.GetPopUpUIController();

        if (name.Length <= 0)
        {
            popUp.Open("loc_error_validation_title", "loc_error_validation_message_name");
            return;
        }

        if (password.Length <= 0)
        {
            popUp.Open("loc_error_validation_title", "loc_error_validation_message_password");
            return;
        }

        callbacks.OnSuccess += (ans) => {
            DebugManager.Instance?.Log($"Successfully logged into the account : {ans}");
            this.UserAccount = JsonUtility.FromJson<Account>(ans);
            this.UserAccount.isLoggedIn = true;
            SceneLoadingManager.Instance?.LoadSceneAccount();
        };
        callbacks.OnError += (err) => {
            DebugManager.Instance?.Log($"Could not log into the account : {err}");
            popUp.Open("loc_error_login_title", "loc_error_login_message");
        };
        callbacks.OnConnectionError += () => {
            DebugManager.Instance?.Log("Connection Error");
            popUp.Open("loc_error_connection_title", "loc_error_connection_message");
        };

        ConnectionManager.Instance.MakeRequest("GET", ConnectionManager.Instance.ServerAddress.http, $"/users/login/{name}/{password}", callbacks);
    }

    public void DeleteAccount(string name, string password, ConnectionManager.RequestCallbacks callbacks)
    { }

    public void ModifyAccount(string oldName, string oldPassword, string newName, string newPassword, ConnectionManager.RequestCallbacks callbacks)
    { }

    #endregion

    // Discarded old code
    /*
    #region PublicMethods - Account Requests

    public void RegisterAccount(string name, string password, ConnectionManager.RequestCallbacks callbacks)
    {
        ConnectionManager.Instance.MakeRequest("GET", ConnectionManager.Instance.ServerAddress.http, $"/users/add/{name}/{password}", callbacks);
    }

    public void AccessAccount(string name, string password, ConnectionManager.RequestCallbacks callbacks)
    {
        ConnectionManager.Instance.MakeRequest("GET", ConnectionManager.Instance.ServerAddress.http, $"/users/login/{name}/{password}", callbacks);
    }

    public void DeleteAccount(string name, string password, ConnectionManager.RequestCallbacks callbacks)
    { }

    public void ModifyAccount(string oldName, string oldPassword, string newName, string newPassword, ConnectionManager.RequestCallbacks callbacks)
    { }

    #endregion

    #region PublicMethods - Account Data

    public void SetAccountData()
    { }

    #endregion
    */

    #region PrivateMethods
    #endregion
}
