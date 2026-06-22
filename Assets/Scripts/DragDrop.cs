using TMPro;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragDrop : MonoBehaviour
{
    [Header("Cara oculta")]
    public GameObject cardBack;  // Arrastra aquí el GameObject que representa el dorso de la carta

    [HideInInspector] public bool isFaceDown = false;
    [HideInInspector] public HandSlot myHandSlot;

    Vector2 difference = Vector2.zero;

    Vector3 startPosition;
    Transform startParent;
    private CardSlot originSlot;
    private HandSlot originHandSlot;
    private bool isDragging = false;

    public void SetFaceDown(bool faceDown)
    {
        isFaceDown = faceDown;
        if (cardBack != null)
            cardBack.SetActive(faceDown);
    }

    private void OnMouseDown()
    {
        if (UIBlocker.isBlocking) return;
        isDragging = false;

        if (isFaceDown)
        {
            SetFaceDown(false);
            return;
        }

        isDragging = true;
        difference = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - (Vector2)transform.position;

        startPosition = transform.position;
        startParent = transform.parent;
        originSlot = transform.GetComponentInParent<CardSlot>();
        originHandSlot = (myHandSlot != null && myHandSlot.currentCard == gameObject) ? myHandSlot : null;
    }

    private void OnMouseDrag()
    {
        if (!isDragging) return;
        transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - difference;
    }

    private void OnMouseUp()
    {
        if (!isDragging) return;
        isDragging = false;
        TryPlaceCard();
    }

    void TryPlaceCard()
    {
        Descarte discard = FindClosestDiscard();
        if (discard != null)
        {
            Debug.Log($"[DragDrop] Descartando carta — originSlot: {(originSlot != null ? originSlot.name : "NULL")}");
            discard.DiscardCard(gameObject, originSlot);
            return;
        }

        CardSlot closestSlot = FindClosestSlot();

        if (closestSlot != null && closestSlot.CanAcceptCard())
        {
            Debug.Log($"[DragDrop] Colocando en slot: {closestSlot.name}");
            closestSlot.SetCard(gameObject);
        }
        else
        {
            Debug.Log($"[DragDrop] Volviendo a posición original — startParent: {(startParent != null ? startParent.name : "NULL")}");
            if (originHandSlot != null)
            {
                transform.SetParent(originHandSlot.transform);
                transform.position = startPosition;
                originHandSlot.currentCard = gameObject;
            }
            else
            {
                transform.position = startPosition;
                transform.SetParent(startParent);
            }
        }
    }

    Descarte FindClosestDiscard()
    {
        Descarte[] zones = FindObjectsByType<Descarte>(FindObjectsSortMode.None);
        foreach (Descarte zone in zones)
        {
            float distance = Vector2.Distance(transform.position, zone.transform.position);
            Debug.Log($"Distancia a zona de descarte: {distance}");
            if (distance < 3f) // ajusta seg�n el tama�o de tu zona
                return zone;
        }
        return null;
    }

    CardSlot FindClosestSlot()
    {
        CardSlot[] slots = FindObjectsByType<CardSlot>(FindObjectsSortMode.None);

        float minDistance = Mathf.Infinity;
        CardSlot closest = null;

        foreach (CardSlot slot in slots)
        {
            float distance = Vector2.Distance(transform.position, slot.transform.position);

            if (distance < minDistance)
            {
                minDistance = distance;
                closest = slot;
            }
        }

        if (minDistance < 2f)
            return closest;

        return null;
    }
}