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

    public void RegisterAccount(string name, string password)
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

        var callbacks = new ConnectionManager.RequestCallbacks();
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
