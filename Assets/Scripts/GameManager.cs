using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    StartMenu,
    Running,
    Running_Spawning,
    Running_SpawnOver,
    GameOver
}

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null; //singleton

    private int kittensResurrected = 0;
    private int kittensDied = 0;
    private int kittensSpawned = 0;
    private KittenSpawner spawner;
    private TextMesh scoreText;
    private float idleTimeLimit = 30.0f;
    private float idleTimerReset = 0.0f;
    public GameState gameState = GameState.StartMenu;

    public int kittensToSpawn = 25;
    public float percetageToResurrect = 0.5f;
    public GameObject SpawnerObj;
    public GameObject ScoreTextObj;
    public GameObject GameOverObj;

    public float debugIdleTime = 0.0f;

    private void Awake()
    {
        //Check if instance already exists
        if (instance == null)
            //if not, set instance to this
            instance = this;

        //If instance already exists and it's not this:
        else if (instance != this)
            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);
    }

    void Start ()
    {
        gameState = GameState.StartMenu;
        //....
        gameState = GameState.Running;
        StartLevel();
    }

    void StartLevel()
    {
        if (SpawnerObj != null)
        {
            spawner = SpawnerObj.GetComponent<KittenSpawner>();
            spawner.MaxSpawns = kittensToSpawn;
        }
        else
        {
            Debug.LogError("No Kitten Spawner defined in GameManager!!");
        }
        if (ScoreTextObj != null)
        {
            scoreText = ScoreTextObj.GetComponent<TextMesh>();
        }
        else
        {
            Debug.LogError("No Score Text Object defined in GameManager!!");
        }
    }

    public void AddResurrection()
    {
        kittensResurrected++;
        scoreText.text = "Kittens Saved:\n" + kittensResurrected + " out of " + kittensToSpawn;
        idleTimerReset = Time.time;
    }

    public void AddDeath()
    {
        kittensDied++;
        idleTimerReset = Time.time;
    }

    public void AddSpawned()
    {
        kittensDied++;
        idleTimerReset = Time.time;
    }

    void GameOver()
    {
        gameState = GameState.GameOver;

        float percetageResurrected = (float)kittensResurrected / (float)kittensToSpawn;
        string gameOverMessage = "" ;
        if (percetageResurrected >= percetageToResurrect)
        {
            gameOverMessage = "Congratulations!\nYou saved " + kittensResurrected + " out of " + kittensToSpawn;
        }
        else
        {
            gameOverMessage = "Not enough Kittens Resurrected!\nYou only saved " + kittensResurrected + " out of " + kittensToSpawn;
        }
        Debug.Log(gameOverMessage);
        if (GameOverObj != null)
        {
            GameObject GOGO = Instantiate(GameOverObj);
            TextMesh gameoverText = GOGO.GetComponent<TextMesh>();
            gameoverText.text = gameOverMessage;
        }
    }

    void Update ()
    {
        debugIdleTime = Time.time - idleTimerReset;
		if ( gameState == GameState.Running && Time.time - idleTimerReset > idleTimeLimit)
        {
            GameOver();
        }
        if( gameState == GameState.Running && kittensResurrected + kittensDied >= kittensToSpawn )
        {
            GameOver();
        }
	}
}
