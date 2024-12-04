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
        // NOTE : With the new loading code, resuming is done automatically so we no longer need to call this by hand.
        // GameUtility.Resume(); // Resume before loading main menu
        SceneLoadingManager.Instance?.LoadSceneWithTransition(SceneLoadingManager.Instance.MainMenuScene);
    }

    #endregion

    #region PrivateMethods
    #endregion
}
