using UnityEngine;

public class BotonMulligan : MonoBehaviour
{
    public GameManager gameManager;

    public void OnMouseDown()
    {
        gameManager.DrawHand();
        gameManager.ShuffleDeck();

    }
}
