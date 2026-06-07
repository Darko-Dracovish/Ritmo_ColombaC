using UnityEngine;

public class BotonComenzar : MonoBehaviour
{ 
 public GameManager gameManager;

    public void OnMouseDown()
    {
    gameManager.StartSong();

    }
}