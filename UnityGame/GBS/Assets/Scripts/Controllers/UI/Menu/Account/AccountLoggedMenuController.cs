using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AccountLoggedMenuController : UIController
{
    #region Variables

    [Header("Account Logged Menu Controller")]
    [SerializeField] private TMP_Text[] usernameTexts;

    #endregion

    #region MonoBehaviour

    void Start()
    {
        UpdateUsernameTexts();
    }

    void Update()
    {
        
    }

    #endregion

    #region PublicMethods

    public void Button_Return()
    {
        SceneLoadingManager.Instance?.LoadSceneMainMenu();
    }

    #endregion

    #region PrivateMethods

    private void UpdateUsernameTexts()
    {
        foreach (var text in this.usernameTexts)
            if (text != null)
                text.text = AccountManager.Instance.Account.name;
    }

    #endregion
}
