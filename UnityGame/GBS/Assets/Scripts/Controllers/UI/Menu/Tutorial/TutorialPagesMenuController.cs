using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialPagesMenuController : MonoBehaviour
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

    public void ReturnToMenu()
    {
        SceneLoadingManager.Instance?.LoadSceneTutorial();
    }

    #endregion

    #region PrivateMethods
    #endregion
}
