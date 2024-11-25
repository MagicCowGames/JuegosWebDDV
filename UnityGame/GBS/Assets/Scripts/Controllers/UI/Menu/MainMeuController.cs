using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMeuController : MonoBehaviour
{
    #region Variables



    #endregion

    #region MonoBehaviour

    void Start()
    {
        // Play the main menu music each time we go back to the main menu, but don't start playing it again if we're already playing this theme to prevent
        // cuts in audio when moving from a sub-menu back to main menu.
        if (!SoundManager.Instance.IsPlayingMusic("main"))
            SoundManager.Instance?.PlayMusic("main");
    }

    void Update()
    {
        
    }

    #endregion

    #region PublicMethods

    public void LoadCredits()
    {
        SceneLoadingManager.Instance?.LoadSceneCredits();
    }

    public void LoadGame()
    {
        SceneLoadingManager.Instance?.LoadScene(/*"GS_PrisonCell"*/ "gs_s1_m1_prison_arena");
    }

    public void LoadAccountMenu()
    {
        SceneLoadingManager.Instance?.LoadSceneAccount();
    }

    public void LoadSettings()
    {
        SceneLoadingManager.Instance?.LoadSceneSettings();
    }

    public void LoadTutorial()
    {
        SceneLoadingManager.Instance?.LoadSceneTutorial();
    }

    public void LoadStore()
    {
        SceneLoadingManager.Instance?.LoadSceneStore();
    }

    public void LoadInventory()
    {
        SceneLoadingManager.Instance?.LoadSceneInventory();
    }

    public void LoadScoreboard()
    {
        SceneLoadingManager.Instance?.LoadSceneScoreboard();
    }

    #endregion

    #region PrivateMethods
    #endregion
}
