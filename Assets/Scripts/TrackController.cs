using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using UnityEngine;
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
    private string recordFile = @"./trackrecords.json";
    private int countdownDuration = 3;
    public float goldTime;
    public float silverTime;
    public float bronzeTime;
    public AudioManager audioManager;
    bool crashed;
    // RecordManager recordManager;


    void Start()
    {
        SetUpTrackStuff();
        SetUpRunStuff();
    }


    /// Track specific stuff that won't need change on resets
    private void SetUpTrackStuff()
    {
        // recordManager = GameObject.FindGameObjectWithTag("RecordManager")
        //         .GetComponent<RecordManager>();
        pim = GameObject.FindGameObjectWithTag("PlayerManager").
                GetComponent<PlayerInputManager>();
        ship = GameObject.FindGameObjectWithTag("Ship");
        checkpoints = GameObject.FindGameObjectWithTag("CheckpointManager").
                GetComponent<CheckpointManager>().GetCheckpoints();

        startPoint = ship.transform.position;
        Debug.Log("Start Position" + startPoint);
        GetCheckpointRecords();

        // Set record split times to checkpoints
        var rec = recordTime.GetCheckpointTimes();
        // Debug.Log(rec.Length);
        // Debug.Log(checkpoints.Length);
        if (checkpoints.Length != rec.Length)
            Debug.LogError("Number of checkpoints and split times differ");

        // Debug.Log(checkpoints.Length);

        for (int i = 0; i < checkpoints.Length; i++)
        {
            checkpoints[i].GetComponent<CheckpointController>().
                SetTime(rec[i]);
        }
    }


    /// Run specifig stuff that need to reset on track reset
    private void SetUpRunStuff()
    {
        cpRunTimes = new float[checkpoints.Length];
        cpReached = 0;

        var shipCont = ship.GetComponent<ShipController>();
        shipCont.SetLastCheckpoint(startPoint);
        shipCont.ResetToLastCheckpoint();
        UiManager.Instance.ResetTimes();

        // Reset checkpoints used status
        for (int i = 0; i < checkpoints.Length; i++)
        {
            checkpoints[i].GetComponent<CheckpointController>().ResetStatus();
        }

        crashed = false;
        gameState = GameState.waitingPlayers;

    }


    private string ReadFile()
    {
        if (!File.Exists(recordFile))
        {
            Debug.LogWarning("Record file does not exist");
            return null;
        }

        StreamReader reader = new StreamReader(recordFile);
        string data = reader.ReadToEnd();
        reader.Close();
        return data;
    }


    private void GetCheckpointRecords()
    {
        string recordJSON = ReadFile();
        if (recordJSON == null)
        {
            recordTime = new RecordData("MISSING", cpDefaultRecords);
            return;
        }
        Debug.Log(recordJSON);
        Result r = JsonUtility.FromJson<Result>(recordJSON);

        float[] cps = JsonHelper.FromJson<float>(r.checkpoints);

        // Debug.Log("CPS PITUUS " + cps.Length);
        float[] cpsFloat = new float[cps.Length];
        Debug.Log("cpsFloat PITUUS " + cpsFloat.Length);
        for (int i = 0; i < cps.Length; i++)
        {
            // Debug.Log(cps[i]);
            cpsFloat[i] = (cps[i]);
            Debug.Log(cpsFloat[i]);
        }

        recordTime = new RecordData(r.date, cpsFloat);
    }


    public void ShipCrashed()
    {
        PlaySound("Crash");
        crashed = true;
        DisablePlayerControls();
        CancelInvoke();
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

        CancelInvoke();
        UiManager.Instance.ResetTimes();
        Debug.Log("RESETING TO START");
        DisablePlayerControls();
        SetUpRunStuff();
    }


    private void SaveRecord()
    {
        Result r = new Result();
        r.date = "UUSI";
        r.checkpoints = JsonHelper.ToJson(cpRunTimes, true);
        string resultJson = JsonUtility.ToJson(r, true);

        StreamWriter writer = new StreamWriter(recordFile);
        writer.Write(resultJson);
        writer.Close();

    }


    private void RecoverFromCrash()
    {
        crashed = false;
        if (gameState == GameState.racing)
            EnablePlayerControls();
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
        // TODO: Send to UI
    }


    private void TrackComplete()
    {
        // Check if final time is better than the record time
        gameState = GameState.finished;
        DisablePlayerControls();

        float finalTime = cpRunTimes[cpRunTimes.Length - 1];
        if (finalTime < recordTime.GetFinalTime())
        {
            // New record time
            Debug.Log("NEW RECORD " + cpRunTimes[cpRunTimes.Length - 1]);
            SaveRecord();
            recordTime.SetCheckpointTimes(cpRunTimes);
        }
        SaveOnNewRecordSystem();


    }


    private int SaveOnNewRecordSystem()
    {
        // New Record system
        // int place = recordManager.AddRecord("track1", "Testaaja", cpRunTimes);
        int place = RecordManager.Instance.AddRecord("track1", "Testaaja", cpRunTimes);
        Debug.Log("Sijoitus uudella pistelistalla " + place);
        return place;
    }


    void Update()
    {
        switch (gameState)
        {
            case GameState.waitingPlayers:
                UiManager.Instance.SetPlayerCount(pim.playerCount);
                if (pim.playerCount == 2)
                {
                    UiManager.Instance.HidePlayerCount();
                    SetPlayerColors();
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
        Debug.Log("Coundtown began");
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
        var pim = GameObject.FindGameObjectWithTag("PlayerManager").GetComponent<PlayerInputManager>();
        Debug.Log("PLAYERS: " + pim.playerCount);
    }


    enum GameState
    {
        waitingPlayers,
        countdown,
        racing,
        finished
    }

}