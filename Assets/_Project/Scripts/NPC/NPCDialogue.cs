using UnityEngine;

[CreateAssetMenu(fileName = "New NPC Dialogue", menuName = "NPC Dialogue")]
public class NPCDialogue : ScriptableObject
{
    public string npcName;
    public Sprite npcPortrait;
    public string[] dialogueLines;
    public bool[] autoProgressLines;
    public bool[] endDialogueLines;
    public float typingSpeed = 0.05f;
    public AudioClip voiceSound;
    public float voicePitch = 1f;
    public float autoProgressDelay = 1.5f;
    public DialogueChoice[] dialogueChoicesArray;
}

[System.Serializable]
public class DialogueChoice
{
    public int dialogueIndex;
    public string[] choicesArray;
    public int[] nextDialogueIndices;
}