using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FinishUIController : UIController
{
    #region Variables

    [Header("Finish UI Controller")]
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text moneyText;

    #endregion

    #region MonoBehaviour

    void Start()
    {

    }

    void Update()
    {
        UpdateScoreText();
        UpdateMoneyText();
    }

    #endregion

    #region PublicMethods

    public void Button_ReturnToMenu()
    {
        SceneLoadingManager.Instance?.LoadSceneMainMenu();
    }

    #endregion

    #region PrivateMethods

    private void UpdateScoreText()
    {
        if (this.scoreText != null)
            this.scoreText.text = $"{PlayerDataManager.Instance.GetPlayerScore().Score} pts";
    }

    private void UpdateMoneyText()
    {
        if (this.moneyText != null)
            this.moneyText.text = $"${PlayerDataManager.Instance.GetPlayerMoney().Money}";
    }

    #endregion
}
