using UnityEngine;

public class HandSlot : MonoBehaviour
{
    public GameObject currentCard;

    public bool IsEmpty()
    {
        return currentCard == null;
    }
}
