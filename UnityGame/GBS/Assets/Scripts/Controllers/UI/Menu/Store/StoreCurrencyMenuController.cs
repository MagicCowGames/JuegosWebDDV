using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StoreCurrencyMenuController : MonoBehaviour
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

    public void Button_ReturnToStoreMenu()
    {
        SceneLoadingManager.Instance?.LoadSceneStore();
    }

    public void Button_WIP_Transaction()
    {
        UIManager.Instance.GetPopUpUIController().Open("loc_transaction", "loc_transaction_success_msg");
    }

    #endregion

    #region PrivateMethods
    #endregion
}
