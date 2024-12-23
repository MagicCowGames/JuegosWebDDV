using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// TODO : Unity the scene loading logic into a single function.
// TODO : Modify the scene loading logic to allow multiple scenes to be loaded at the same time, as well as allowing a map Cmd that was run during a scene
// transition to be executed, load the scene, and just forget the scene that was going to be opened with the scene transition... basically, as if the code was just "killed"
// in the middle of its execution. This can only be achieved (AFAIK) if I refactor the code to no longer make use of a coroutine, or maybe adding some other extra check...
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
    [SerializeField] private string inventoryScene;
    [SerializeField] private string storeScene;
    [SerializeField] private string accountScene;
    [SerializeField] private string scoreboardScene;

    private bool isLoadingSceneWithTransition;

    // NOTE : Prefixes used in strings for scene names:
    /*
        - GS : Game Scene / Gameplay Scene
        - MS : Menu Scene
    */

    #endregion

    #region Properties

    public string MainMenuScene { get { return this.mainMenuScene; } }
    public string CreditsScene { get { return this.creditsScene; } }
    public string SettingsScene { get { return this.settingsScene; } }
    public string TutorialScene { get { return this.tutorialScene; } }
    public string InventoryScene { get { return this.inventoryScene; } }
    public string StoreScene { get { return this.storeScene; } }
    public string AccountScene { get { return this.accountScene; } }
    public string ScoreboardScene { get { return this.scoreboardScene; } }


    public string CurrentScene { get { return SceneManager.GetActiveScene().name; } }

    #endregion

    #region MonoBehaviour

    void Start()
    {
        this.isLoadingSceneWithTransition = false;
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

        // Always unpause when the scene finishes loading to prevent issues
        if(GameUtility.GetPaused())
            GameUtility.Resume();
    }

    // NOTE : This could be modified to make use of events through the fade function on the fade UI controller itself rather than a coroutine, but it is what it is.
    // No need to change what works...
    // TODO : Merge the logic of this function with the LoadScene() one, or find a more consistent naming convention...
    public void LoadSceneWithTransition(string name)
    {
        // Can't load multiple scenes at the same time, done to prevent race conditions...
        // NOTE : This only exists precisely because of the capability to load scenes with transitions, which add a fucking delay to the loading call...
        // An example of a problem that comes up is being able to pause, press quit, then quickly press retry... This check solves that issue.
        // This also prevents the issue of some fucker just spamming the play button or return to menu button multiple times during the transition.
        if (this.isLoadingSceneWithTransition)
            return;

        StartCoroutine(TransitionToSceneInternal(name, 0.2f, 0.5f));
    }

    #endregion

    #region PublicMethods - Specific Scenes

    public void LoadSceneMainMenu()
    {
        LoadScene(this.mainMenuScene);
    }

    public void LoadSceneCredits()
    {
        LoadScene(this.creditsScene);
    }

    public void LoadSceneSettings()
    {
        LoadScene(this.settingsScene);
    }

    public void LoadSceneTutorial()
    {
        LoadScene(this.tutorialScene);
    }

    public void LoadSceneStore()
    {
        LoadScene(this.storeScene);
    }

    public void LoadSceneInventory()
    {
        LoadScene(this.inventoryScene);
    }

    public void LoadSceneAccount()
    {
        LoadScene(this.accountScene);
    }

    public void LoadSceneScoreboard()
    {
        LoadScene(this.scoreboardScene);
    }

    #endregion

    #region PublicMethods - Reload Scenes

    public void ReloadScene()
    {
        LoadScene(CurrentScene);
    }

    public void ReloadSceneWithTransition()
    {
        LoadSceneWithTransition(CurrentScene);
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

    private IEnumerator TransitionToSceneInternal(string name, float enterDuration, float exitDuration)
    {
        this.isLoadingSceneWithTransition = true;

        UIManager.Instance.GetFadeUIController().FadeOut(enterDuration);
        yield return new WaitForSecondsRealtime(enterDuration); // NOTE : The old implementation used the call WaitForSeconds(enterDuration), which actually uses scaled delta time and made it impossible to load levels with transitions while the game was paused, since the pause sets the time scale to 0.
        LoadScene(name);
        UIManager.Instance.GetFadeUIController().FadeIn(exitDuration);

        this.isLoadingSceneWithTransition = false;
    }

    #endregion
}
