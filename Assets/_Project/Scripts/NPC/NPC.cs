using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NPC : MonoBehaviour, IInteractable
{
    [SerializeField] private NPCDialogue _dialogueData;
    [SerializeField] private GameObject _dialoguePanel;
    [SerializeField] private TMP_Text _dialogueText;
    [SerializeField] private TMP_Text _nameText;
    [SerializeField] private Image _portraitImage;

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
        _dialogueText.SetText("");
        _dialoguePanel.SetActive(false);
        PauseManager.SetPause(false);
    }

    private void StartDialogue()
    {
        _isDialogueActive = true;
        _dialogueIndex = 0;
        _nameText.SetText(_dialogueData.npcName);
        _portraitImage.sprite = _dialogueData.npcPortrait;
        _dialoguePanel.SetActive(true);
        PauseManager.SetPause(true);

        StartCoroutine(TypeLineCoroutine());
    }

    private void NextLine()
    {
        if (_isTyping)
        {
            StopAllCoroutines();
            _dialogueText.SetText(_dialogueData.dialogueLines[_dialogueIndex]);
            _isTyping = false;
        }
        else if (++_dialogueIndex < _dialogueData.dialogueLines.Length)
            StartCoroutine(TypeLineCoroutine());
        else
            EndDialogue();
    }

    private IEnumerator TypeLineCoroutine()
    {
        _isTyping = true;
        _dialogueText.SetText("");

        foreach (char letter in _dialogueData.dialogueLines[_dialogueIndex])
        {
            _dialogueText.text += letter;
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
}