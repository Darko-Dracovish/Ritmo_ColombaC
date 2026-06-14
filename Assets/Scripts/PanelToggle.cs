using UnityEngine;

public class PanelToggle : MonoBehaviour
{
    public GameObject panel;
   

    void OnMouseDown()
    {
        if (panel != null) panel.SetActive(true);
       
    }

    public void ClosePanel()
    {
        if (panel != null) panel.SetActive(false);
        
    }
}
