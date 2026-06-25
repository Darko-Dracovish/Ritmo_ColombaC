using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class DialoguePanelUI : MonoBehaviour, IPointerClickHandler
{
    public static DialoguePanelUI instance;

    [Header("Referencias del panel")]
    public GameObject dialoguePanel;
    public TextMeshProUGUI speakerText;
    public TextMeshProUGUI dialogueText;

    [Header("Botones")]
    public GameObject nextButton;
    public GameObject backButton;
    public GameObject acceptButton;
    public GameObject rejectButton;

    void Awake()
    {
        instance = this;
    }

    void OnEnable()
    {
        instance = this;
    }

    public void ShowLine(DialogueLine line)
    {
        if (!dialoguePanel.activeSelf) UIBlocker.Open();
        dialoguePanel.SetActive(true);
        if (speakerText != null)
        {
            speakerText.gameObject.SetActive(true);
            speakerText.text = line.speaker;
        }
        if (dialogueText != null)
        {
            dialogueText.gameObject.SetActive(true);
            dialogueText.text = line.text;
        }
    }

    public void SetButtons(bool isLast, bool isFirst)
    {
        if (acceptButton != null) acceptButton.SetActive(isLast);
        if (rejectButton != null) rejectButton.SetActive(isLast);
        if (backButton != null) backButton.SetActive(!isFirst);
    }

    public void Hide()
    {
        if (dialoguePanel.activeSelf) UIBlocker.Close();
        dialoguePanel.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData) { }

    public void OnNext()
    {
        if (NPCDialogue.currentOpen == null) return;
        if (!NPCDialogue.currentOpen.isLastLine())
            NPCDialogue.currentOpen.NextLine();
    }

    // Conectar al botón "Aceptar"
    public void OnAccept()
    {
        if (NPCDialogue.currentOpen != null)
            NPCDialogue.currentOpen.OnAccept();
    }

    public void OnBack()
    {
        if (NPCDialogue.currentOpen != null)
            NPCDialogue.currentOpen.PreviousLine();
    }

    public void OnReject()
    {
        if (NPCDialogue.currentOpen != null)
            NPCDialogue.currentOpen.OnReject();
    }
}
