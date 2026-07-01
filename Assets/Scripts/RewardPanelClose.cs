using UnityEngine;

public class RewardPanelClose : MonoBehaviour
{
    void OnMouseDown()
    {
        RewardPanel.instance?.Close();
    }
}
