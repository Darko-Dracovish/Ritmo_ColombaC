using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.Cinemachine;
using System.Collections.Generic;

public class HandSlot : MonoBehaviour
{
    public GameObject currentCard;

    public bool IsEmpty()
    {
        return currentCard == null;
    }
}
