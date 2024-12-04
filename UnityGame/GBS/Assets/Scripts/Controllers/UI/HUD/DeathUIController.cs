using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathUIController : UIController
{
    #region Variables
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

    // TODO : Maybe even add checkpoint support if you find it to be cool with this game's gameplay.
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
