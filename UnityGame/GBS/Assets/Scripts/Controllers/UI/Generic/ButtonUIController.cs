using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

// TODO : Rename this class, since the UIController nomenclature is sort of reserved in this project for classes that actually inherit from the UIController class...
// Maybe it's time to start using fucking namespaces, eh? Retard...

[ExecuteInEditMode]
public class ButtonUIController : MonoBehaviour, IComponentValidator
{
    #region Variables

    [Header("Components - Button")]
    [SerializeField] private Button button;
    [SerializeField] private Image buttonShadow;

    [Header("Components - Text")]
    [SerializeField] private TMP_Text text;
    [SerializeField] private TMP_Text textShadow;
    [SerializeField] private LocalizedTextController localizedTextController;

    [Header("Settings - Button")]
    [SerializeField] private Button.ButtonClickedEvent onClick;
    [SerializeField] private bool buttonHasShadow;

    [Header("Settings - Text")]
    [SerializeField] private string localizationString;
    [SerializeField] private bool textHasShadow;

    #endregion

    #region MonoBehaviour

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    void OnValidate()
    {
        if (!AllComponentsAreValid())
            return;

        UpdateButton();
        UpdateText();
    }

    #endregion

    #region PublicMethods

    public bool AllComponentsAreValid()
    {
        return
            this.button != null &&
            this.buttonShadow != null &&
            this.text != null &&
            this.textShadow != null
            ;
    }

    #endregion

    #region PrivateMethods

    private void UpdateButton()
    {
        this.button.onClick = this.onClick;
        this.buttonShadow?.gameObject.SetActive(this.buttonHasShadow);
    }

    private void UpdateText()
    {
        this.localizedTextController.LocalizationString = this.localizationString;
        this.textShadow?.gameObject.SetActive(this.textHasShadow);
    }

    #endregion
}
