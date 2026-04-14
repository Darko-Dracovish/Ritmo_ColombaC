using JetBrains.Annotations;
using UnityEngine;

public class CardSlot : MonoBehaviour
{
    public bool isOccupied = false;

    public Transform gameplaypoint;
    private GameObject spawnedCard;



    public bool CanAcceptCard()
    {
        return !isOccupied;
    }

    public void SetCard(GameObject card)
    {
      
        HandSlot hand = card.GetComponentInParent<HandSlot>();
        if (hand != null)
        {
            hand.currentCard = null;
        }

   
        CardSlot oldSlot = card.GetComponentInParent<CardSlot>();
        if (oldSlot != null)
        {
            oldSlot.isOccupied = false;
        }

        if (card.CompareTag("Starter"))
        {
            Carta carta = card.GetComponent<Carta>();
            if (carta != null)
            {
                carta.noteScore = carta.noteScore * 2;
            }
            Debug.Log("Si compara Starter");
        }

        if (card.CompareTag("Finisher"))
        {
            Carta carta = card.GetComponent<Carta>();
            if (carta != null)
            {
                carta.noteScore = carta.noteScore * 2;
            }
            Debug.Log("Si compara Finisher");
        }

        card.transform.position = transform.position;
        card.transform.SetParent(transform);

        isOccupied = true;
        SpawnGameplayCard(card);
    }

    void SpawnGameplayCard(GameObject card)
    {
        
        if (spawnedCard != null)
        {
            Destroy(spawnedCard);
        }

       

        spawnedCard = Instantiate(card, gameplaypoint.position, Quaternion.identity);
    }

    public void ClearSlot()
    {
        isOccupied = false;

       
        if (spawnedCard != null)
        {
            Destroy(spawnedCard);
        }
    }
    public void OnTriggerEnter2D(Collider2D card)
    {
        
    }
}