using UnityEngine;
using System.Collections.Generic;

public class DeckBuilderUI : MonoBehaviour
{
    [Header("Carta Containers")]
    public Transform collectionContent;
    public Transform activeDeckContent;

    [Header("Prefab UI Carta")]
    public GameObject cardButtonPrefab;

    private List<CardUI> collectionPool = new();
    private List<CardUI> deckPool = new();

    public void InitializeUI()
    {
        RefreshPanel(collectionContent, GameManager.instance.collectionCards, collectionPool, false);
        RefreshPanel(activeDeckContent, GameManager.instance.deckPrefabs, deckPool, true);
    }

    public void RefreshDeckPanel()
    {
        RefreshPanel(activeDeckContent, GameManager.instance.deckPrefabs, deckPool, true);

        // Actualiza estado visual de la colección sin recrearla
        foreach (CardUI ui in collectionPool)
        {
            if (ui.gameObject.activeSelf)
                ui.SetInDeckState(GameManager.instance.deckPrefabs.Contains(ui.GetPrefab()));
        }
    }

    void RefreshPanel(Transform parent, List<GameObject> cards, List<CardUI> pool, bool inDeck)
    {
        // Desactiva todos primero
        foreach (CardUI ui in pool)
            ui.gameObject.SetActive(false);

        for (int i = 0; i < cards.Count; i++)
        {
            CardUI ui;

            if (i < pool.Count)
            {
                // Reusar existente
                ui = pool[i];
                ui.gameObject.SetActive(true);
            }
            else
            {
                // Crear nuevo solo si no alcanza el pool
                GameObject obj = Instantiate(cardButtonPrefab, parent);
                obj.transform.localScale = Vector3.one;
                ui = obj.GetComponent<CardUI>();
                pool.Add(ui);
            }

            ui.Setup(cards[i], inDeck);
        }
    }
}