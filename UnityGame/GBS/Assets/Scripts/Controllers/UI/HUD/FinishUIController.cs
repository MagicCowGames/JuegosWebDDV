using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FinishUIController : UIController, IComponentValidator
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
        if (!AllComponentsAreValid())
            return;

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
        this.scoreText.text = $"{PlayerDataManager.Instance.GetPlayerScore().Score} pts";
    }

    private void UpdateMoneyText()
    {
        this.moneyText.text = $"${PlayerDataManager.Instance.GetPlayerMoney().Money}";
    }

    #endregion

    #region IComponentValidator

    public bool AllComponentsAreValid()
    {
        return
            PlayerDataManager.Instance != null &&
            PlayerDataManager.Instance.GetPlayerMoney() != null &&
            PlayerDataManager.Instance.GetPlayerScore() != null &&
            this.moneyText != null &&
            this.scoreText != null
            ;
    }

    #endregion
}
