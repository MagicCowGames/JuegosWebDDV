using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadingManager : SingletonPersistent<SceneLoadingManager>
{
    #region Variables

    [Header("Scene Names - Menus")]
    [SerializeField] private string mainMenuScene;
    [SerializeField] private string creditsScene;

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

    public void LoadScene(string name)
    {
        SceneManager.LoadScene(name, LoadSceneMode.Single);
        // SceneManager.SetActiveScene(SceneManager.GetSceneByName(name));
    }

    #endregion

    #region PublicMethods Specific Scenes

    public void LoadSceneMainMenu()
    {
        LoadScene(mainMenuScene);
    }

    public void LoadSceneCredits()
    {
        LoadScene(creditsScene);
    }

    #endregion

    #region PrivateMethods
    #endregion
}
