using UnityEngine;

// Se añade en runtime a cada carta instanciada en el deck builder.
// Maneja el click para agregar/quitar del mazo y el feedback de color.
public class DeckCardPreview : MonoBehaviour
{
    private GameObject cardPrefab;
    private bool isDeckCard;

    public GameObject GetPrefab() => cardPrefab;

    // Colores de feedback — se aplican al SpriteRenderer principal de la carta
    private static readonly Color normalColor   = Color.white;
    private static readonly Color inDeckColor   = new Color(0.6f, 1f, 0.6f);
    private static readonly Color fullDeckColor = new Color(1f, 0.6f, 0.6f);

    public void Setup(GameObject prefab, bool inDeck)
    {
        cardPrefab = prefab;

        // Desactivar todo lo interactivo
        foreach (var drag  in GetComponentsInChildren<DragDrop>())    drag.enabled  = false;
        foreach (var arrow in GetComponentsInChildren<ArrowObject>()) arrow.enabled = false;
        foreach (var col   in GetComponentsInChildren<Collider2D>())  col.enabled   = false;

        // Añadir collider propio para detectar clicks en el deck builder
        if (GetComponent<Collider2D>() == null)
        {
            var col = gameObject.AddComponent<BoxCollider2D>();
            col.size = new Vector2(4f, 6f); // ajusta si hace falta
        }
        else
        {
            GetComponent<Collider2D>().enabled = true;
        }

        SetInDeckState(inDeck);
    }

    public void SetInDeckState(bool inDeck)
    {
        isDeckCard = inDeck;
        bool deckFull = GameManager.instance.deckPrefabs.Count >= GameManager.instance.maxDeckSize;

        Color target = inDeck ? inDeckColor : (deckFull ? fullDeckColor : normalColor);

        var sr = GetComponent<SpriteRenderer>();
        if (sr != null) sr.color = target;
    }

    private void OnMouseDown()
    {
        if (isDeckCard)
        {
            GameManager.instance.RemoveCardFromDeck(cardPrefab);
        }
        else
        {
            if (GameManager.instance.deckPrefabs.Count >= GameManager.instance.maxDeckSize)
            {
                Debug.Log("Deck lleno");
                return;
            }
            GameManager.instance.AddCardToDeck(cardPrefab);
        }

        GameManager.instance.deckUI.RefreshDeckPanel();
    }
}
