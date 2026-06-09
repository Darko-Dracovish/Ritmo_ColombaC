using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class NPCDialogue : MonoBehaviour
{
    public enum InteractionType { Desafio, Nivel }

    [Header("Tipo de interacción")]
    public InteractionType interactionType = InteractionType.Desafio;

    [Header("Diálogo")]
    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;
    public string dialogue;

    [Header("Desafío (secuencia prehecha)")]
    public List<GameObject> challengeCards;
    public List<GameObject> challengeRewards;
    public int challengeObjective;

    [Header("Nivel (el jugador construye la secuencia)")]
    public int levelObjective;

    [Header("Desbloqueo al completar")]
    // NPCs que se desbloquean cuando el jugador completa este desafío/nivel
    public List<NPCDialogue> unlockOnComplete = new List<NPCDialogue>();

    [Header("Bloqueo")]
    public bool isLocked = false;
    public string lockedMessage = "Aún no puedes enfrentarte a este pájaro.";
    public GameObject lockedPanel;
    public TextMeshProUGUI lockedText;

    void OnMouseDown()
    {
        if (currentOpen != null && currentOpen != this)
        {
            currentOpen.dialoguePanel.SetActive(false);
            if (currentOpen.lockedPanel != null)
                currentOpen.lockedPanel.SetActive(false);
        }

        currentOpen = this;

        if (isLocked)
            OpenLockedPanel();
        else
            OpenDialogue();

        Debug.Log("Click en NPC: " + gameObject.name);
    }

    void OpenDialogue()
    {
        if (dialogueText != null)
            dialogueText.text = dialogue;

        dialoguePanel.SetActive(true);
    }

    void OpenLockedPanel()
    {
        if (lockedText != null)
            lockedText.text = lockedMessage;

        lockedPanel.SetActive(true);
    }

    public void CloseLockedPanel()
    {
        lockedPanel.SetActive(false);
        currentOpen = null;
    }

    // Botón Aceptar del panel de diálogo
    public void OnAccept()
    {
        Debug.Log($"[{gameObject.name}] OnAccept — tipo: {interactionType}");

        dialoguePanel.SetActive(false);
        currentOpen = null;

        if (interactionType == InteractionType.Desafio)
        {
            GameManager.instance.StartChallenge(challengeCards, challengeRewards, challengeObjective, this);
        }
        else
        {
            GameManager.instance.StartLevel(levelObjective, this);
            // Mostrar confirmación brevemente en pantalla
            GameManager.instance.ShowMessage($"Nuevo objetivo: {levelObjective} puntos");
        }
    }

    public void OnReject()
    {
        dialoguePanel.SetActive(false);
        currentOpen = null;
    }

    // Desbloquea los NPCs enlazados
    public void UnlockNext()
    {
        foreach (NPCDialogue npc in unlockOnComplete)
        {
            if (npc != null)
            {
                npc.isLocked = false;
                Debug.Log($"NPC desbloqueado: {npc.gameObject.name}");
            }
        }
    }

    public static NPCDialogue currentOpen;
}
