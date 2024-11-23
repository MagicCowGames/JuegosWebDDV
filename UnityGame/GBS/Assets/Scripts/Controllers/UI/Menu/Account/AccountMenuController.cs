using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccountMenuController : MonoBehaviour
{
    #region Variables
    #endregion

    #region MonoBehaviour
    
    void Start()
    {
        // If the user is logged in, then load the "Account control panel"-like UI menu scene, where all of the account info and actions are displayed.
        if (AccountManager.Instance.UserAccount.IsLoggedIn)
        {
            SceneLoadingManager.Instance.LoadScene("MS_AccountLogged");
        }
    }

    void Update()
    {
        
    }

    #endregion

    #region PublicMethods

    public void Button_Return()
    {
        SceneLoadingManager.Instance?.LoadSceneMainMenu();
    }

    public void Button_Login()
    {
        SceneLoadingManager.Instance?.LoadScene("MS_A_Login");
    }

    public void Button_Register()
    {
        SceneLoadingManager.Instance?.LoadScene("MS_A_Register");
    }

    #endregion

    #region PrivateMethods
    #endregion
}
