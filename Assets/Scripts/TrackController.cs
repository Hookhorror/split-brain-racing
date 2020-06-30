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
    public GameObject[] checkpoints;
    private GameState gameState = GameState.waitingPlayers;
    PlayerInputManager pim;
    GameObject[] players;
    GameObject ship;
    public Color[] playerColors;
    private string recordFile = @"./trackrecords.json";


    void Start()
    {
        SetUpTrackStuff();
        SetUpRunStuff();
    }


    /// Track specific stuff that won't need change on resets
    private void SetUpTrackStuff()
    {
        pim = GameObject.FindGameObjectWithTag("PlayerManager").GetComponent<PlayerInputManager>();
        ship = GameObject.FindGameObjectWithTag("Ship");
        startPoint = ship.transform.position;
        GetCheckpointRecords();

        // Set record split times to checkpoints
        var rec = recordTime.GetCheckpointTimes();
        if (checkpoints.Length != rec.Length)
            Debug.LogError("Number of checkpoints and split times differ");

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

        // Reset checkpoints used status
        for (int i = 0; i < checkpoints.Length; i++)
        {
            checkpoints[i].GetComponent<CheckpointController>().ResetStatus();
        }

        // DisablePlayerControls();
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
            recordTime = new RecordData("MISSING", new float[] { 9, 9, 9 });
            return;
        }
        Debug.Log(recordJSON);
        Result r = JsonUtility.FromJson<Result>(recordJSON);

        float[] cps = JsonHelper.FromJson<float>(r.checkpoints);

        Debug.Log("CPS PITUUS " + cps.Length);

        float[] cpsFloat = new float[cps.Length];
        Debug.Log("cpsFloat PITUUS " + cpsFloat.Length);
        for (int i = 0; i < cps.Length; i++)
        {
            Debug.Log(cps[i]);
            cpsFloat[i] = (cps[i]);
            Debug.Log(cpsFloat[i]);

        }

        recordTime = new RecordData(r.date, cpsFloat);
    }


    public void ShipCrashed()
    {
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

        EnablePlayerControls();
        ship.GetComponent<ShipController>().RequestReset();
    }


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
        Debug.Log("Time: " + runTimeAtCp);
        cpRunTimes[cpReached] = runTimeAtCp;
        Debug.Log("Difference: " + differenceToRecord);
        cpReached++;
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
    }


    void Update()
    {
        switch (gameState)
        {
            case GameState.waitingPlayers:
                if (pim.playerCount == 2)
                {
                    // Invoke("SetPlayerColors", 2);
                    SetPlayerColors();
                    gameState = GameState.countdown;
                    StartCountdown();
                }
                break;

            case GameState.countdown:
                break;

            default:
                break;
        }
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
        Invoke("StartRacing", 2);
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