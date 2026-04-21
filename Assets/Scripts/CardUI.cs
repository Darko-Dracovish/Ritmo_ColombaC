using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CardUI : MonoBehaviour
{
    public TextMeshProUGUI cardNameText;
    public Button button;

    private GameObject cardPrefab;
    private bool isDeckCard;

    public void Setup(GameObject prefab, bool inDeck)
    {
        cardPrefab = prefab;
        isDeckCard = inDeck;

        cardNameText.text = prefab.name;

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(OnClickCard);
    }

    void OnClickCard()
    {
        if (isDeckCard)
        {
            GameManager.instance.RemoveCardFromDeck(cardPrefab);
        }
        else
        {
            GameManager.instance.AddCardToDeck(cardPrefab);
        }
    }
}