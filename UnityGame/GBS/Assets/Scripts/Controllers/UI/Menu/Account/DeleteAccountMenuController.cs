using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeleteAccountMenuController : MonoBehaviour
{
    #region Variables

    [SerializeField] UserDataInputBoxController inputBox;

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

    public void Button_Back()
    {
        SceneLoadingManager.Instance?.LoadScene("MS_Account");
    }

    public void Button_Submit()
    {
        var password = this.inputBox.GetPassword();
        AccountManager.Instance?.DeleteAccount(password);
    }

    #endregion

    #region PrivateMethods
    #endregion
}
