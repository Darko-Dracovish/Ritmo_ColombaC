using UnityEngine;

public class BotonMulligan : MonoBehaviour
{
    public GameManager gameManager;

    public void OnMouseDown()
    {
        if (UIBlocker.isBlocking) return;
        gameManager.DrawHand();
        gameManager.ShuffleDeck();

    }
}
