using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    StartMenu,
    Positioning,
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
    private GameObject spawnerInstance;
    private KittenSpawner spawner;
    private TextMesh scoreText;
    private HoloToolkit.Unity.SpatialMappingManager spatialMapper;
    private float idleTimeLimit = 10.0f;
    private float idleTimerReset = 0.0f;
    private float gameOverCounter = 0.0f;
    private GameObject GameOverInstance;
    private GameObject scoreInstance;
    private bool positionClicked = false;
    private KittenGoal kittenGoal;

    public GameState gameState = GameState.StartMenu;

    public int kittensToSpawn = 25;
    public float percetageToResurrect = 0.5f;
    public GameObject GateTextObj;
    public GameObject SpawnerObj;
    public GameObject ScoreTextObj;
    public GameObject GameOverObj;
    public GameObject GoalObj;
    public GameObject SpatialMapperObj;
    public Vector3 SpawnOffset = new Vector3( 1.5f, 1.5f, 0.0f );
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
        gameState = GameState.Positioning;
        if (SpatialMapperObj != null) spatialMapper = SpatialMapperObj.GetComponent<HoloToolkit.Unity.SpatialMappingManager>();
        else Debug.LogError("No Spatial Mapper Object defined in GameManager!!");
        if (GoalObj != null) kittenGoal = GoalObj.GetComponent<KittenGoal>();
        else Debug.LogError("No Goal Object defined in GameManager!!");
        if (SpawnerObj != null)
        {
            spawnerInstance = Instantiate(SpawnerObj);
        }
        if (spawnerInstance != null)
        {
            spawner = spawnerInstance.GetComponent<KittenSpawner>();
            spawner.MaxSpawns = kittensToSpawn;
        }
        else Debug.LogError("No Kitten Spawner defined in GameManager!!");
        idleTimerReset = Time.time;
    }

    void StartLevel()
    {
        if (ScoreTextObj != null)
        {
            scoreInstance = Instantiate(ScoreTextObj);
            scoreText = scoreInstance.GetComponent<TextMesh>();
            scoreInstance.transform.position = GoalObj.transform.position + new Vector3( 0.0f, 1.0f, 0.0f );
        }
        else Debug.LogError("No Score Text Object defined in GameManager!!");

        GateTextObj.SetActive(false);
    }

    void OnClick()
    {
        if (gameState == GameState.Positioning)
        {
            positionClicked = true;
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
        kittensSpawned++;
        idleTimerReset = Time.time;
    }

    void GameOver()
    {
        gameState = GameState.GameOver;
        gameOverCounter = Time.time;
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
            GameOverInstance = Instantiate(GameOverObj);
            GameOverInstance.transform.position = GoalObj.transform.position + new Vector3(1.5f, 1.2f, 0.0f);
            TextMesh gameoverText = GameOverInstance.GetComponent<TextMesh>();
            gameoverText.text = gameOverMessage;
        }
    }

    void Update ()
    {
        if (gameState == GameState.Positioning && !positionClicked )
        {
            Vector3 pos = kittenGoal.Position();
            spawnerInstance.transform.position = pos + SpawnOffset;
            GateTextObj.transform.LookAt(Camera.main.transform);
            //GateTextObj.transform.Rotate(new Vector3(0,180,0)); //180 needed for textmesh
        }
        else if (positionClicked)
        {
            gameState = GameState.Running;
            StartLevel();
            positionClicked = false;
            spawner.positioned = true;
            idleTimerReset = Time.time;
            spatialMapper.DrawVisualMeshes = false;
        }
        debugIdleTime = Time.time - idleTimerReset;
		if ( gameState == GameState.Running && debugIdleTime > idleTimeLimit)
        {
            Debug.Log("gameover 1");
            GameOver();
        }
        if (gameState == GameState.Running && kittensResurrected + kittensDied >= kittensToSpawn)
        {
            Debug.Log("gameover 2 " + kittensResurrected + " " + kittensDied);
            GameOver();
        }
        if (gameState == GameState.GameOver && Time.time - gameOverCounter > 5.0f)
        {
            Reset();
        }

    }

    private void KillAllKittens()
    {
        GameObject[] allKittens = GameObject.FindGameObjectsWithTag("Kitten");
        foreach (GameObject kitten in allKittens)
        {
            Destroy(kitten);
        }
    }

    private void Reset()
    {
        gameState = GameState.Running;
        Destroy(GameOverInstance);
        kittensResurrected = 0;
        kittensDied = 0;
        kittensSpawned = 0;
        KillAllKittens();
        spawner.Restart();
        idleTimerReset = Time.time;
    }
}
