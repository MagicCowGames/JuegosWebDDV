using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadingManager : SingletonPersistent<SceneLoadingManager>
{
    #region Variables

    // NOTE : This may seem like a weird system... why not just load them by name always? well, simple. What happens if I need to rename a scene that is accessed
    // by many other scenes? then I would need to go renaming the destination scene of the return button on multiple scenes that lead back to said scene...
    // an alternative would be to make a stack system for the menus where the menus you entered are saved on a stack so that you can make the return button
    // just pop the last level and return to the previous level without having to specify names. This would also add weird beahviour if we used the map command
    // to load any specific menu map... because using map would require clearing the stack, and possibly breaking things, so I need to think about this before
    // implementing shit all willy nilly...
    [Header("Scene Names - Menus")]
    [SerializeField] private string mainMenuScene;
    [SerializeField] private string creditsScene;
    [SerializeField] private string settingsScene;
    [SerializeField] private string tutorialScene;

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
        ResetOtherManagers();
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

    public void LoadSceneSettings()
    {
        LoadScene(settingsScene);
    }

    public void LoadSceneTutorial()
    {
        LoadScene(tutorialScene);
    }

    #endregion

    #region PrivateMethods

    // This method is used to reset managers that reference objects from the scene that will no longer exist when a new scene is loaded.
    // This would not be an issue if those managers were level specific like I implemented them originally, but right now I'm testing it like this to see
    // if its a better pattern or not.
    private void ResetOtherManagers()
    {
        CameraManager.Instance?.ResetManager();
        UIManager.Instance?.ResetManager();
    }

    #endregion
}
