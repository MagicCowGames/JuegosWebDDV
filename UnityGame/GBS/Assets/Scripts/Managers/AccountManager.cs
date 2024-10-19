using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccountManager : SingletonPersistent<AccountManager>
{
    #region Variables

    public Account UserAccount { get; private set; }

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

    #region PrivateMethods
    #endregion
}
