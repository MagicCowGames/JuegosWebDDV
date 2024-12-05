using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InitSceneController : UIController
{
    #region Variables

    #endregion

    #region MonoBehaviour

    void Start()
    {
        Init();
        LoadMainMenu();
    }

    void Update()
    {
        
    }

    #endregion

    #region PublicMethods
    #endregion

    #region PrivateMethods

    private void Init()
    {
        #region Settings - Debug
        // NOTE : This part of the code that is dedicated to the DebugManager's debug enabled mode should always work since, unlike other systems, the DebugManager
        // is initialized on Awake(), and this Init() method runs on Start(), so the order in which the calls to Start() are made on the scene should not matter.
#if UNITY_EDITOR
        // If compiling the editor build, always set as default:
        // - Set Debug Display to TRUE.
        // - Set FPS Display to TRUE.
        // - Set Console Enabled to TRUE.
        
        DebugManager.Instance.SetDebugEnabled(true);
        UIManager.Instance.GetInfoUI().DisplayFPS = true;
        UIManager.Instance.GetConsoleUI().ConsoleEnabled = true;
#else
        // If compiling the release build, always set as default:
        // - Set Debug Display to FALSE.
        // - Set FPS Display to FALSE.
        // - Set Console Enabled to FALSE.
        
        DebugManager.Instance.SetDebugEnabled(false);
        UIManager.Instance.GetInfoUI().DisplayFPS = false;
        UIManager.Instance.GetConsoleUI().ConsoleEnabled = false;

        // This is done just to make sure that I don't forget to set the debug to false at some point in the inspector panel before making a release build.
#endif
        #endregion

        #region Settings - Screen

        // Set the screen orientation to "landscape" for mobile devices.
        // This gives an error on web when running on a PC, but it doesn't matter cause it just does nothing so that's fine for now.
        Screen.orientation = ScreenOrientation.LandscapeLeft;

        #endregion

    }

    private void LoadMainMenu()
    {
        // NOTE : You don't have to worry about the manager instance not existing when calling this on the init scene, because we're calling it from Start(),
        // and the manager instance is created on Awake(), which means that the order in which the object calls are done by Unity does not matter, since
        // all of the Awake() stage calls will take place before all of the Start() calls.
        SceneLoadingManager.Instance?.LoadSceneWithTransition(SceneLoadingManager.Instance.MainMenuScene);
        // UIManager.Instance.GetFadeUIController().FadeIn(1.5f); // Fade In when loading into the main menu scene.
    }

#endregion
}
