using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccountManager : SingletonPersistent<AccountManager>
{
    #region Variables

    public Account UserAccount { get; private set; }
    public bool IsLoggedIn { get { return this.UserAccount.IsLoggedIn; } }

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
            popUp.Open("Validation Error!", PopUpUIController.TextType.Raw, "The chosen name is not valid!", PopUpUIController.TextType.Raw);
            return;
        }

        if (password.Length <= 0)
        {
            popUp.Open("Validation Error!", PopUpUIController.TextType.Raw, "The chosen password is not valid!", PopUpUIController.TextType.Raw);
            return;
        }

        var callbacks = new ConnectionManager.RequestCallbacks();
        callbacks.OnSuccess += (ans) => {
            DebugManager.Instance?.Log($"Successfully created the account : {ans}");
            SceneLoadingManager.Instance?.LoadSceneAccount();
        };
        callbacks.OnError += (err) => {
            DebugManager.Instance?.Log($"Could not create the account : {err}");
            // UIManager.Instance?.GetPopUpUIController().OpenLoc("loc_register_error");
        };
        callbacks.OnConnectionError += () => {
            DebugManager.Instance?.Log("Connection Error");
            // UIManager.Instance?.GetPopUpUIController().OpenLoc("loc_connection_error");
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
