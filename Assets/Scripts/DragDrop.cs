using TMPro;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragDrop : MonoBehaviour
{
    Vector2 difference = Vector2.zero;

    Vector3 startPosition;
    Transform startParent;
    private CardSlot originSlot;

    private void OnMouseDown()
    {
        difference = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - (Vector2)transform.position;

        startPosition = transform.position;
        startParent = transform.parent;
        originSlot = transform.GetComponentInParent<CardSlot>();
        Debug.Log("originSlot: " + (originSlot != null ? originSlot.name : "NULL"));

        Debug.Log("Click");
    }

    private void OnMouseDrag()
    {
        transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - difference;
    }

    private void OnMouseUp()
    {
        TryPlaceCard();
    }

    void TryPlaceCard()
    {
        Descarte discard = FindClosestDiscard();
        if (discard != null)
        {
            discard.DiscardCard(gameObject, originSlot);
            return;
        }

        CardSlot closestSlot = FindClosestSlot();

        if (closestSlot != null && closestSlot.CanAcceptCard())
        {
            closestSlot.SetCard(gameObject);
        }
        else
        {
            transform.position = startPosition;
            transform.SetParent(startParent);
        }
    }

    Descarte FindClosestDiscard()
    {
        Descarte[] zones = FindObjectsByType<Descarte>(FindObjectsSortMode.None);
        foreach (Descarte zone in zones)
        {
            float distance = Vector2.Distance(transform.position, zone.transform.position);
            Debug.Log($"Distancia a zona de descarte: {distance}");
            if (distance < 5f) // ajusta seg�n el tama�o de tu zona
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