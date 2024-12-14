using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// TODO : Implement
public class DialogueUIController : UIController
{
    #region Variables

    [Header("Dialogue UI Controller")]
    [SerializeField] private TMP_Text dialogueText;

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
    #endregion

    #region PrivateMethods

    private void SetDisplayCharacters(int amount)
    {
        this.dialogueText.maxVisibleCharacters = amount;
    }

    #endregion
}
