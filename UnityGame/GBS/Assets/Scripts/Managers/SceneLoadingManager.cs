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
        // The call to SetActiveScene requires that the input scene we're passing is registered as loaded.
        // For that to happen, Unity requires at least 1 frame between the LoadScene() call and the SetActiveScene() call.
        // This would require making use of a coroutine or some waiting mechanism to delay the calls by 1 update / frame.
        // This call is now disabled because it is not really needed as of now, as it would only make sense if we were to load multiple scenes at a time,
        // but we're only using LoadSceneMode.Single so fuck it.
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