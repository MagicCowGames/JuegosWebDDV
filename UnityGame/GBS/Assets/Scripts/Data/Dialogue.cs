using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public struct Dialogue
{
    public Sprite characterSprite;
    public string name;
    public string text;
    public UnityEvent onDialogueStart;
    public UnityEvent onDialogueEnd;

    public Dialogue(Sprite characterSprite, string name, string text, UnityEvent onDialogueStart, UnityEvent onDialogueEnd)
    {
        this.characterSprite = characterSprite;
        this.name = name;
        this.text = text;
        this.onDialogueStart = onDialogueStart;
        this.onDialogueEnd = onDialogueEnd;
    }
}

[System.Serializable]
public struct DialogueSequence
{
    public Dialogue[] dialogues;

    public DialogueSequence(Dialogue[] dialogues)
    {
        this.dialogues = dialogues;
    }
}
