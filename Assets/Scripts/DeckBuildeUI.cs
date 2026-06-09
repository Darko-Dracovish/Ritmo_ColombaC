using UnityEngine;
using System.Collections.Generic;

public class DeckBuilderUI : MonoBehaviour
{
    [Header("Contenedores (transforms de mundo)")]
    public Transform collectionContent;   // Donde se muestran las cartas de la colección
    public Transform activeDeckContent;   // Donde se muestran las cartas del mazo activo

    [Header("Layout")]
    public float cardSpacingX = 5f;       // Separación horizontal entre cartas
    public float cardSpacingY = 7f;       // Separación vertical entre filas
    public int cardsPerRow = 4;           // Cuántas cartas por fila
    public float previewScale = 0.4f;     // Tamaño de las cartas

    private List<DeckCardPreview> collectionPool = new();
    private List<DeckCardPreview> deckPool = new();

    public void InitializeUI()
    {
        RefreshPanel(collectionContent, GameManager.instance.collectionCards, collectionPool, false);
        RefreshPanel(activeDeckContent, GameManager.instance.deckPrefabs, deckPool, true);
    }

    public void RefreshDeckPanel()
    {
        // Refrescar ambos paneles para reflejar el estado actual del mazo
        RefreshPanel(activeDeckContent, GameManager.instance.deckPrefabs, deckPool, true);
        RefreshPanel(collectionContent, GameManager.instance.collectionCards, collectionPool, false);
    }

    void RefreshPanel(Transform parent, List<GameObject> cards, List<DeckCardPreview> pool, bool inDeck)
    {
        // Destruir todo lo anterior para garantizar que refleje el estado actual
        foreach (DeckCardPreview p in pool)
            if (p != null) Destroy(p.gameObject);
        pool.Clear();

        for (int i = 0; i < cards.Count; i++)
        {
            GameObject obj = Instantiate(cards[i], parent);
            obj.transform.localRotation = Quaternion.identity;
            obj.transform.localScale = Vector3.one * previewScale;

            DeckCardPreview preview = obj.GetComponent<DeckCardPreview>();
            if (preview == null)
                preview = obj.AddComponent<DeckCardPreview>();

            int col = i % cardsPerRow;
            int row = i / cardsPerRow;
            obj.transform.localPosition = new Vector3(col * cardSpacingX, -row * cardSpacingY, 0f);

            preview.Setup(cards[i], inDeck);
            pool.Add(preview);
        }

        // Recalcular límite de scroll si el contenedor tiene WorldScroll
        WorldScroll scroller = parent.GetComponent<WorldScroll>();
        if (scroller != null)
            scroller.ResetScroll(cards.Count, cardsPerRow, cardSpacingY);
    }
}
