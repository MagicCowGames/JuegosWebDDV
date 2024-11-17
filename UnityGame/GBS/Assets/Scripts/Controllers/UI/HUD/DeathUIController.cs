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

    // NOTE : As of now, it simply reloads the only scene we have.
    // TODO : Change implementation to get current level name and reload from start rather than hardcoding the level to be loaded.
    // Maybe even add checkpoint support if you find it to be cool with this game's gameplay.
    public void Button_Retry()
    {
        SceneLoadingManager.Instance?.LoadScene(SceneManager.GetActiveScene().name);
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
