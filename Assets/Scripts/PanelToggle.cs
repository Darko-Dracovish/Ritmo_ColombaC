using UnityEngine;

public class PanelToggle : MonoBehaviour
{
    public GameObject panel;
    public GameObject comboPanel;

    void OnMouseDown()
    {
        Debug.Log($"[PanelToggle] Click en {gameObject.name} | isOpen: {SettingsPanel.isOpen} | panel: {(panel != null ? panel.name : "NULL")}");
        if (SettingsPanel.isOpen) return;
        if (panel != null) panel.SetActive(true);
        if (comboPanel != null && GameManager.instance != null && GameManager.instance.comboUnlocked)
            comboPanel.SetActive(true);
    }

    public void ClosePanel()
    {
        if (panel != null) panel.SetActive(false);
        if (comboPanel != null) comboPanel.SetActive(false);
    }
}
