using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StoreMenuController : MonoBehaviour
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

    public void Button_ReturnToMenu()
    {
        SceneLoadingManager.Instance?.LoadSceneMainMenu();
    }

    #endregion

    #region PrivateMethods
    #endregion
}
