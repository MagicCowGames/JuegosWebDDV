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
        // is initialized on Awake(), and this Init() method runs on Start().
#if UNITY_EDITOR
        // If compiling the editor build, always set debug to true.
        DebugManager.Instance?.SetDebugEnabled(true);
#else
        // If compiling the release build, always set debug to false by default.
        // Just to make sure that I don't forget to set the debug to false at some point in the inspector panel before making a release build.
        DebugManager.Instance?.SetDebugEnabled(false);
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
        SceneLoadingManager.Instance?.LoadSceneMainMenu();
    }

#endregion
}
