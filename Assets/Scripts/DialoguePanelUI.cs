using UnityEngine;

// Adjunta este script al dialoguePanel compartido.
// Asigna los botones Aceptar y Rechazar a este componente en lugar de a un NPC específico.
public class DialoguePanelUI : MonoBehaviour
{
    public void OnAccept()
    {
        if (NPCDialogue.currentOpen != null)
            NPCDialogue.currentOpen.OnAccept();
    }

    public void OnReject()
    {
        if (NPCDialogue.currentOpen != null)
            NPCDialogue.currentOpen.OnReject();
    }
}
