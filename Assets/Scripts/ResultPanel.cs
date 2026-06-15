using UnityEngine;
using TMPro;

public class ResultPanel : MonoBehaviour
{
    [Header("Textos")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI objectiveText;
    public TextMeshProUGUI resultMessage;

    [Header("Mensajes")]
    public string passMessage = "¡Nivel superado!";
    public string failMessage = "No fue suficiente...";

    void OnEnable()
    {
        if (GameManager.instance == null) return;

        int score = GameManager.instance.lastScore;
        int objective = GameManager.instance.lastObjective;
        bool passed = score >= objective;

        if (scoreText != null)     scoreText.text = score.ToString();
        if (objectiveText != null) objectiveText.text = objective.ToString();
        if (resultMessage != null) resultMessage.text = passed ? passMessage : failMessage;
    }
}
