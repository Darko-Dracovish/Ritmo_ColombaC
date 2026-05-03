using UnityEngine;

public class ArrowControl : MonoBehaviour
{

    public float beatTempo;
    public bool scrollStart;
   



    void Start()
    {
        beatTempo = beatTempo / 60f; 
    }

    
    void Update()
    {

        if (!scrollStart)
        {
            //  if (Input.GetKeyDown(KeyCode.E))
            // {
            // scrollStart = true;
            //  }


        }
        else
        {
            transform.position -= new Vector3(0f, beatTempo * Time.deltaTime, 0f);
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ender"))
        {
            scrollStart = false;
            GameManager.instance.ObjectiveScore();
            GameManager.instance.ObjectiveScoreFail();
            GameManager.instance.CheckChallengeCompletion();
            GameManager.instance.ResetGame();
            GameManager.instance.ChangeState(GameManager.GameState.Hub);
        }
    }


}
