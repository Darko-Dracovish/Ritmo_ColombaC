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
        Build,
        Playing,
        Dialogue,
        SongSelect,
        Deck
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
    public CinemachineVirtualCameraBase deckCamera;

    [Header("Score")]
    public int currentScore;
    public int objectiveScore = 30;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI objectiveText;

    [Header("Colección completa")]
    public List<GameObject> collectionCards = new List<GameObject>();

    [Header("Mazo activo")]
    public List<GameObject> deckPrefabs = new List<GameObject>();

    public int maxDeckSize = 8;

    [Header("UI")]
    public GameObject gameplayCanvas;
    public GameObject deckCanvas;
    public GameObject dialogueCanvas;

    public GameObject hubCanvas;
    public GameObject buildCanvas;
    public GameObject songCanvas;

    public DeckBuilderUI deckUI;

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

            case GameState.Build:
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
        deckCamera.Priority = 0;

        switch (currentState)
        {
            case GameState.Hub:
                hubCamera.Priority = 10;
                break;

            case GameState.Build:
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

            case GameState.Deck:
                deckCamera.Priority = 10;
                break;
        }

        Debug.Log("Estado actual: " + currentState);
        UpdateCanvases();
    }

    void UpdateCanvases()
    {
        gameplayCanvas.SetActive(false);
        deckCanvas.SetActive(false);
        dialogueCanvas.SetActive(false);

        if (hubCanvas != null) hubCanvas.SetActive(false);
        if (buildCanvas != null) buildCanvas.SetActive(false);
        if (songCanvas != null) songCanvas.SetActive(false);

        switch (currentState)
        {
            case GameState.Hub:
                if (hubCanvas != null) hubCanvas.SetActive(true);
                break;

            case GameState.Build:
                if (buildCanvas != null) buildCanvas.SetActive(true);
                break;

            case GameState.Playing:
                gameplayCanvas.SetActive(true);
                break;

            case GameState.Dialogue:
                dialogueCanvas.SetActive(true);
                break;

            case GameState.SongSelect:
                if (songCanvas != null) songCanvas.SetActive(true);
                break;

            case GameState.Deck:
                deckCanvas.SetActive(true);
                if (deckUI != null)
                    deckUI.InitializeUI();
                break;
        }
    }

    void DebugStateInputs()
    {
        if (Input.GetKeyDown(KeyCode.I))
            ChangeState(GameState.Hub);

        if (Input.GetKeyDown(KeyCode.O))
            ChangeState(GameState.Build);

        if (Input.GetKeyDown(KeyCode.P))
            ChangeState(GameState.Playing);

        if (Input.GetKeyDown(KeyCode.U))
            ChangeState(GameState.Deck);
    }

    void HubInput() { }

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
            StartSong();
    }

    void DialogueInput() { }

    void SongSelectInput() { }

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
                Destroy(slotData.currentCard);

            GameObject newCard =
                Instantiate(deckPrefabs[i], slot.position, Quaternion.identity, slot);

            slotData.currentCard = newCard;
        }
    }

    public void AddCardToDeck(GameObject cardPrefab)
    {
        if (deckPrefabs.Count >= maxDeckSize)
        {
            Debug.Log("Deck lleno");
            return;
        }

        deckPrefabs.Add(cardPrefab);

           
    }

    public void RemoveCardFromDeck(GameObject cardPrefab)
    {
        if (deckPrefabs.Contains(cardPrefab))
        {
            deckPrefabs.Remove(cardPrefab);

        }
    }

    public void OpenHub() => ChangeState(GameState.Hub);
    public void OpenDeckBuilder() => ChangeState(GameState.Deck);
    public void OpenGameplay() => ChangeState(GameState.Playing);
    public void OpenDialogue() => ChangeState(GameState.Dialogue);
    public void OpenSongSelect() => ChangeState(GameState.SongSelect);
    public void OpenBuild() => ChangeState(GameState.Build);
}