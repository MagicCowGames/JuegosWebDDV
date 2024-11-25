using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialMenuController : MonoBehaviour
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
        SceneLoadingManager.Instance?.LoadSceneMainMenu();
    }

    public void Button_TutorialMenu()
    {
        SceneLoadingManager.Instance?.LoadScene("MenuScene_Tutorial_Pages");
    }

    public void Button_TutorialLevel()
    {
        SceneLoadingManager.Instance?.LoadScene("TutorialMap");
    }

    #endregion

    #region PrivateMethods
    #endregion
}
