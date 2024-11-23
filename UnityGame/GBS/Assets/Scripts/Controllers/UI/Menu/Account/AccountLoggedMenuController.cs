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

    private User userData;

    #endregion

    #region MonoBehaviour

    void Start()
    {
        UpdateMenu();
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

    public void Button_DeleteAccount()
    {
        SceneLoadingManager.Instance?.LoadScene("MS_A_Delete");
    }

    #endregion

    #region PrivateMethods

    private void UpdateMenu()
    {
        this.userData = new User();
        UpdateTexts(false);
        FetchAccountData();
    }

    #endregion

    #region PrivateMethods - Texts

    private void UpdateTexts(bool useValues)
    {
        UpdateUsernameText(useValues);
        UpdateMoneyText(useValues);
        UpdateScoreText(useValues);
    }

    private void UpdateUsernameText(bool useValues)
    {
        string txt = useValues ? this.userData.name : "...";
        foreach (var text in this.usernameTexts)
            if (text != null)
                text.text = txt;
    }

    private void UpdateMoneyText(bool useValues)
    {
        if (this.moneyText != null)
        {
            this.moneyText.text = useValues ? $"${this.userData.money}" : "...";
        }
    }
    private void UpdateScoreText(bool useValues)
    {
        if (this.scoreText != null)
        {
            this.scoreText.text = useValues ? $"{this.userData.score} pts" : "...";
        }
    }

    #endregion

    #region PrivateMethods - Network

    // Fetches the account data from the server to make sure we always have the most up to date information displayed on screen.
    private void FetchAccountData()
    {
        var id = AccountManager.Instance.Account.id;
        var callbacks = new ConnectionManager.RequestCallbacks();
        callbacks.OnSuccess += (msg) => {
            var user = JsonUtility.FromJson<User>(msg);
            this.userData = user;
            UpdateTexts(true);
        };
        callbacks.OnError += (err) => {
            UpdateTexts(true);
        };
        callbacks.OnConnectionError += () => {
            UpdateTexts(true);
        };
        ConnectionManager.Instance.MakeRequestToServer("GET", $"/users/{id}", callbacks);
    }

    #endregion
}
