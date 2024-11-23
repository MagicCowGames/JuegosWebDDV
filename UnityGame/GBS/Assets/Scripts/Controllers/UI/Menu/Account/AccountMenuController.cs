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
        // Note that this happens as soon as we enter this scene if we're logged in, so we should not see this scene appear on screen, or just a few frames at most...
        if (AccountManager.Instance != null && AccountManager.Instance.IsLoggedIn)
        {
            DebugManager.Instance?.Log("User is logged in; Loading user account settings menu.");
            SceneLoadingManager.Instance?.LoadScene("MS_AccountLogged");
        }
        else
        {
            DebugManager.Instance?.Log("User is not logged in; Loading default account menu.");
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
