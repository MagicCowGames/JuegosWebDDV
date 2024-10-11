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
        // NOTE : You don't have to worry about the manager instance not existing when calling this on the init scene, because we're calling it from Start(),
        // and the manager instance is created on Awake(), which means that the order in which the object calls are done by Unity does not matter, since
        // all of the Awake() stage calls will take place before all of the Start() calls.
        SceneLoadingManager.Instance?.LoadSceneMainMenu();
    }

    void Update()
    {
        
    }

    #endregion

    #region PublicMethods
    #endregion

    #region PrivateMethods
    #endregion
}
