using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoginMenuController : MonoBehaviour
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
        var name = this.inputBox.GetName();
        var password = this.inputBox.GetPassword();
        AccountManager.Instance?.AccessAccount(name, password);
    }

    #endregion

    #region PrivateMethods
    #endregion
}
