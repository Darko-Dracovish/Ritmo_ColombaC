using UnityEngine;

public class ButtonControl : MonoBehaviour
{
    private SpriteRenderer spriteR;
    public Sprite button;
    public Sprite buttonPressed;
    public KeyCode keyToPress;
    void Start()
    {
        spriteR = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
       // if (Input.GetKeyDown(keyToPress))
       // {
      //      spriteR.sprite = buttonPressed;
       // }

        if (Input.GetKeyDown(keyToPress))
        {
            spriteR.sprite = buttonPressed;
        }


    }
}
