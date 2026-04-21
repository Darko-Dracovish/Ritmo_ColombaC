using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CardUI : MonoBehaviour
{
    [Header("Referencias")]
    public TextMeshProUGUI cardNameText;
    public Button button;

    [Header("Feedback visual")]
    public Image cardBackground;
    public Color normalColor = Color.white;
    public Color inDeckColor = new Color(0.6f, 1f, 0.6f); // verde suave
    public Color fullDeckColor = new Color(1f, 0.6f, 0.6f); // rojo suave

    public GameObject cardPrefab;
    private bool isDeckCard;

    public GameObject GetPrefab() => cardPrefab;
    public void Setup(GameObject prefab, bool inDeck)
    {
        cardPrefab = prefab;
        cardNameText.text = prefab.name;

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(OnClickCard);

        SetInDeckState(inDeck);
    }

    public void SetInDeckState(bool inDeck)
    {
        isDeckCard = inDeck;

        if (cardBackground == null) return;

        if (inDeck)
        {
            cardBackground.color = inDeckColor;
        }
        else
        {
            // Si el mazo está lleno, mostrar que no se puede agregar
            bool deckFull = GameManager.instance.deckPrefabs.Count
                            >= GameManager.instance.maxDeckSize;
            cardBackground.color = deckFull ? fullDeckColor : normalColor;
        }
    }

    void OnClickCard()
    {
        if (isDeckCard)
        {
            GameManager.instance.RemoveCardFromDeck(cardPrefab);
        }
        else
        {
            if (GameManager.instance.deckPrefabs.Count >= GameManager.instance.maxDeckSize)
            {
                Debug.Log("Deck lleno, no se puede agregar");
                return;
            }
            GameManager.instance.AddCardToDeck(cardPrefab);
        }

        // Notificar al DeckBuilderUI para que refresque solo el panel del mazo
        GameManager.instance.deckUI.RefreshDeckPanel();
    }
}