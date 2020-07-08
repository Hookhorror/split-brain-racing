using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;


public class TrackController : MonoBehaviour
{

    private float startTime;
    private float[] cpRunTimes;
    private int cpReached = 0;
    public RecordData recordTime;
    private Vector2 startPoint;
    private GameObject[] checkpoints;
    public float[] cpDefaultRecords;
    private GameState gameState = GameState.waitingPlayers;
    PlayerInputManager pim;
    GameObject[] players;
    private GameObject ship;
    public Color[] playerColors;
    private int countdownDuration = 3;
    public float goldTime;
    public float silverTime;
    public float bronzeTime;
    public AudioManager audioManager;
    bool crashed;
    string trackTag;
    private bool paused;
    public GameObject pauseMenuUI;
    private MedalTime mt;
    int medalToChase; // 0 record, 1 gold, 2 silver, 3 bronze


    void Start()
    {
        SetUpTrackStuff();
        SetUpRunStuff();
    }


    /// Track specific stuff that won't need change on resets
    private void SetUpTrackStuff()
    {
        pim = GameObject.FindGameObjectWithTag("PlayerManager").
                GetComponent<PlayerInputManager>();
        ship = GameObject.FindGameObjectWithTag("Ship");
        checkpoints = GameObject.FindGameObjectWithTag("CheckpointManager").
                GetComponent<CheckpointManager>().GetCheckpoints();
        trackTag = SceneManager.GetActiveScene().name;
        Debug.Log(trackTag);
        mt = MedalManager.Instance.GetMedalTimes(trackTag);

        startPoint = ship.transform.position;
        Debug.Log("Start Position" + startPoint);

    }


    /// Run specifig stuff that need to reset on track reset
    private void SetUpRunStuff()
    {
        SetTimesToCheckpoints();
        cpRunTimes = new float[checkpoints.Length];
        cpReached = 0;

        var shipCont = ship.GetComponent<ShipController>();
        shipCont.SetLastCheckpoint(startPoint);
        shipCont.ResetToLastCheckpoint();
        UiManager.Instance.ResetTimes();

        // Reset checkpoints used status
        crashed = false;
        paused = false;
        UiManager.Instance.HideResultScreen();
        UiManager.Instance.SetChasingText(medalToChase);
        gameState = GameState.waitingPlayers;

    }


    private void ResetCheckpoints()
    {
        for (int i = 0; i < checkpoints.Length; i++)
        {
            checkpoints[i].GetComponent<CheckpointController>().ResetStatus();
        }
        // Make the last checkpoint to use goal line sprites
        checkpoints[checkpoints.Length - 1].GetComponent<CheckpointController>().MakeGoalLine();
    }


    ///<summary>
    /// Sets checkpoint times of next medal to achieve, or personal record if
    /// gold time is already beaten.
    ///</summary>
    private void SetTimesToCheckpoints()
    {
        float[] recordCps = RecordManager.Instance.GetBestRunCheckpoints(trackTag);
        float record;
        if (recordCps == null || recordCps.Length == 0)
            record = 99999f;
        else
            record = recordCps[recordCps.Length - 1];

        // Check the checkpoint times to put on checkpoints
        float[] timesToCheckpoints;
        if (record < mt.goldTime)
        {
            timesToCheckpoints = recordCps;
            medalToChase = 0;
            Debug.Log("Checkpoints get record times");
        }
        else if (record < mt.silverTime)
        {
            timesToCheckpoints = mt.goldTimes;
            medalToChase = 1;
            Debug.Log("Checkpoints get GOLD times");
        }
        else if (record < mt.bronzeTime)
        {
            timesToCheckpoints = mt.silverTimes;
            medalToChase = 2;
            Debug.Log("Checkpoints get SILVER times");
        }
        else
        {
            timesToCheckpoints = mt.bronzeTimes;
            medalToChase = 3;
            Debug.Log("Checkpoints get BRONZE times");
        }

        for (int i = 0; i < checkpoints.Length; i++)
        {
            checkpoints[i].GetComponent<CheckpointController>().
                SetTime(timesToCheckpoints[i]);
        }
    }


    public void ShipCrashed()
    {
        PlaySound("Crash");
        crashed = true;
        DisablePlayerControls();
        CancelInvoke();
        ship.GetComponent<ShipController>().Crash();
        Invoke("RecoverFromCrash", 1);
    }


    public void ResetToStart()
    {
        Debug.Log("Reset ASKED");
        // Reset can be asked only while racing or after goal
        if (!(gameState == GameState.racing ||
               gameState == GameState.finished))
        {
            return;
        }

        if (paused)
            return;

        CancelInvoke();
        UiManager.Instance.ResetTimes();
        Debug.Log("RESETING TO START");
        DisablePlayerControls();
        SetUpRunStuff();
    }


    private void RecoverFromCrash()
    {
        crashed = false;
        if (gameState == GameState.racing)
            EnablePlayerControls();
        ship.GetComponent<ShipController>().RecoverFromCrash();
        ship.GetComponent<ShipController>().RequestReset();
    }


    public bool IsCrashed() => crashed;


    /// Run when countdown ends and racing begin
    private void StartRacing()
    {
        startTime = Time.time;
        EnablePlayerControls();
        gameState = GameState.racing;
    }


    /// Takes a split time and send it to UI
    public void SplitTime(float splitTimeRecord)
    {
        float runTimeAtCp = RunStopWatch();
        float differenceToRecord = runTimeAtCp - splitTimeRecord;
        // Debug.Log("Time: " + runTimeAtCp);
        cpRunTimes[cpReached] = runTimeAtCp;
        // Debug.Log("Difference: " + differenceToRecord);
        cpReached++;
        UiManager.Instance.SetSplitTime(differenceToRecord);
        if (cpReached == checkpoints.Length)
        {
            TrackComplete();
            Debug.Log("Rata suoritettu");
        }
    }


    private void TrackComplete()
    {
        // Check if final time is better than the record time
        gameState = GameState.finished;
        DisablePlayerControls();

        float finalTime = cpRunTimes[cpRunTimes.Length - 1];
        int placement = SaveRecord();

        // Update checkpoint times if run is a new record
        if (placement == 1)
            Debug.Log("NEw RECORD");

        int medal = 0;
        if (finalTime < mt.goldTime)
            medal = 1;
        else if (finalTime < mt.silverTime)
            medal = 2;
        else if (finalTime < mt.bronzeTime)
            medal = 3;
        UiManager.Instance.ShowResultScreen(trackTag, placement, medal);

    }


    /// <summary>
    /// Puts record to top-list if time is good enough. Returns the placement
    /// on list, or -1 if result id not make to list.
    /// </summary>
    private int SaveRecord()
    {
        // New Record system
        int placement = RecordManager.Instance.
                AddRecord(trackTag, "Testaaja", cpRunTimes);
        Debug.Log("Sijoitus uudella pistelistalla " + placement);
        return placement;
    }


    void Update()
    {
        switch (gameState)
        {
            case GameState.waitingPlayers:
                UiManager.Instance.SetPlayerCount(pim.playerCount);
                if (pim.playerCount == 2)
                {
                    Debug.Log("Kaksi pelaajaa liittynyt");
                    Debug.Log("Time scale " + Time.timeScale);
                    UiManager.Instance.HidePlayerCount();
                    SetPlayerColors();
                    // Pause bugs and leaves time scale to 0 when
                    // Time.timeScale = 1f;
                    ResetCheckpoints();

                    gameState = GameState.countdown;
                    StartCountdown();
                }
                break;

            case GameState.countdown:
                break;

            case GameState.racing:
                UiManager.Instance.SetCurrentTime(RunStopWatch());
                break;

            default:
                break;
        }
    }


    public void PlaySound(string sound)
    {
        audioManager.Play(sound);
    }


    private void SetPlayerColors()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        int i = 0;
        foreach (var p in players)
        {
            p.GetComponent<PlayerController>().SetColor(playerColors[i]);
            i++;
        }
    }


    float RunStopWatch()
    {
        return Time.time - startTime;
    }


    private void StartCountdown()
    {
        Invoke("StartRacing", countdownDuration);
        UiManager.Instance.PlayCountdown();
    }


    private void EnablePlayerControls()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        foreach (var p in players)
        {
            p.GetComponent<PlayerController>().EnableControls();
        }
    }


    private void DisablePlayerControls()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        foreach (var p in players)
        {
            p.GetComponent<PlayerController>().DisableControls();
        }
    }


    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag("Ship"))
            return;

        Debug.Log("You went over the edge");
        // Inform track about the crash
        GameObject.FindGameObjectWithTag("Track")
            .GetComponent<TrackController>().ShipCrashed();
    }


    private void Debugaa()
    {
        Debug.Log("PLAYERS: " + pim.playerCount);
    }


    public void Pause()
    {
        if (!paused)
            SetPauseOn();
        else
            SetPauseOff();
    }


    private void SetPauseOn()
    {
        if (gameState != GameState.racing)
            return;
        Debug.Log("PELI PAUSELLE");
        paused = true;
        DisablePlayerControls();
        Time.timeScale = 0f;
        pauseMenuUI.SetActive(true);
    }


    private void SetPauseOff()
    {
        Debug.Log("PELI POIS PAUSELTA");
        paused = false;
        EnablePlayerControls();
        Time.timeScale = 1f;
        pauseMenuUI.SetActive(false);
    }


    private void OnDestroy()
    {
        // Exit to track selection on pause menu leaves time scale to 0 and
        // freezes next track.
        Time.timeScale = 1f;
        Debug.Log("OnDestroy here");
    }


    enum GameState
    {
        waitingPlayers,
        countdown,
        racing,
        finished
    }

}