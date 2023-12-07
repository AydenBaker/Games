using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;

public class WaveController : MonoBehaviour
{
    // Variables
    [SerializeField] private Waaves[] waves;
    
    public bool equallyDistributeAll = false;
    
    public GameObject enemyOriginal;
    public GameObject spawners;
    private int waveCount = 1;
    private bool countdownEnabled = true;
    
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI waveText;
    [SerializeField] private TextMeshProUGUI highscoreText;
    private float timeLeft = 0.0f;

    private int tempChildCounter = 0;

    public bool debugging = false;

    private int waveCounter;
    private int highscore;
    
    void Start()
    {
        Scene scene = SceneManager.GetActiveScene();
        string s = scene.name;
        highscoreText.text = ("Highest Wave: " + PlayerPrefs.GetInt(s, 0));
    }
    void Awake()
    {
        // Check We Have Waves Initialized
        if (waves.Length == 0)
        {
            print("Error: No Waves");
            Application.Quit();
        }
        
        // At beginning set the first "waveStart" to 0 so the first wave starts immediately regardless of field input
        waves[0].waveStart = 0;
        
    }
    
    void Update()
    {
        
        // Print Wave Information (Debugging)
        if(debugging)
            print("wave: "+waveCount+"       size: "+waves[waveCount-1].waveSize+"     start: " + waves[waveCount-1].waveStart+"       childs:"+this.transform.childCount);
        
        
        // Only Start the Next Wave If We've Met The Child Requirement
        if (this.transform.childCount <= waves[waveCount-1].waveStart)
        {
            // First Start A Countdown To Let The Player A New Wave Is Incoming
            if (countdownEnabled)
            {
                timerText.CrossFadeAlpha(1f, 2f, false);
                timeLeft = 5.0f;
                
                // Don't Allow Another Wave To Start Until We've Finished Spawning This One
                countdownEnabled = false;
            }
            
            // While Countdown Is Going, Update Time Left
            if (timeLeft > 0f)
            {
                timeLeft -= Time.deltaTime;
                timerText.text = ("Next Wave Starting In " + timeLeft.ToString("F2"));
            }
            
            // When Time Runs Out Spawn The Next Wave
            else
            {
                // Set Timer To 0.00 So That We Don't Have A Negative Timer
                timerText.text = "Next Wave Starting In 0.00";
                
                // Fade Out The Timer
                timerText.CrossFadeAlpha(0f, .5f, false);
                
                // Do One Of The Following Wave Size Times
                for (int i = 0; i < waves[waveCount - 1].waveSize; i++)
                {
                    // If Spawner Has Children (ie multiple spawn locations)
                    
                    if (spawners.transform.childCount > 0)
                    {
                        // If Uniform Spawning Is On, Then Spawn An Equal Number Of Enemies At Each Spawn Point
                        if ((waves[waveCount-1].equalDistribution) || equallyDistributeAll)
                        {
                            GameObject enemyClone = Instantiate(enemyOriginal,
                                spawners.transform.GetChild(tempChildCounter).transform.position,
                                spawners.transform.GetChild(tempChildCounter).transform.rotation);
                            enemyClone.transform.parent = this.transform;

                            tempChildCounter++;

                            if (tempChildCounter >= spawners.transform.childCount)
                            {
                                tempChildCounter = 0;
                            }

                        }
                        // If Uniform Spawning Is Off, Assign Each Enemy To A Random Spawn Position
                        else
                        {
                            int wildChild = Random.Range(0, spawners.transform.childCount);
                            GameObject enemyClone = Instantiate(enemyOriginal, spawners.transform.GetChild(wildChild).transform.position, spawners.transform.GetChild(wildChild).transform.rotation);
                            enemyClone.transform.parent = this.transform;
                        }
                    }
                    // The spawner has no children
                    else
                    {
                        GameObject enemyClone = Instantiate(enemyOriginal, spawners.transform.position, spawners.transform.rotation);
                        enemyClone.transform.parent = this.transform;
                    }

                }
                
                // If We Haven't Reach The Last Wave, Move To The Next Wave
                if (waveCount < waves.Length)
                    waveCount++;
                
                // Now That We've Finished, Enabled Wave Spawning Again
                waveCounter++;
                waveText.text = ("Wave: " + waveCounter);

                Scene scene = SceneManager.GetActiveScene();
                string s = scene.name;
                if (waveCounter > PlayerPrefs.GetInt(s, 0))
                {
                    
                    PlayerPrefs.SetInt(s, waveCounter);
                    highscoreText.text = "Highest Wave: " + PlayerPrefs.GetInt(s);
                }
                countdownEnabled = true;
            }
        }

    }

    public void SavePrefs()
    {
        Scene scene = SceneManager.GetActiveScene();
        string s = scene.name;
        PlayerPrefs.SetInt(s, waveCounter);
        PlayerPrefs.Save();
    }

    
}


[System.Serializable]
public class Waaves
{   
    // Data Member For Waves
    public int waveStart;
    public int waveSize;
    public bool equalDistribution;
}