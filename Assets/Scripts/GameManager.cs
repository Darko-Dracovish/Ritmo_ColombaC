using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.Cinemachine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{

    public AudioSource music;
    public bool musicStart;

    public ArrowControl beatScroll;
    public static GameManager instance;
    public CinemachineVirtualCameraBase mainCamera;
    public CinemachineVirtualCameraBase cardCamera;
    

    public int currentScore;
 
    public int objectiveScore = 30;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI objectiveText;

    public Transform[] cardSlots;
    public List<GameObject> placedCards = new List<GameObject>();
    

    public List<GameObject> deckPrefabs;
    public List<GameObject> handCards;
    public int handSize = 4;
    public Transform[] handSlots;



    void Start()
    {
        instance = this;
    }


    void Update()
    {
        if (!musicStart)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
               musicStart = true;
                beatScroll.scrollStart = true;
                music.Play();
                objectiveText.text = "Objective: " + objectiveScore;
            }
            foreach (Transform slot in cardSlots)
            {
                CardSlot slotData = slot.GetComponent<CardSlot>();

                if (slotData.isOccupied && slot.childCount > 0)
                {
                    GameObject card = slot.GetChild(0).gameObject;

                    Instantiate(card, slotData.gameplayPoint.position, Quaternion.identity);
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.O)) 
        {
            mainCamera.Priority = 0;
            cardCamera.Priority = 10;

        }
        if (Input.GetKeyDown(KeyCode.P))
        {
       
            cardCamera.Priority = 0;
            mainCamera.Priority = 10;
        }
    }

 public void NoteHit(int puntaje)
    {
        Debug.Log("Hit");
        currentScore += puntaje;
        scoreText.text = "Score: " + currentScore;
    }

    public void NoteHitGood()
    {

    }

    public void NoteMiss()
    {
        Debug.Log("Miss");
    }

    
    public void ObjectiveScore()
    {

        if (currentScore >= objectiveScore)
        {
            Debug.Log("Objective Met");
          
        }

    }
    public void ObjectiveScoreFail()
    {

        if (currentScore < objectiveScore)
        {
            Debug.Log("Objective Fail");

        }

    }
    public void PlaceCardInSlot(GameObject card)
    {
      
        HandSlot handSlot = card.GetComponentInParent<HandSlot>();
        if (handSlot != null)
        {
            handSlot.currentCard = null;
        }

       
        foreach (Transform slot in cardSlots)
        {
            CardSlot slotData = slot.GetComponent<CardSlot>();

            if (!slotData.isOccupied)
            {
                card.transform.position = slot.position;
                card.transform.SetParent(slot);

                slotData.isOccupied = true;
                placedCards.Add(card);

                return;
            }
        }

        Debug.Log("No hay espacio en slots");
    }
    public void ShuffleDeck()
    {
        for (int i = 0; i < deckPrefabs.Count; i++)
        {
            int rand = Random.Range(i, deckPrefabs.Count);

            var temp = deckPrefabs[i];
            deckPrefabs[i] = deckPrefabs[rand];
            deckPrefabs[rand] = temp;
        }
    }

    public void DrawHand()
    {
        for (int i = 0; i < handSlots.Length; i++)
        {
            if (i >= deckPrefabs.Count) return;

            Transform slot = handSlots[i];
            HandSlot slotData = slot.GetComponent<HandSlot>();

            if (slotData.currentCard != null)
            {
                Destroy(slotData.currentCard);
            }

            GameObject newCard = Instantiate(deckPrefabs[i], slot.position, Quaternion.identity, slot);

            slotData.currentCard = newCard;
        }
    }
}
