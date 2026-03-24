using UnityEngine;

public class ArrowObject : MonoBehaviour
{

    public bool canBePressed;
    public KeyCode keyToPress;
    private bool hit;
    public int points;
    void Start()
    {
        
    }

 
    void Update()
    {

        if (Input.GetKeyDown(keyToPress))
        {
            if (canBePressed) 
            {
                GameManager.instance.NoteHit(points);
                hit = true;
                gameObject.SetActive(false);
            }
        }


    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Activate")
        {
            canBePressed = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Activate"&& !hit)
        {
            GameManager.instance.NoteMiss();
    
            canBePressed = false; 
        }
    }
}
