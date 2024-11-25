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

    public void Button_Return()
    {
        SceneLoadingManager.Instance?.LoadSceneTutorial();
    }

    public void Button_Next()
    { }

    public void Button_Previous()
    { }

    #endregion

    #region PrivateMethods
    #endregion
}
