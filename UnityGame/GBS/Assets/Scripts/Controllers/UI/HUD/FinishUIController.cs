using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishUIController : UIController
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
