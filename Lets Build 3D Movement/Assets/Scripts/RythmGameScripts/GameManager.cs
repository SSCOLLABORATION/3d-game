using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public AudioSource theMusic;

    public bool startPlaying;

    public BeatScroller theBS;

    public static GameManager instance;

    public int currentScore;
    public int scorePerNote = 100;
    public int scorePerGoodNote = 125;
    public int scorePerPerfectNote = 150;

    public Text scoreText;
    public Text multiText;

    public int currentMultiplier;
    public int multiplierTracker;
    public int[] multiplierThresholds;




    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       

    instance = this;
    scoreText.text = "Score: 0" ;
    currentMultiplier = 1;


    }

    // Update is called once per frame
    void Update()
    {
        if(!startPlaying)
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                startPlaying = true;
                theBS.hasStarted = true;
                theMusic.Play();
            }
        }
        else if(!theMusic.isPlaying)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex-1 );
        }
    }

    public void NoteHit()
    {
        Debug.Log("Hit On Time");
        if(currentMultiplier - 1 < multiplierThresholds.Length)
        { 
        multiplierTracker++;

        if(multiplierThresholds[currentMultiplier - 1] <= multiplierTracker)
        {
            multiplierTracker = 0;
            currentMultiplier++;
        }
        }

        multiText.text = "Multiplier: x" + currentMultiplier;


        //currentScore += scorePerNote * currentMultiplier;
        scoreText.text = "Score: " + currentScore;
    }


    public void normalHit()
    {
        currentScore += scorePerNote * currentMultiplier;
         NoteHit();
    }

    public void goodHit()
    {
        currentScore += scorePerGoodNote * currentMultiplier;
        NoteHit();
        ;
    }
    
    public void perfectHit()
    {
        currentScore += scorePerPerfectNote * currentMultiplier;
        NoteHit();
        
    }
    public void NoteMissed()
    {
        Debug.Log("Missed Note");
        currentMultiplier = 1;
        multiplierTracker = 0;
        multiText.text = "Multiplier: x" + currentMultiplier;
    }
}
