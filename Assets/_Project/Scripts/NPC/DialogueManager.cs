using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }

    [field: SerializeField] public TMP_Text DialogueText { get; private set; }

    [SerializeField] private GameObject _dialoguePanel;
    [SerializeField] private TMP_Text _nameText;
    [SerializeField] private Image _portraitImage;
    [SerializeField] private Transform _choicePanel;
    [SerializeField] private GameObject _choiceButtonPrefab;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void ShowDialogueUI(bool show)
    {
        _dialoguePanel.SetActive(show);
    }

    public void SetNPCInfo(string npcName, Sprite portrait)
    {
        _nameText.text = npcName;
        _portraitImage.sprite = portrait;
    }

    public void SetDialogueText(string text)
    {
        DialogueText.text = text;
    }

    public void ClearChoices()
    {
        foreach (Transform child in _choicePanel)
        {
            Destroy(child.gameObject);
        }
    }
    
    public void CreateChoiceButton(string choiceText, UnityEngine.Events.UnityAction onClick)
    {
        GameObject choiceButton = Instantiate(_choiceButtonPrefab, _choicePanel);
        choiceButton.GetComponentInChildren<TMP_Text>().text = choiceText;
        choiceButton.GetComponent<Button>().onClick.AddListener(onClick);
    }
}