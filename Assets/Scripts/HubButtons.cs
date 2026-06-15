using UnityEngine;

public class HubButtons : MonoBehaviour
{
    public GameManager.GameState targetState;

    public void OnMouseDown()
    {
        if (SettingsPanel.isOpen) return;
        GameManager.instance.ChangeState(targetState);
        Debug.Log("Click en bot�n: " + gameObject.name + " | Target: " + targetState);
    }
}
