using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AccountLoggedMenuController : UIController
{
    #region Variables

    [Header("Account Logged Menu Controller")]
    [SerializeField] private TMP_Text[] usernameTexts;
    [SerializeField] private TMP_Text moneyText;
    [SerializeField] private TMP_Text scoreText;

    #endregion

    #region MonoBehaviour

    void Start()
    {
        UpdateTexts();
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

    public void Button_LogOut()
    {
        AccountManager.Instance?.LogOut();
    }

    #endregion

    #region PrivateMethods

    private void UpdateTexts()
    {
        UpdateUsernameText();
        UpdateMoneyText();
        UpdateScoreText();
    }

    private void UpdateUsernameText()
    {
        foreach (var text in this.usernameTexts)
            if (text != null)
                text.text = AccountManager.Instance.Account.name;
    }

    private void UpdateMoneyText()
    {
        if (this.moneyText != null)
            this.moneyText.text = $"${AccountManager.Instance.Account.money}";
    }
    private void UpdateScoreText()
    {
        if (this.moneyText != null)
            this.moneyText.text = $"{AccountManager.Instance.Account.score} pts";
    }

    #endregion
}
