using UnityEngine;

public class CardSlot : MonoBehaviour
{
    [Header("Estado del Slot")]
    public bool isOccupied = false;

    [Header("Lugar donde aparece la copia para gameplay")]
    public Transform gameplaypoint;

    private GameObject spawnedCard;

 
    public bool CanAcceptCard()
    {
        return !isOccupied;
    }

    
    public void SetCard(GameObject card)
    {
        if (card == null) return;

        
        HandSlot hand = card.GetComponentInParent<HandSlot>();

        if (hand != null)
        {
            hand.currentCard = null;
        }

      
        CardSlot oldSlot = card.GetComponentInParent<CardSlot>();

        if (oldSlot != null && oldSlot != this)
        {
            oldSlot.ClearSlot();
        }

      
        card.transform.position = transform.position;
        card.transform.SetParent(transform);

        isOccupied = true;

        
        SpawnGameplayCard(card);
    }

  
    int GetModifiedScore(GameObject card)
    {
        Carta carta = card.GetComponent<Carta>();

        if (carta == null)
            return 0;

        int finalScore = carta.noteScore;

        
        if (card.CompareTag(gameObject.tag))
        {
            finalScore *= 2;
            Debug.Log("Bonus activado por tag: " + gameObject.tag);
        }

        return finalScore;
    }


    void SpawnGameplayCard(GameObject card)
    {
        if (gameplaypoint == null) return;

        if (spawnedCard != null)
        {
            Destroy(spawnedCard);
        }

       
        spawnedCard = Instantiate(card, gameplaypoint.position, Quaternion.identity);

        Carta originalCarta = card.GetComponent<Carta>();
        Carta nuevaCarta = spawnedCard.GetComponent<Carta>();

        if (originalCarta != null && nuevaCarta != null)
        {
            nuevaCarta.noteScore = originalCarta.noteScore;

            if (card.CompareTag(gameObject.tag))
            {
                nuevaCarta.noteScore *= 2;
                Debug.Log("Bonus aplicado: " + nuevaCarta.noteScore);
            }
        }
    }


    public void ClearSlot()
    {
        isOccupied = false;

        if (spawnedCard != null)
        {
            Destroy(spawnedCard);
            spawnedCard = null;
        }

        // Eliminar carta visual del slot, preservando gameplaypoint
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            GameObject child = transform.GetChild(i).gameObject;
            if (child != gameplaypoint.gameObject)
                Destroy(child);
        }
    }


    private void OnDrawGizmos()
    {
        if (gameplaypoint != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(gameplaypoint.position, 0.2f);
        }
    }
}