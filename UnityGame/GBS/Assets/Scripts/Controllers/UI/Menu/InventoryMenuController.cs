using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InventoryMenuController : MonoBehaviour
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

    public void Button_WIP()
    {
        UIManager.Instance.GetPopUpUIController().Open("loc_wip_title", "loc_wip_message");
    }

    #endregion

    #region PrivateMethods
    #endregion
}
