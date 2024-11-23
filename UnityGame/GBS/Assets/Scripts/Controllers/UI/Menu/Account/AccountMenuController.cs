using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccountMenuController : MonoBehaviour
{
    #region Variables
    #endregion

    #region MonoBehaviour

    void Awake()
    {
        // If the user is logged in, then go to the user's account info menu.
        // Otherwise, stay on the account select menu (the one with the register and login options).
        #region Comment

        // If the user is logged in, then load the "Account control panel"-like UI menu scene, where all of the account info and actions are displayed.
        // Note that this happens as soon as we enter this scene if we're logged in, and since the code runs on Awake() rather than Start(), we don't see
        // this current scene appear on screen, not even by 1 frame, so it feels smooth and doesn't have a jarring transition (this used to be a problem before
        // because this code was ran on Start() rather than on Awake(), so we had 1 frame of seeing this menu and then we got popped into the right one... that also
        // came with the potential problem of an user timing a button press really well and pressing the login or register buttons during that one frame, which would
        // not break anything, but it would be kind of funny and weird...)

        #endregion
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

    void Start()
    {
        
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
