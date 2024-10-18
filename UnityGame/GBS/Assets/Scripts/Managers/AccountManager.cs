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

    public void RegisterAccount(string name, string password)
    { }

    public void EnterAccount(string name, string password)
    { }

    public void DeleteAccount(string name, string password)
    { }

    public void ModifyAccount(string oldName, string oldPassword, string newName, string newPassword)
    { }

    #endregion

    #region PrivateMethods
    #endregion
}
