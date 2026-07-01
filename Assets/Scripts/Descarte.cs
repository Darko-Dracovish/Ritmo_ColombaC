using UnityEngine;

public class Descarte : MonoBehaviour
{
    [Header("Penalización")]
    public int objectivePenalty = 5;

    public void DiscardCard(GameObject card, CardSlot originSlot)
    {
        if (originSlot != null)
            originSlot.ClearSlot();
        else
            Object.Destroy(card);

        GameManager.instance.objectiveScore += objectivePenalty;
        GameManager.instance.UpdateScoreUI();

        Debug.Log("Carta descartada, objetivo aumentado en: " + objectivePenalty);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 3f);
    }
}
