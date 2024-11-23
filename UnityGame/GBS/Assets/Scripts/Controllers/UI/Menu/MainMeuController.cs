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
        SceneLoadingManager.Instance?.LoadScene("GS_PrisonCell");
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
