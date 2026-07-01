using UnityEngine;

public class BotonMulligan : MonoBehaviour
{
    public GameManager gameManager;
    public int objectivePenaltyPerCard = 3;

    public void OnMouseDown()
    {
        if (UIBlocker.isBlocking) return;

        int unusedCards = 0;
        foreach (Transform slot in gameManager.handSlots)
        {
            HandSlot handSlot = slot.GetComponent<HandSlot>();
            if (handSlot != null && handSlot.currentCard != null)
                unusedCards++;
        }

        if (unusedCards > 0)
        {
            gameManager.objectiveScore += objectivePenaltyPerCard * unusedCards;
            gameManager.UpdateScoreUI();
            Debug.Log($"Mulligan: {unusedCards} cartas sin usar, objetivo aumentado en {objectivePenaltyPerCard * unusedCards}");
        }

        gameManager.DrawHand();
        gameManager.ShuffleDeck();
    }
}
