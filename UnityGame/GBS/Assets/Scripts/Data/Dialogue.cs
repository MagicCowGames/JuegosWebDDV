using UnityEngine;

[System.Serializable]
public struct Dialogue
{
    public Sprite characterSprite;
    public string name;
    public string text;

    public Dialogue(Sprite characterSprite, string name, string text)
    {
        this.characterSprite = characterSprite;
        this.name = name;
        this.text = text;
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
