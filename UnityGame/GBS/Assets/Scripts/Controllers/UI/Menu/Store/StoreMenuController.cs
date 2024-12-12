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

    public void Button_StoreItems()
    {
        SceneLoadingManager.Instance?.LoadScene("MS_Store_Items");
    }

    public void Button_StoreCurrency()
    {
        SceneLoadingManager.Instance?.LoadScene("MS_Store_Currency");
    }

    #endregion

    #region PrivateMethods
    #endregion
}
