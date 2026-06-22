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
    public GameObject nextButton;       // Avanza a la siguiente línea
    public GameObject acceptButton;     // Aparece solo en la última línea
    public GameObject rejectButton;     // Aparece solo en la última línea

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

    // isLast: si es la última línea muestra Aceptar/Rechazar, si no muestra Siguiente
    public void SetButtons(bool isLast)
    {
        if (acceptButton != null) acceptButton.SetActive(isLast);
        if (rejectButton != null) rejectButton.SetActive(isLast);
    }

    public void Hide()
    {
        if (dialoguePanel.activeSelf) UIBlocker.Close();
        dialoguePanel.SetActive(false);
    }

    // Se llama al hacer clic en cualquier parte del panel
    public void OnPointerClick(PointerEventData eventData)
    {
        // Ignorar si el click fue sobre un botón
        if (eventData.pointerCurrentRaycast.gameObject != null)
        {
            var btn = eventData.pointerCurrentRaycast.gameObject.GetComponentInParent<UnityEngine.UI.Button>();
            if (btn != null) return;
        }
        OnNext();
    }

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

    // Conectar al botón "Rechazar"
    public void OnReject()
    {
        if (NPCDialogue.currentOpen != null)
            NPCDialogue.currentOpen.OnReject();
    }
}
