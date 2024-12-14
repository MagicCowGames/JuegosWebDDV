using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueController : MonoBehaviour
{
    #region Variables

    [SerializeField] private DialogueSequence dialogueSequence;

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

    public void StartDialogue()
    {
        UIManager.Instance.GetDialogueUIController().DisplayDialogueSequence(this.dialogueSequence);
    }

    #endregion

    #region PrivateMethods
    #endregion
}
