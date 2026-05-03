using UnityEngine;

public class HubButtons : MonoBehaviour
{
    public GameManager.GameState targetState;

    public void OnMouseDown()
    {
        GameManager.instance.ChangeState(targetState);
        Debug.Log("Click en botón: " + gameObject.name + " | Target: " + targetState);
    }
}
