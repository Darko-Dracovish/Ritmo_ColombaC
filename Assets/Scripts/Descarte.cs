using UnityEngine;

public class Descarte : MonoBehaviour
{
    [Header("Penalización")]
    public int scorePenalty = 5;

    public void DiscardCard(GameObject card, CardSlot originSlot)
    {
        if (originSlot != null)
            originSlot.ClearSlot();
        else
            Object.Destroy(card);

        GameManager.instance.currentScore -= scorePenalty;
        GameManager.instance.UpdateScoreUI();

        Debug.Log("Carta descartada, penalización: " + scorePenalty);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 3f);
    }
}
