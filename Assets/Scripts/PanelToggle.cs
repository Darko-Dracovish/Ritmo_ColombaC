using UnityEngine;

public class PanelToggle : MonoBehaviour
{
    public GameObject panel;
    public GameObject comboPanel;

    void OnMouseDown()
    {
        if (UIBlocker.isBlocking) return;
        if (panel != null) { panel.SetActive(true); UIBlocker.Open(); }
        if (comboPanel != null && GameManager.instance != null && GameManager.instance.comboUnlocked)
            comboPanel.SetActive(true);
    }

    public void ClosePanel()
    {
        if (panel != null && panel.activeSelf) { panel.SetActive(false); UIBlocker.Close(); }
        if (comboPanel != null) comboPanel.SetActive(false);
    }
}
