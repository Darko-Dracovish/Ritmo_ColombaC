using UnityEngine;
using TMPro;
using Unity.Cinemachine;
using System.Collections.Generic;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public enum GameState
    {
        Hub, Build, Playing, Dialogue, SongSelect, Deck
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
    public TextMeshProUGUI messageText; // Texto temporal para notificaciones (ej. "Nuevo objetivo")

    [Header("Colección desbloqueada")]
    public List<GameObject> collectionCards = new List<GameObject>();

    [Header("Colección completa")]
    public List<GameObject> totalcollectionCards = new List<GameObject>();

    [Header("Espacios Cartas")]
    public CardSlot[] cardSlots;

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

    [Header("Cartas ocultas en secuencia")]
    public int hiddenCardCount = 4;
    public List<GameObject> hiddenCardPool = new List<GameObject>(); // Arrastra aquí los prefabs que pueden salir ocultos

    [Header("Sesión activa")]
    public List<GameObject> challengeRewards = new List<GameObject>();
    public int challengeObjectiveScore;
    public GameObject lineEnd;

    public enum SessionType { Ninguna, Desafio, Nivel }
    [HideInInspector] public SessionType currentSession = SessionType.Ninguna;
    [HideInInspector] public NPCDialogue activeNPC;

    void Awake()
    {
        instance = this;
    }

    private Vector3 lineStartPosition;

    void Start()
    {
        if (beatScroll != null)
            lineStartPosition = beatScroll.transform.localPosition;

        ChangeState(GameState.Dialogue);
        UpdateScoreUI();
    }

    void Update()
    {
        DebugStateInputs();

        switch (currentState)
        {
            case GameState.Hub: HubInput(); break;
            case GameState.Build: DeckInput(); break;
            case GameState.Playing: PlayingInput(); break;
            case GameState.Dialogue: DialogueInput(); break;
            case GameState.SongSelect: SongSelectInput(); break;
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
            case GameState.Hub: hubCamera.Priority = 10; break;
            case GameState.Build: cardCamera.Priority = 10; break;
            case GameState.Playing: mainCamera.Priority = 10; break;
            case GameState.Dialogue: hubCamera.Priority = 10; break;
            case GameState.SongSelect: hubCamera.Priority = 10; break;
            case GameState.Deck: deckCamera.Priority = 10; break;
        }

        Debug.Log("Estado actual: " + currentState);
        UpdateCanvases();
    }

    void UpdateCanvases()
    {
        gameplayCanvas.SetActive(false);
        if (deckCanvas != null) deckCanvas.SetActive(false);
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
                ClearAllSlots();
                ShuffleDeck();
                DrawHand();
                PlaceHiddenCards();
                break;
            case GameState.Playing:
                gameplayCanvas.SetActive(true);
                RevealHiddenCards();
                break;
            case GameState.Dialogue:
                dialogueCanvas.SetActive(true);
                break;
            case GameState.SongSelect:
                if (songCanvas != null) songCanvas.SetActive(true);
                break;
            case GameState.Deck:
                StartCoroutine(OpenDeckWithDelay());
                break;
        }
    }

    IEnumerator OpenDeckWithDelay()
    {
        yield return new WaitForSeconds(0f);
        if (deckCanvas != null) deckCanvas.SetActive(true);
        if (deckUI != null) deckUI.InitializeUI();
    }

    void DebugStateInputs()
    {
        if (Input.GetKeyDown(KeyCode.I)) ChangeState(GameState.Hub);
        if (Input.GetKeyDown(KeyCode.O)) ChangeState(GameState.Build);
        if (Input.GetKeyDown(KeyCode.P)) ChangeState(GameState.Playing);
        if (Input.GetKeyDown(KeyCode.U)) ChangeState(GameState.Deck);
    }

    void HubInput() { }

    public void ResetGame()
    {
        currentScore = 0;
        musicStart = false;
        UpdateScoreUI();

        if (beatScroll != null)
        {
            beatScroll.scrollStart = false;
            beatScroll.transform.localPosition = lineStartPosition; //  posición original
            lineEnd.transform.localPosition = new Vector3(-51, -126, 0);
        }

        if (music != null)
            music.Stop();

        foreach (Transform slot in handSlots)
        {
            HandSlot slotData = slot.GetComponent<HandSlot>();
            if (slotData != null && slotData.currentCard != null)
            {
                Destroy(slotData.currentCard);
                slotData.currentCard = null;
            }
        }

        CardSlot[] cardSlots = FindObjectsByType<CardSlot>(FindObjectsSortMode.None);
        foreach (CardSlot slot in cardSlots)
            slot.ClearSlot();

        ShuffleDeck();
        DrawHand();
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
            StartSong();
    }

    void DialogueInput() { }
    void SongSelectInput() { }

    public void StartSong()
    {
        musicStart = true;
        if (beatScroll != null) beatScroll.scrollStart = true;
        if (music != null) music.Play();
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

    public void UpdateScoreUI()
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
        // Barajamos los índices para elegir 4 al azar
        List<int> indices = new List<int>();
        for (int i = 0; i < deckPrefabs.Count; i++) indices.Add(i);
        for (int i = indices.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            int temp = indices[i]; indices[i] = indices[j]; indices[j] = temp;
        }

        for (int i = 0; i < handSlots.Length; i++)
        {
            if (i >= deckPrefabs.Count) return;

            Transform slot = handSlots[i];
            HandSlot slotData = slot.GetComponent<HandSlot>();

            if (slotData.currentCard != null)
                Destroy(slotData.currentCard);

            GameObject newCard = Instantiate(deckPrefabs[indices[i]], slot.position, Quaternion.identity, slot);
            slotData.currentCard = newCard;

            // Colocar boca abajo
            DragDrop drag = newCard.GetComponent<DragDrop>();
            if (drag != null)
                drag.SetFaceDown(true);
        }
    }

    public void AddCardToDeck(GameObject cardPrefab)
    {
        if (deckPrefabs.Contains(cardPrefab))
        {
            Debug.Log("La carta ya está en el mazo");
            return;
        }
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
            deckPrefabs.Remove(cardPrefab);
    }

    public void UnlockCard(GameObject cardPrefab)
    {
        if (!collectionCards.Contains(cardPrefab))
        {
            collectionCards.Add(cardPrefab);
            Debug.Log("Carta desbloqueada: " + cardPrefab.name);
        }
    }

    // Inicia un desafío: secuencia prehecha, el jugador la toca para ganar cartas
    public void StartChallenge(List<GameObject> cards, List<GameObject> rewards, int objective, NPCDialogue npc)
    {
        currentSession = SessionType.Desafio;
        activeNPC = npc;
        challengeRewards = rewards;
        challengeObjectiveScore = objective;

        ClearAllSlots();
        for (int i = 0; i < cardSlots.Length; i++)
        {
            if (i >= cards.Count) break;
            GameObject card = Instantiate(cards[i]);
            cardSlots[i].SetCard(card);
        }

        if (lineEnd != null)
            lineEnd.transform.localPosition = new Vector3(-51, -36, 0);

        ChangeState(GameState.Playing);
    }

    // El NPC de nivel fija el objetivo — el jugador decide cuándo ir a Build
    public void StartLevel(int objective, NPCDialogue npc)
    {
        currentSession = SessionType.Nivel;
        activeNPC = npc;
        objectiveScore = objective;
        OpenDeckBuilder();
        Debug.Log($"Nuevo objetivo fijado: {objective} puntos");
    }

    // Llamar desde el botón de fin de canción / resultado
    public void CheckCompletion()
    {
        if (currentSession == SessionType.Desafio)
        {
            if (currentScore >= challengeObjectiveScore)
            {
                foreach (GameObject reward in challengeRewards)
                    UnlockCard(reward);

                activeNPC?.UnlockNext();
                Debug.Log("Desafío completado — cartas desbloqueadas y NPCs siguientes habilitados.");
            }
            else
            {
                Debug.Log("Desafío fallido.");
            }
        }
        else if (currentSession == SessionType.Nivel)
        {
            if (currentScore >= objectiveScore)
            {
                activeNPC?.UnlockNext();
                Debug.Log("Nivel completado — progresión desbloqueada.");
            }
            else
            {
                Debug.Log("Nivel fallido.");
            }
        }

        currentSession = SessionType.Ninguna;
        activeNPC = null;
        OpenHub();
    }

    public void ShowMessage(string msg, float duration = 3f)
    {
        if (messageText != null)
            StartCoroutine(ShowMessageCoroutine(msg, duration));
        else
            Debug.Log($"[Mensaje] {msg}");
    }

    IEnumerator ShowMessageCoroutine(string msg, float duration)
    {
        messageText.text = msg;
        messageText.gameObject.SetActive(true);
        yield return new WaitForSeconds(duration);
        messageText.gameObject.SetActive(false);
    }

    public void OpenHub() => ChangeState(GameState.Hub);
    public void OpenDeckBuilder() => ChangeState(GameState.Deck);
    public void OpenGameplay() => ChangeState(GameState.Playing);
    public void OpenDialogue() => ChangeState(GameState.Dialogue);
    public void OpenSongSelect() => ChangeState(GameState.SongSelect);
    public void OpenBuild() => ChangeState(GameState.Build);

    // Limpia todos los CardSlots al iniciar una nueva ronda de construcción
    void ClearAllSlots()
    {
        CardSlot[] allSlots = FindObjectsByType<CardSlot>(FindObjectsSortMode.None);
        foreach (CardSlot slot in allSlots)
            slot.ClearSlot();
    }

    // Coloca cartas al azar boca abajo en slots libres al entrar a Build
    void PlaceHiddenCards()
    {
        Debug.Log($"[PlaceHiddenCards] hiddenCardPool tiene {hiddenCardPool.Count} cartas.");

        if (hiddenCardPool.Count == 0)
        {
            Debug.LogWarning("[PlaceHiddenCards] hiddenCardPool está vacío. Asigna prefabs en el GameManager.");
            return;
        }

        CardSlot[] allSlots = FindObjectsByType<CardSlot>(FindObjectsSortMode.None);
        Debug.Log($"[PlaceHiddenCards] CardSlots encontrados: {allSlots.Length}");

        // Slots disponibles (vacíos)
        List<CardSlot> freeSlots = new List<CardSlot>();
        foreach (CardSlot slot in allSlots)
            if (!slot.isOccupied) freeSlots.Add(slot);

        // Barajar slots libres
        for (int i = freeSlots.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            CardSlot temp = freeSlots[i]; freeSlots[i] = freeSlots[j]; freeSlots[j] = temp;
        }

        // Barajar el pool para tomar al azar (con repetición si hay menos cartas que slots)
        List<GameObject> shuffled = new List<GameObject>(hiddenCardPool);
        for (int i = shuffled.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            GameObject temp = shuffled[i]; shuffled[i] = shuffled[j]; shuffled[j] = temp;
        }

        int count = Mathf.Min(hiddenCardCount, freeSlots.Count);
        for (int i = 0; i < count; i++)
        {
            // Si hay menos prefabs que slots, repite ciclando el pool
            GameObject prefab = shuffled[i % shuffled.Count];
            freeSlots[i].SetCardFaceDown(prefab);
        }

        Debug.Log($"Cartas ocultas colocadas: {count}");
    }

    // Revela todas las cartas ocultas al pasar a Playing
    void RevealHiddenCards()
    {
        CardSlot[] allSlots = FindObjectsByType<CardSlot>(FindObjectsSortMode.None);
        Debug.Log($"[RevealHiddenCards] Intentando revelar en {allSlots.Length} slots.");
        foreach (CardSlot slot in allSlots)
            slot.Reveal();
    }
}