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
    #region Enums

    public enum ButtonType
    {
        Long = 0,
        Short
    }

    #endregion

    #region Variables

    // TODO : Maybe, for easier handling, separate all of the component getting logic into some sort of internal logic component or whatever, and
    // have another component for user facing configuration.

    [Header("Components - Button")]
    [SerializeField] private Button button;
    [SerializeField] private Image buttonImage;
    [SerializeField] private Image buttonShadow;

    [Header("Components - Text")]
    [SerializeField] private TMP_Text text;
    [SerializeField] private TMP_Text textShadow;
    [SerializeField] private LocalizedTextController localizedTextController;

    // TODO : Move this to some sort of global sprite manager so that we don't have a gazillion copies of this data lying around.
    [Header("Data - Button")]
    [SerializeField] private Sprite spriteButtonShort;
    [SerializeField] private Sprite spriteButtonLong;

    [Header("Settings - Button")]
    [SerializeField] private Button.ButtonClickedEvent onClick;
    [SerializeField] private bool buttonHasShadow;
    [SerializeField] private ButtonType buttonType;

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
            this.textShadow != null &&
            this.localizedTextController != null
            ;
    }

    #endregion

    #region PrivateMethods

    private void UpdateButton()
    {
        this.button.onClick = this.onClick;
        this.buttonShadow.gameObject.SetActive(this.buttonHasShadow);

        Sprite chosen;
        switch (this.buttonType)
        {
            default:
            case ButtonType.Long:
                chosen = this.spriteButtonLong;
                break;
            case ButtonType.Short:
                chosen = this.spriteButtonShort;
                break;
        }
        this.buttonImage.sprite = chosen;
        this.buttonShadow.sprite = chosen;
    }

    private void UpdateText()
    {
        this.localizedTextController.LocalizationString = this.localizationString;
        this.textShadow.gameObject.SetActive(this.textHasShadow);
    }

    #endregion
}
