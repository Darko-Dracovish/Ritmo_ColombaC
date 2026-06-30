using UnityEngine;

public class TutorialButton : MonoBehaviour
{
    public enum TutorialType { Build, Playing, Deck }
    public TutorialType tutorial;

    void OnMouseDown()
    {
        if (UIBlocker.isBlocking) return;
        if (GameManager.instance == null || TutorialPanel.instance == null) return;

        switch (tutorial)
        {
            case TutorialType.Build:
                TutorialPanel.instance.ShowAgain("tut_build", GameManager.instance.tutorialBuild);
                break;
            case TutorialType.Playing:
                TutorialPanel.instance.ShowAgain("tut_playing", GameManager.instance.tutorialPlaying);
                break;
            case TutorialType.Deck:
                TutorialPanel.instance.ShowAgain("tut_deck", GameManager.instance.tutorialDeck);
                break;
        }
    }
}
