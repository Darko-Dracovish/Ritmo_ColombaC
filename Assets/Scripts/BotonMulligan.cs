using UnityEngine;

public class BotonMulligan : MonoBehaviour
{
    public GameManager gameManager;

    public void OnMouseDown()
    {
        if (SettingsPanel.isOpen) return;
        gameManager.DrawHand();
        gameManager.ShuffleDeck();

    }
}
