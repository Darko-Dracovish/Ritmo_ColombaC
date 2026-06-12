using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Tutorial : MonoBehaviour
{
    public Button tutorialBoton;
    public Button tutorialSalir;
    public GameObject tutorialPanel;
    
    void Start()
    {
        
    }

   
    void Update()

    {
    }

    private void OnMouseDown()
    {

        if (tutorialBoton != null)
        {
            tutorialPanel.SetActive(true);
        }
        if (tutorialSalir != null)
        {
            tutorialPanel.SetActive(false);
        }
    }
}
