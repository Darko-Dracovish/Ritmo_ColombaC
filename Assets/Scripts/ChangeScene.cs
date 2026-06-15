using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public void OnMouseDown()
    {
        SceneManager.LoadScene("EscenaJuego");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
