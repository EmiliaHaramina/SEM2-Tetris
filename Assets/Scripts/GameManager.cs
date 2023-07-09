using TMPro;
using UnityEngine;
using System;
using UnityEngine.PlayerLoop;
using System.Collections.Generic;
using UnityEngine.InputSystem.Controls;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;

public class GameManager : MonoBehaviour
{
    [Header("Game settings")]
    public int lvl_2 = 300;
    public int lvl_3 = 800;
    public int lvl_4 = 2000;
    public int lvl_5 = 3000;

    [Tooltip("Amount of pieces that can despawn or be dropped")]
    public int lives;

    [Header("Spawner settings")]
    [Tooltip("Spawn tetris piece every x seconds")]
    [SerializeField] float seconds;
    private float setSeconds;

    [Tooltip("Start spawning pieces after startOffset seconds")]
    [SerializeField] float startOffset;
    private float setStartOffset;

    [Tooltip("Reduce time using this variable")]
    [SerializeField] float reduceTime;

    public static Action<bool> onGameStarted;
    public static Action<bool> onGameStopped;
    public static Action onGameStoppedClearBox;

    public GameObject[] spawners;

    [Header("Scoreboard")]
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI lvlText;
    public TextMeshProUGUI livesText;

    private const int AMOUNT = 100;
    private const int LIVES = 3;

    private float currentTime;
    private bool start;

    private int score;
    private int level;

    private int nextLvl;

    private int[] levelTresholds;

    [SerializeField] List<SymptomTracker> symptomTrackers;
    [SerializeField] List<SymptomTracker> initialSymptomTrackers;
    private int wearables = 2;
    private int wornItems = 0;

    public GameObject[] objectsToHide;
    public GameObject[] objectsToShow;

    public ButtonVR stopButton;

    [SerializeField] private List<GameObject> _teleportationControllers;
    
    [Header("Providers")]
    [SerializeField] private ActionBasedContinuousMoveProvider _continuousMovementController;
    [SerializeField] private TeleportationProvider _teleportationProvider;
    [SerializeField] private ActionBasedSnapTurnProvider _snapTurnProvider;
    [SerializeField] private ActionBasedContinuousTurnProvider _continuousTurnProvider;

    public void AddWornItem()
    {
        wornItems++;
        if (wearables == wornItems)
        {
            foreach (GameObject objectToHide in objectsToHide)
                Destroy(objectToHide);

            foreach (GameObject objectToShow in objectsToShow)
                objectToShow.SetActive(true);
        }
    }

    private void OnEnable()
    {
        RowManager.onRowCleared += UpdateScore;
    }

    private void OnDisable()
    {
        RowManager.onRowCleared -= UpdateScore;
    }

    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject objectToShow in objectsToShow)
        {
            objectToShow.SetActive(false);
        }

        start = false;

        levelTresholds = new int[4];
        levelTresholds[0] = lvl_2;
        levelTresholds[1] = lvl_3;
        levelTresholds[2] = lvl_4;
        levelTresholds[3] = lvl_5;

        setSeconds = seconds;
        setStartOffset = startOffset;

        //called by button, uncomment for testing
        //StartGame();

        StartInitialSymptoms();

        if (StateManager.movement.Equals("continuous"))
        {
            foreach (var controller in _teleportationControllers)
            {
                controller.SetActive(false);
            }
            _teleportationProvider.enabled = false;
            _continuousMovementController.enabled = true;
            _snapTurnProvider.enabled = false;
            _continuousTurnProvider.enabled = true;
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (start)
        {
            currentTime += Time.deltaTime;
            float min = Mathf.FloorToInt(currentTime / 60);
            float sec = Mathf.FloorToInt(currentTime % 60);
            timeText.text = "Vrijeme: " + string.Format("{0:00}:{1:00}", min, sec);

            scoreText.text = "Bodovi: " + score;
            lvlText.text = "Razina: " + level;
            livesText.text = "Broj života: " + lives;

        }

    }

    public void StartGame()
    {
        timeText.alignment = TextAlignmentOptions.Left;
        livesText.enabled = true;
        currentTime = 0f;
        score = 0;

        level = 1;
        nextLvl = levelTresholds[level - 1];

        seconds = setSeconds;
        startOffset = setStartOffset;

        //pokreni spawnere
        spawners[0].GetComponent<GenerateTetris>().startSpawning(0, seconds);
        spawners[1].GetComponent<GenerateTetris>().startSpawning(startOffset, seconds);

        //pokreni timer
        start = true;
        onGameStarted?.Invoke(start);

        ResetSymptoms();
    }

    //poziva se kada se brise red u tetris mrezi
    public void UpdateScore()
    {
        int amount = AMOUNT;
        //lvl multiplier
        amount *= level;
        score += amount;

        //lvl 5 == max lvl 
        if (level < 5 && score >= nextLvl)
        {
            level++;
            lives++;

            if (level < 5)
            {
                nextLvl = levelTresholds[level - 1];
            }

            //stop the spawners
            spawners[0].GetComponent<GenerateTetris>().stopSpawning();
            spawners[1].GetComponent<GenerateTetris>().stopSpawning();

            //reduce time gap by half -- ovo bumo prilagodili
            seconds /= reduceTime;
            startOffset /= reduceTime;

            //start the spawners again (wait for the previous piece to fall)
            spawners[0].GetComponent<GenerateTetris>().startSpawning(startOffset, seconds);
            spawners[1].GetComponent<GenerateTetris>().startSpawning(seconds, seconds);

            ImproveSymptoms();
        }
    }

    public void StopGame()
    {
        start = false;
        onGameStopped?.Invoke(start);
        onGameStoppedClearBox?.Invoke();

        spawners[0].GetComponent<GenerateTetris>().stopSpawning();
        spawners[1].GetComponent<GenerateTetris>().stopSpawning();

        GameObject[] tetrisPieces = GameObject.FindGameObjectsWithTag("Tetris");

        foreach(GameObject tetris in tetrisPieces)
        {
            if (tetris != null)
            {
                TetrisPieceScript tetrisPiece = tetris.GetComponent<TetrisPieceScript>();
                tetrisPiece.SetExploded(true);
                tetrisPiece.Explode();
            }
        }
        livesText.enabled = false;
        lives = LIVES;

        timeText.text = "<b>IGRA JE GOTOVA<b>";
        timeText.alignment = TextAlignmentOptions.Center;
        scoreText.text = "Konačni bodovi: " + score;

        float min = Mathf.FloorToInt(currentTime / 60);
        float sec = Mathf.FloorToInt(currentTime % 60);

        lvlText.text = "Vrijeme: " + string.Format("{0:00}:{1:00}", min, sec);
    }

    public void DroppedOrDespawned()
    {
        lives--;

        if (lives <= 0)
        {
            // StopGame();
            stopButton.InvokeOnRelease();
        }

        WorsenSymptoms();
    }

    private void StartInitialSymptoms()
    {
        foreach(SymptomTracker tracker in initialSymptomTrackers)
            tracker.StartTracking();
    }

    private void ResetSymptoms()
    {
        foreach(SymptomTracker tracker in symptomTrackers)
            tracker.ResetTracker();
    }

    private void WorsenSymptoms()
    {
        foreach(SymptomTracker tracker in symptomTrackers)
            tracker.WorsenSymptoms();
    }

    private void ImproveSymptoms()
    {
        foreach(SymptomTracker tracker in symptomTrackers)
            tracker.ImproveSymptoms();
    }

    public void ReturnToClassroom()
    {
        StateManager.gameOver = true;
        SceneManager.LoadScene("Classroom");
    }
}
