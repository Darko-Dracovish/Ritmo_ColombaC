using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.Cinemachine;

public class GameManager : MonoBehaviour
{

    public AudioSource music;
    public bool musicStart;

    public ArrowControl beatScroll;
    public static GameManager instance;
    public CinemachineVirtualCameraBase mainCamera;
    public CinemachineVirtualCameraBase cardCamera;
    

    public int currentScore;
 
    public int objectiveScore = 30;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI objectiveText;



    void Start()
    {
        instance = this;
    }


    void Update()
    {
        if (!musicStart)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
               musicStart = true;
                beatScroll.scrollStart = true;
                music.Play();
                objectiveText.text = "Objective: " + objectiveScore;
            }
        }

        if (Input.GetKeyDown(KeyCode.O)) 
        {
            mainCamera.Priority = 0;
            cardCamera.Priority = 10;

        }
        if (Input.GetKeyDown(KeyCode.P))
        {
       
            cardCamera.Priority = 0;
            mainCamera.Priority = 10;
        }
    }

 public void NoteHit(int puntaje)
    {
        Debug.Log("Hit");
        currentScore += puntaje;
        scoreText.text = "Score: " + currentScore;
    }

    public void NoteHitGood()
    {

    }

    public void NoteMiss()
    {
        Debug.Log("Miss");
    }

    
    public void ObjectiveScore()
    {

        if (currentScore >= objectiveScore)
        {
            Debug.Log("Objective Met");
          
        }

    }
    public void ObjectiveScoreFail()
    {

        if (currentScore < objectiveScore)
        {
            Debug.Log("Objective Fail");

        }

    }
}
