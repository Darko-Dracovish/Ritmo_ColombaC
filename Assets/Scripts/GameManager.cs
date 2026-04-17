using UnityEngine;
using TMPro;
using Unity.Cinemachine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

  
    public enum GameState
    {
        Hub,
        DeckBuilding,
        Playing,
        Dialogue,
        SongSelect
    }

    public GameState currentState;

  
    [Header("Audio")]
    public AudioSource music;
    public bool musicStart;

    [Header("Rhythm")]
    public ArrowControl beatScroll;

   
    [Header("Cameras")]
    public CinemachineVirtualCameraBase mainCamera;   
    public CinemachineVirtualCameraBase cardCamera;   
    public CinemachineVirtualCameraBase hubCamera;    

   
    [Header("Score")]
    public int currentScore;
    public int objectiveScore = 30;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI objectiveText;

    
    [Header("Card Slots")]
    public Transform[] cardSlots;
    public List<GameObject> placedCards = new List<GameObject>();

   
    [Header("Deck")]
    public List<GameObject> deckPrefabs;

    [Header("Hand")]
    public int handSize = 4;
    public Transform[] handSlots;

 
    void Start()
    {
        instance = this;

        ShuffleDeck();
        DrawHand();

        ChangeState(GameState.Hub);
        UpdateScoreUI();
    }

   
    void Update()
    {
        
        DebugStateInputs();

        
        switch (currentState)
        {
            case GameState.Hub:
                HubInput();
                break;

            case GameState.DeckBuilding:
                DeckInput();
                break;

            case GameState.Playing:
                PlayingInput();
                break;

            case GameState.Dialogue:
                DialogueInput();
                break;

            case GameState.SongSelect:
                SongSelectInput();
                break;
        }
    }

    
    public void ChangeState(GameState newState)
    {
        currentState = newState;

       
        mainCamera.Priority = 0;
        cardCamera.Priority = 0;
        hubCamera.Priority = 0;

        switch (currentState)
        {
            case GameState.Hub:
                hubCamera.Priority = 10;
                break;

            case GameState.DeckBuilding:
                cardCamera.Priority = 10;
                break;

            case GameState.Playing:
                mainCamera.Priority = 10;
                break;

            case GameState.Dialogue:
                hubCamera.Priority = 10;
                break;

            case GameState.SongSelect:
                hubCamera.Priority = 10;
                break;
        }

        Debug.Log("Estado actual: " + currentState);
    }

    
    void DebugStateInputs()
    {
        if (Input.GetKeyDown(KeyCode.I))
            ChangeState(GameState.Hub);

        if (Input.GetKeyDown(KeyCode.O))
            ChangeState(GameState.DeckBuilding);

        if (Input.GetKeyDown(KeyCode.P))
            ChangeState(GameState.Playing);
    }

  
    void HubInput()
    {
        
    }

    void DeckInput()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ShuffleDeck();
            DrawHand();
            Debug.Log("Nueva mano");
        }
    }

    void PlayingInput()
    {
        if (!musicStart && Input.GetKeyDown(KeyCode.E))
        {
            StartSong();
        }
    }

    void DialogueInput()
    {
        
    }

    void SongSelectInput()
    {
        
    }

   
    void StartSong()
    {
        musicStart = true;

        if (beatScroll != null)
            beatScroll.scrollStart = true;

        if (music != null)
            music.Play();

        objectiveText.text = "Objective: " + objectiveScore;
    }

    
    public void NoteHit(int puntaje)
    {
        currentScore += puntaje;
        UpdateScoreUI();

        Debug.Log("Hit +" + puntaje);
    }

    public void NoteMiss()
    {
        Debug.Log("Miss");
    }

    void UpdateScoreUI()
    {
        if (scoreText != null)
            scoreText.text = "Score: " + currentScore;
    }

    public void ObjectiveScore()
    {
        if (currentScore >= objectiveScore)
            Debug.Log("Objective Met");
    }

    public void ObjectiveScoreFail()
    {
        if (currentScore < objectiveScore)
            Debug.Log("Objective Fail");
    }
 
    public void ShuffleDeck()
    {
        for (int i = 0; i < deckPrefabs.Count; i++)
        {
            int rand = Random.Range(i, deckPrefabs.Count);

            GameObject temp = deckPrefabs[i];
            deckPrefabs[i] = deckPrefabs[rand];
            deckPrefabs[rand] = temp;
        }
    }

    
    public void DrawHand()
    {
        for (int i = 0; i < handSlots.Length; i++)
        {
            if (i >= deckPrefabs.Count)
                return;

            Transform slot = handSlots[i];
            HandSlot slotData = slot.GetComponent<HandSlot>();

           
            if (slotData.currentCard != null)
            {
                Destroy(slotData.currentCard);
            }

            GameObject newCard =
                Instantiate(deckPrefabs[i], slot.position, Quaternion.identity, slot);

            slotData.currentCard = newCard;
        }
    }

 
    public void OpenHub()
    {
        ChangeState(GameState.Hub);
    }

    public void OpenDeckBuilder()
    {
        ChangeState(GameState.DeckBuilding);
    }

    public void OpenGameplay()
    {
        ChangeState(GameState.Playing);
    }

    public void OpenDialogue()
    {
        ChangeState(GameState.Dialogue);
    }

    public void OpenSongSelect()
    {
        ChangeState(GameState.SongSelect);
    }
}