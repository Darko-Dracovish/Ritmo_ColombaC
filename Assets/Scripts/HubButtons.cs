using UnityEngine;

public class HubButtons : MonoBehaviour
{
    public GameManager.GameState targetState;

    public void OnMouseDown()
    {
        if (UIBlocker.isBlocking) return;
        GameManager.instance.ChangeState(targetState);
        Debug.Log("Click en bot�n: " + gameObject.name + " | Target: " + targetState);
    }
}
