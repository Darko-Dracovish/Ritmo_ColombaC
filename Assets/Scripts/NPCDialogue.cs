using UnityEngine;
using System.Collections.Generic;
using TMPro;

[System.Serializable]
public class DialogueLine
{
    public string speaker;
    [TextArea(2, 5)] public string text;
}

[System.Serializable]
public class DialogueSequence
{
    public string sequenceId;
    public NPCDialogue.InteractionType interactionType = NPCDialogue.InteractionType.Solo;
    public List<DialogueLine> lines = new List<DialogueLine>();
}

public class NPCDialogue : MonoBehaviour
{
    public enum InteractionType { Desafio, Nivel, Solo }

    [Header("Tipo de interacción")]
    public InteractionType interactionType = InteractionType.Desafio;

    [Header("Panel de diálogo")]
    public DialoguePanelUI dialoguePanelUI;

    [Header("Diálogos (el juego elige cuál mostrar con dialogueIndex)")]
    public List<DialogueSequence> dialogues = new List<DialogueSequence>();
    [HideInInspector] public int dialogueIndex = 0;

    [Header("Desafío (secuencia prehecha)")]
    public List<GameObject> challengeCards;
    public List<GameObject> challengeRewards;
    public int challengeObjective;

    [Header("Nivel (el jugador construye la secuencia)")]
    public int levelObjective;

    [Header("Desbloqueo al completar")]
    public List<NPCDialogue> unlockOnComplete = new List<NPCDialogue>();

    [Header("NPCs que avanzan su diálogo al completar esta sesión")]
    public List<NPCDialogue> advanceDialogueOnComplete = new List<NPCDialogue>();

    [Header("Bailarines")]
    public DancerSlot[] dancers;
    public string danceTrigger = "Dance";
    public string idleTrigger = "Idle";

    [Header("Bloqueo")]
    public bool isLocked = false;
    public string lockedMessage = "Aún no puedes enfrentarte a este pájaro.";
    public GameObject lockedPanel;
    public TextMeshProUGUI lockedText;

    // Estado interno de la conversación
    [HideInInspector] public int currentLine = 0;

    public static NPCDialogue currentOpen;

    void OnMouseDown()
    {
        if (SettingsPanel.isOpen) return;
        Debug.Log($"[NPCDialogue] Click en {gameObject.name} | isLocked: {isLocked} | diálogos: {dialogues.Count}");

        if (currentOpen != null && currentOpen != this)
            currentOpen = null;

        currentOpen = this;

        if (isLocked)
            OpenLockedPanel();
        else
            StartDialogue();
    }

    void StartDialogue()
    {
        currentLine = 0;

        if (dialoguePanelUI == null)
        {
            Debug.LogError($"[{gameObject.name}] dialoguePanelUI no asignado en el Inspector.");
            return;
        }

        if (dialogues == null || dialogues.Count == 0 || dialogueIndex >= dialogues.Count)
        {
            Debug.LogWarning($"[{gameObject.name}] Sin diálogos configurados.");
            return;
        }

        dialoguePanelUI.ShowLine(GetCurrentLine());
        dialoguePanelUI.SetButtons(isLastLine());
    }

    public void NextLine()
    {
        currentLine++;

        if (currentLine >= GetCurrentSequence().lines.Count)
        {
            // No debería llegar aquí — el botón Next se oculta en la última línea
            return;
        }

        dialoguePanelUI?.ShowLine(GetCurrentLine());
        dialoguePanelUI?.SetButtons(isLastLine());
    }

    public void OnAccept()
    {
        Debug.Log($"[{gameObject.name}] OnAccept — tipo: {interactionType}");

        dialoguePanelUI?.Hide();
        currentOpen = null;

        var tipo = GetCurrentSequence().interactionType;

        if (tipo == InteractionType.Desafio)
            GameManager.instance.StartChallenge(challengeCards, challengeRewards, challengeObjective, this);
        else if (tipo == InteractionType.Nivel)
        {
            GameManager.instance.StartLevel(levelObjective, this);
            GameManager.instance.ShowMessage($"Nuevo objetivo: {levelObjective} puntos");
        }
        // Solo: solo cierra el panel, no hace nada más
    }

    public void OnReject()
    {
        dialoguePanelUI?.Hide();
        currentOpen = null;
    }

    void OpenLockedPanel()
    {
        if (lockedText != null) lockedText.text = lockedMessage;
        if (lockedPanel != null) lockedPanel.SetActive(true);
    }

    public void CloseLockedPanel()
    {
        if (lockedPanel != null) lockedPanel.SetActive(false);
        currentOpen = null;
    }

    public void StartDance()
    {
        foreach (var dancer in dancers)
            dancer?.Activate(danceTrigger);
    }

    public void StopDance()
    {
        foreach (var dancer in dancers)
            dancer?.Deactivate(idleTrigger);
    }

    public void AdvanceDialogue()
    {
        if (dialogueIndex < dialogues.Count - 1)
            dialogueIndex++;
    }

    public void UnlockNext()
    {
        foreach (NPCDialogue npc in unlockOnComplete)
            if (npc != null) npc.isLocked = false;

        Debug.Log($"[{gameObject.name}] UnlockNext — advanceDialogueOnComplete: {advanceDialogueOnComplete.Count}");
        foreach (NPCDialogue npc in advanceDialogueOnComplete)
        {
            if (npc != null)
            {
                Debug.Log($"[{gameObject.name}] Avanzando diálogo de {npc.gameObject.name} (antes: {npc.dialogueIndex})");
                npc.AdvanceDialogue();
                Debug.Log($"[{gameObject.name}] Diálogo de {npc.gameObject.name} ahora: {npc.dialogueIndex}");
            }
        }
    }

    DialogueSequence GetCurrentSequence()
    {
        return dialogues[Mathf.Clamp(dialogueIndex, 0, dialogues.Count - 1)];
    }

    DialogueLine GetCurrentLine()
    {
        var seq = GetCurrentSequence();
        return seq.lines[Mathf.Clamp(currentLine, 0, seq.lines.Count - 1)];
    }

    public bool isLastLine()
    {
        return currentLine >= GetCurrentSequence().lines.Count - 1;
    }
}
