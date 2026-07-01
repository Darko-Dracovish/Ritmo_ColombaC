using UnityEngine;
using System.Collections.Generic;

public class RewardPanel : MonoBehaviour
{
    public static RewardPanel instance;

    [Header("Panel world-space")]
    public GameObject panel;

    [Header("Contenedor de cartas (Transform world-space)")]
    public Transform cardContainer;

    [Header("Layout")]
    public float cardSpacingX = 5f;
    public int cardsPerRow = 4;
    public float previewScale = 0.4f;

    private readonly List<GameObject> spawnedCards = new List<GameObject>();

    void Awake() => instance = this;

    public void Show(List<GameObject> newCards)
    {
        if (newCards == null || newCards.Count == 0) return;

        Clear();

        for (int i = 0; i < newCards.Count; i++)
        {
            GameObject card = Instantiate(newCards[i], cardContainer);
            card.transform.localRotation = Quaternion.identity;
            card.transform.localScale = Vector3.one * previewScale;

            int col = i % cardsPerRow;
            int row = i / cardsPerRow;
            card.transform.localPosition = new Vector3(col * cardSpacingX, -row * cardSpacingX, 0f);

            foreach (var drag in card.GetComponentsInChildren<DragDrop>())
                drag.enabled = false;
            foreach (var col2 in card.GetComponentsInChildren<Collider2D>())
                col2.enabled = false;
            foreach (var col2 in card.GetComponentsInChildren<Collider>())
                col2.enabled = false;

            spawnedCards.Add(card);
        }

        UIBlocker.Open();
        panel.SetActive(true);
    }

    public void Close()
    {
        Clear();
        UIBlocker.Close();
        panel.SetActive(false);
    }

    void Clear()
    {
        foreach (GameObject go in spawnedCards)
            if (go != null) Destroy(go);
        spawnedCards.Clear();
    }
}
