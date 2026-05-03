using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class NPCDialogue : MonoBehaviour
{
    public static NPCDialogue currentOpen;

    [Header("Dialogo")]
    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;
    public string dialogue;


    [Header("Desafío")]
    public List<GameObject> challengeCards;
    public List<GameObject> challengeRewards;
    public int challengeObjective;

    [Header("Bloqueo")]
    public bool isLocked = false;
    public string lockedMessage = "Aún no puedes enfrentarte a este pájaro.";
    public GameObject lockedPanel;
    public TextMeshProUGUI lockedText;

    void OnMouseDown()
    {
        // Cerrar panel abierto anteriormente
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
        Debug.Log("dialoguePanel: " + (dialoguePanel != null ? dialoguePanel.name : "NULL"));

        if (dialogueText != null)
            dialogueText.text = dialogue;

        dialoguePanel.SetActive(true);
        Debug.Log("Panel activo: " + dialoguePanel.activeSelf);
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

    public void OnAccept()
    {
        dialoguePanel.SetActive(false);
        GameManager.instance.StartChallenge(challengeCards, challengeRewards, challengeObjective);
    }

    public void OnReject()
    {
        dialoguePanel.SetActive(false);
        currentOpen = null;
    }
}