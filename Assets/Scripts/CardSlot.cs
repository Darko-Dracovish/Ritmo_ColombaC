using UnityEngine;
using DG.Tweening;

public class CardSlot : MonoBehaviour
{
    [Header("Estado del Slot")]
    public bool isOccupied = false;

    [Header("Lugar donde aparece la copia para gameplay")]
    public Transform gameplaypoint;

    [Header("Carta oculta del sistema")]
    public GameObject cardBackCover;

    [Header("Visual de tipo (combo)")]
    public SpriteRenderer slotTypeIndicator; // El marco/indicador de color del slot
    public Color lockedColor  = Color.black;
    public Color unlockedColor = Color.white; // Ajusta al color original de tu marco

    private GameObject spawnedCard;
    private GameObject hiddenCardPrefab;   // prefab guardado hasta revelar
    private bool isFaceDown = false;

    [Header("TweenSlots")]
    public Vector3 comboRot;
    public Vector3 comboStart;
    public float rotTime;
  
    


    void Start()
    {
        RefreshVisual();
    }

    public void RefreshVisual()
    {
        if (slotTypeIndicator == null) return;
        slotTypeIndicator.color = GameManager.instance.comboUnlocked ? unlockedColor : lockedColor;
    }

    // Llamado desde GameManager al desbloquear el combo
    public static void RefreshAllSlotVisuals()
    {
        foreach (CardSlot slot in FindObjectsByType<CardSlot>(FindObjectsSortMode.None))
            slot.RefreshVisual();
    }

    public bool CanAcceptCard()
    {
        return !isOccupied;
    }

    // Llamado por el jugador al arrastrar una carta
    public void SetCard(GameObject card)
    {
        if (card == null) return;

        HandSlot hand = card.GetComponentInParent<HandSlot>();
        if (hand != null)
            hand.currentCard = null;

        CardSlot oldSlot = card.GetComponentInParent<CardSlot>();
        if (oldSlot != null && oldSlot != this)
            oldSlot.ClearSlot();

        card.transform.position = transform.position;
        card.transform.SetParent(transform);

        isOccupied = true;
        isFaceDown = false;

        SpawnGameplayCard(card);
    }

    // Llamado por el GameManager: coloca una carta oculta en el slot
    public void SetCardFaceDown(GameObject cardPrefab)
    {
        if (cardPrefab == null) return;

        // Instanciar la carta en el slot igual que SetCard, pero cubierta

         Vector3 flipPos = cardPrefab.transform.position; 
        GameObject visual = Instantiate(cardPrefab, transform.position, Quaternion.identity, transform);
  

        // Deshabilitar arrastre — el jugador no puede moverla
        DragDrop drag = visual.GetComponent<DragDrop>();
        if (drag != null) drag.enabled = false;

        hiddenCardPrefab = cardPrefab;
        isOccupied = true;
        isFaceDown = true;

        // Activar la cubierta encima de la carta
        if (cardBackCover != null)
        {
            cardBackCover.SetActive(true);
           
        }
  

        else
            Debug.LogWarning($"[{name}] cardBackCover no asignado — la carta es visible aunque debería estar oculta.");

        Debug.Log($"[{name}] Carta oculta colocada: {cardPrefab.name}");
    }

    // Llamado al pasar a Playing: revela la carta y genera el gameplay card
    public void Reveal()
    {
        if (!isFaceDown) return;

        isFaceDown = false;

        // Quitar la cubierta
        if (cardBackCover != null)
            cardBackCover.SetActive(false);

        // Buscar la carta instanciada (primer hijo que no sea gameplaypoint ni cardBackCover)
        GameObject visual = null;
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject child = transform.GetChild(i).gameObject;
            if (child != gameplaypoint?.gameObject && child != cardBackCover)
            {
                visual = child;
                break;
            }
        }

        if (visual != null)
            SpawnGameplayCard(visual);

        hiddenCardPrefab = null;
        Debug.Log($"[{name}] Carta revelada.");
    }


    int GetModifiedScore(GameObject card)
    {
        Carta carta = card.GetComponent<Carta>();
        if (carta == null) return 0;

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
            Destroy(spawnedCard);

        spawnedCard = Instantiate(card, gameplaypoint.position, Quaternion.identity);

        Carta originalCarta = card.GetComponent<Carta>();
        Carta nuevaCarta = spawnedCard.GetComponent<Carta>();

        if (originalCarta != null && nuevaCarta != null)
        {
            nuevaCarta.noteScore = originalCarta.noteScore;

            if (GameManager.instance.comboUnlocked && card.CompareTag(gameObject.tag))
            {
                nuevaCarta.noteScore *= 2;
                originalCarta.transform.DOPunchRotation(comboRot, rotTime);
                Debug.Log("Bonus aplicado: " + nuevaCarta.noteScore);
                
            }
        }
    }


    public void ClearSlot()
    {
        isOccupied = false;
        isFaceDown = false;
        hiddenCardPrefab = null;

        if (spawnedCard != null)
        {
            Destroy(spawnedCard);
            spawnedCard = null;
        }

        if (cardBackCover != null)
            cardBackCover.SetActive(false);

        // Eliminar carta visual del slot, preservando gameplaypoint y cardBackCover
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            GameObject child = transform.GetChild(i).gameObject;
            if (child != gameplaypoint.gameObject && child != cardBackCover)
                Destroy(child);
        }
    }
}
