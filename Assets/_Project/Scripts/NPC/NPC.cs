using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NPC : MonoBehaviour, IInteractable
{
    [SerializeField] private NPCDialogue _dialogueData;

    private WaitForSeconds _typingSpeed;
    private WaitForSeconds _autoProgressDelay;
    private int _dialogueIndex;
    private bool _isTyping;
    private bool _isDialogueActive;

    private void Awake()
    {
        _typingSpeed = new(_dialogueData.typingSpeed);
        _autoProgressDelay = new(_dialogueData.autoProgressDelay);
    }

    public bool CanInteract()
    {
        return !_isDialogueActive;
    }

    public void Interact()
    {
        if (_dialogueData == null || (PauseManager.IsGamePaused && !_isDialogueActive)) return;

        if (_isDialogueActive)
            NextLine();
        else
            StartDialogue();
    }

    public void EndDialogue()
    {
        StopAllCoroutines();
        _isDialogueActive = false;
        DialogueManager.Instance.SetDialogueText("");
        DialogueManager.Instance.ShowDialogueUI(false);
        PauseManager.SetPause(false);
    }

    private IEnumerator TypeLineCoroutine()
    {
        _isTyping = true;
        DialogueManager.Instance.SetDialogueText("");

        foreach (char letter in _dialogueData.dialogueLines[_dialogueIndex])
        {
            DialogueManager.Instance.SetDialogueText(DialogueManager.Instance.DialogueText.text += letter);
            SoundEffectManager.PlayVoice(_dialogueData.voiceSound, _dialogueData.voicePitch);
            yield return _typingSpeed;
        }

        _isTyping = false;

        if (_dialogueData.autoProgressLines.Length > _dialogueIndex && _dialogueData.autoProgressLines[_dialogueIndex])
        {
            yield return _autoProgressDelay;
            NextLine();
        }
    }

    private void StartDialogue()
    {
        _isDialogueActive = true;
        _dialogueIndex = 0;
        DialogueManager.Instance.SetNPCInfo(_dialogueData.npcName, _dialogueData.npcPortrait);
        DialogueManager.Instance.ShowDialogueUI(true);
        PauseManager.SetPause(true);

        DisplayCurrentLine();
    }

    private void NextLine()
    {
        if (_isTyping)
        {
            StopAllCoroutines();
            DialogueManager.Instance.SetDialogueText(_dialogueData.dialogueLines[_dialogueIndex]);
            _isTyping = false;
        }

        DialogueManager.Instance.ClearChoices();

        if (_dialogueData.endDialogueLines.Length > _dialogueIndex && _dialogueData.endDialogueLines[_dialogueIndex])
        {
            EndDialogue();
            return;
        }
        // Check if we have choices & display
        foreach (DialogueChoice dialogueChoice in _dialogueData.dialogueChoicesArray)
        {
            if (dialogueChoice.dialogueIndex == _dialogueIndex)
            {
                DisplayChoices(dialogueChoice);
                return;
            }
        }

        if (++_dialogueIndex < _dialogueData.dialogueLines.Length)
            DisplayCurrentLine();
        else
            EndDialogue();
    }

    private void DisplayChoices(DialogueChoice dialogueChoice)
    {
        for (int i = 0; i < dialogueChoice.choicesArray.Length; i++)
        {
            int nextIndex = dialogueChoice.nextDialogueIndices[i];
            DialogueManager.Instance.CreateChoiceButton(dialogueChoice.choicesArray[i], () => ChooseOption(nextIndex));
        }
    }

    private void ChooseOption(int nextIndex)
    {
        _dialogueIndex = nextIndex;
        DialogueManager.Instance.ClearChoices();
        DisplayCurrentLine();
    }

    private void DisplayCurrentLine()
    {
        StopAllCoroutines();
        StartCoroutine(TypeLineCoroutine());
    }
}