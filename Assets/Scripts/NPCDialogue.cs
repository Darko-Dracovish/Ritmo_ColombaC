using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class NPCDialogue : MonoBehaviour
{
    [Header("Dialogo")]
    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;
    public string dialogue;

    [Header("Desafío")]
    public List<GameObject> challengeCards;
    public List<GameObject> challengeRewards;
    public int challengeObjective;

    void OnMouseDown()
    {
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

    public void OnAccept()
    {
        dialoguePanel.SetActive(false);
        GameManager.instance.StartChallenge(challengeCards, challengeRewards, challengeObjective);
    }

    public void OnReject()
    {
        dialoguePanel.SetActive(false);
    }
}