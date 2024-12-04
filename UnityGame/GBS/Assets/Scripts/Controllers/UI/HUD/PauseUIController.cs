using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseUIController : UIController
{
    #region Variables
    #endregion

    #region MonoBehaviour

    void Start()
    {
        GameUtility.SetPaused(false);
    }

    void Update()
    {
        
    }

    #endregion

    #region PublicMethods

    public void Button_Resume()
    {
        GameUtility.Resume();
    }

    public void Button_Retry()
    {
        SceneLoadingManager.Instance?.ReloadSceneWithTransition();
    }

    public void Button_ReturnToMenu()
    {
        GameUtility.Resume(); // Resume before loading main menu
        SceneLoadingManager.Instance?.LoadSceneMainMenu();
    }

    #endregion

    #region PrivateMethods
    #endregion
}
