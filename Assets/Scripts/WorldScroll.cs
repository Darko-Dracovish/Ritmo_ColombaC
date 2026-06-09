using UnityEngine;

// Adjunta este script a collectionContent y/o activeDeckContent
// para permitir scroll con la rueda del mouse.
public class WorldScroll : MonoBehaviour
{
    [Header("Scroll")]
    public float scrollSpeed = 3f;
    public float minY = 0f;      // Límite superior (posición inicial)
    public float maxY = 20f;     // Límite inferior (ajusta según cuántas filas haya)

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.localPosition;
        minY = startPosition.y;
    }

    void Update()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(scroll) < 0.001f) return;

        Vector3 pos = transform.localPosition;
        pos.y += scroll * scrollSpeed;
        pos.y = Mathf.Clamp(pos.y, minY, minY + maxY);
        transform.localPosition = pos;
    }

    // Llamar desde DeckBuilderUI después de refrescar para recalcular el límite
    public void ResetScroll(int totalCards, int cardsPerRow, float cardSpacingY)
    {
        transform.localPosition = new Vector3(transform.localPosition.x, minY, transform.localPosition.z);

        int totalRows = Mathf.CeilToInt((float)totalCards / cardsPerRow);
        maxY = Mathf.Max(0f, (totalRows - 2) * cardSpacingY); // 2 filas visibles antes de hacer scroll
    }
}
