using UnityEngine;

public class DeckBuilderUI : MonoBehaviour
{
    [Header("Panels")]
    public Transform collectionPanel;
    public Transform activeDeckPanel;

    [Header("Prefab UI Carta")]
    public GameObject cardButtonPrefab;

    public void RefreshUI()
    {
        ClearPanel(collectionPanel);
        ClearPanel(activeDeckPanel);

        foreach (GameObject card in GameManager.instance.collectionCards)
        {
            CreateCard(card, collectionPanel, false);
        }

        foreach (GameObject card in GameManager.instance.deckPrefabs)
        {
            CreateCard(card, activeDeckPanel, true);
        }
    }

    void CreateCard(GameObject cardPrefab, Transform parent, bool inDeck)
    {
        GameObject obj = Instantiate(cardButtonPrefab, parent);

        CardUI cardUI = obj.GetComponent<CardUI>();
        cardUI.Setup(cardPrefab, inDeck);
    }

    void ClearPanel(Transform panel)
    {
        for (int i = panel.childCount - 1; i >= 0; i--)
        {
            Destroy(panel.GetChild(i).gameObject);
        }
    }
}