using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class Carta : MonoBehaviour
{
    [Header("Info")]
    public string cardName;
    public int noteScore = 10;
    public List<ArrowObject> notes;

    void Start()
    {
        foreach (ArrowObject note in notes)
        {
            note.points = noteScore;
        }
    }

    void OnMouseDown()
    {
        GameManager.instance.PlaceCardInSlot(gameObject);
    }


}