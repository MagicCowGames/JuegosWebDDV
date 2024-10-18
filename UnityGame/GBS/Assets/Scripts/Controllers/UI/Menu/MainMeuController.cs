using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMeuController : MonoBehaviour
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

    public void LoadCredits()
    {
        SceneLoadingManager.Instance?.LoadSceneCredits();
    }

    public void LoadGame()
    {
        SceneLoadingManager.Instance?.LoadScene("DebugSceneGameplay");
    }

    public void LoadAccountMenu()
    {
        SceneLoadingManager.Instance?.LoadScene("MS_Account");
    }

    #endregion

    #region PrivateMethods
    #endregion
}
