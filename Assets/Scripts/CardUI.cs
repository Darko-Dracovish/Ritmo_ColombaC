using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardUI : MonoBehaviour
{
    [Header("Referencias (se asignan automáticamente si se crea por código)")]
    public TextMeshProUGUI cardNameText;
    public TextMeshProUGUI cardScoreText;
    public TextMeshProUGUI cardNotesText;
    public Button button;

    [Header("Feedback visual")]
    public Image cardBackground;
    public Color normalColor   = Color.white;
    public Color inDeckColor   = new Color(0.6f, 1f, 0.6f);
    public Color fullDeckColor = new Color(1f, 0.6f, 0.6f);

    private GameObject cardPrefab;
    private bool isDeckCard;

    public GameObject GetPrefab() => cardPrefab;

    public void Setup(GameObject prefab, bool inDeck)
    {
        cardPrefab = prefab;

        Carta carta = prefab.GetComponent<Carta>();

        if (cardNameText  != null) cardNameText.text  = carta != null ? carta.cardName : prefab.name;
        if (cardScoreText != null) cardScoreText.text  = carta != null ? $"Puntaje: {carta.noteScore}" : "";
        if (cardNotesText != null) cardNotesText.text  = carta != null ? $"Notas: {carta.notes.Count}" : "";

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(OnClickCard);

        SetInDeckState(inDeck);
    }

    public void SetInDeckState(bool inDeck)
    {
        isDeckCard = inDeck;
        if (cardBackground == null) return;

        bool deckFull = GameManager.instance.deckPrefabs.Count >= GameManager.instance.maxDeckSize;
        cardBackground.color = inDeck ? inDeckColor : (deckFull ? fullDeckColor : normalColor);
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
                Debug.Log("Deck lleno");
                return;
            }
            GameManager.instance.AddCardToDeck(cardPrefab);
        }

        GameManager.instance.deckUI.RefreshDeckPanel();
    }

    // Crea un GameObject de carta UI completo por código, sin necesitar prefab
    public static GameObject CreateCardUIObject(Transform parent)
    {
        // Contenedor principal
        GameObject root = new GameObject("CardUI", typeof(RectTransform), typeof(Image), typeof(Button));
        root.transform.SetParent(parent, false);

        RectTransform rt = root.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(140f, 200f);

        Image bg = root.GetComponent<Image>();
        bg.color = Color.white;

        Button btn = root.GetComponent<Button>();

        CardUI cardUI = root.AddComponent<CardUI>();
        cardUI.cardBackground = bg;
        cardUI.button = btn;

        // Nombre
        cardUI.cardNameText = CreateLabel(root.transform, "CardName", new Vector2(0, 70), new Vector2(130, 40), 16);

        // Puntaje
        cardUI.cardScoreText = CreateLabel(root.transform, "CardScore", new Vector2(0, 20), new Vector2(130, 30), 14);

        // Notas
        cardUI.cardNotesText = CreateLabel(root.transform, "CardNotes", new Vector2(0, -20), new Vector2(130, 30), 14);

        return root;
    }

    static TextMeshProUGUI CreateLabel(Transform parent, string name, Vector2 anchoredPos, Vector2 size, float fontSize)
    {
        GameObject obj = new GameObject(name, typeof(RectTransform), typeof(TextMeshProUGUI));
        obj.transform.SetParent(parent, false);

        RectTransform rt = obj.GetComponent<RectTransform>();
        rt.anchoredPosition = anchoredPos;
        rt.sizeDelta = size;

        TextMeshProUGUI tmp = obj.GetComponent<TextMeshProUGUI>();
        tmp.fontSize = fontSize;
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.color = Color.black;

        return tmp;
    }
}
