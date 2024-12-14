using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// TODO : Implement
public class DialogueUIController : UIController
{
    #region Variables

    [Header("Dialogue UI Controller")]
    [SerializeField] private Image characterImage;
    [SerializeField] private TMP_Text characterName;
    [SerializeField] private TMP_Text dialogueText;

    private DialogueSequence dialogueSequence;
    private DialogueSequence fallbackSequence = new DialogueSequence(); // NOTE : This one exists in case that the input dialogue sequence is not valid

    private float timeAccumulator = 0.0f;
    private float timePerCharacter = 0.05f;

    private int currentDialogue = 0;
    private int numDialogues = 0;

    #endregion

    #region MonoBehaviour

    void Start()
    {
        // Crappy
        this.fallbackSequence.dialogues = new Dialogue[] {new Dialogue()};
        this.fallbackSequence.dialogues[0].name = "None";
        this.fallbackSequence.dialogues[0].text = "Nome";
    }

    void Update()
    {
        float delta = Time.deltaTime;
        this.timeAccumulator += delta;
        SetDialogueDisplayCharacters((int)(this.timeAccumulator / this.timePerCharacter)); // Shitty solution, will eventually overflow...
    }

    #endregion

    #region PublicMethods

    public void DisplayDialogueSequence(DialogueSequence dialogues)
    {
        if (dialogues.dialogues == null)
            this.dialogueSequence = this.fallbackSequence;
        else
            this.dialogueSequence = dialogues;
        StartDialogueSequence();
    }

    public void Skip() // This is the public function that will be called by the button when the player presses it on the UI.
    {
        DisplayNextDialogue();
    }

    #endregion

    #region PrivateMethods

    private void DisplayDialogue(int idx)
    {
        Dialogue dialogue = this.dialogueSequence.dialogues[idx];
        SetCharacterSprite(dialogue.characterSprite);
        SetCharacterName(dialogue.name);
        SetDialogueText(dialogue.text);
        SetDialogueDisplayCharacters(0);
        this.timeAccumulator = 0.0f;
    }

    private void DisplayNextDialogue()
    {
        ++this.currentDialogue;
        if (this.currentDialogue >= this.numDialogues)
        {
            ExitDialogue();
        }
        else
        {
            DisplayDialogue(this.currentDialogue);
        }
    }

    private void StartDialogueSequence()
    {
        this.currentDialogue = -1; // Yet another fucking hack because we must rush all of the code for the deadlines!!! Rush rush to the yeyo!!!
        this.numDialogues = this.dialogueSequence.dialogues.Length;
        EnterDialogue();
        DisplayNextDialogue();
    }

    #endregion

    #region PrivateMethods - Dialogue Box Settings

    // Functionality dedicated toward setting the contents of the dialogue box.

    private void SetDialogueText(string str)
    {
        this.dialogueText.text = str;
    }

    private void SetCharacterName(string str)
    {
        this.characterName.text = str;
    }

    private void SetCharacterSprite(Sprite sprite)
    {
        this.characterImage.sprite = sprite;
    }

    private void SetDialogueDisplayCharacters(int amount)
    {
        this.dialogueText.maxVisibleCharacters = amount;
    }

    #endregion

    #region PrivateMethods - Enter / Exit (player control handling)

    private void EnterDialogue()
    {
        UI_SetVisible(true);
        PlayerDataManager.Instance.GetPlayer().ControlsEnabled = false;
    }

    private void ExitDialogue()
    {
        UI_SetVisible(false);
        PlayerDataManager.Instance.GetPlayer().ControlsEnabled = true;
    }

    #endregion
}
