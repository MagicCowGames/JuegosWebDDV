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
        SceneLoadingManager.Instance?.LoadScene("DebugSceneGameplay");
    }

    public void LoadAccountMenu()
    {
        SceneLoadingManager.Instance?.LoadScene("MS_Account");
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

    #endregion

    #region PrivateMethods
    #endregion
}
