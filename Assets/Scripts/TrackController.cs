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
        LoadRecords();

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


    private void LoadRecords()
    {
        string fileName = @"./trackrecords.json";
        if (!File.Exists(fileName))
        {
            Debug.LogWarning("Record file does not exist");
            return;
        }

        FileStream fs = File.OpenRead(fileName);
        var ser = new DataContractJsonSerializer(typeof(RecordData));

        recordTime = (RecordData)ser.ReadObject(fs);
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
            return;
        CancelInvoke();
        Debug.Log("RESETING TO START");
        DisablePlayerControls();
        SetUpRunStuff();
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
        float splitTime = TakeTime() - splitTimeRecord;
        // Debug.Log("CHECKPOINTIT REACHED " + cpReached);
        cpRunTimes[cpReached] = splitTime;
        Debug.Log("Split time: " + splitTime);
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
        float finalTime = cpRunTimes[cpRunTimes.Length - 1];
        DisablePlayerControls();
        if (finalTime < recordTime.GetFinalTime())
        {
            // New record time
            Debug.Log("NEW RECORD " + cpRunTimes[cpRunTimes.Length - 1]);
            RecordData rd = new RecordData("RoopeAnkka", cpRunTimes);
            string json = JsonUtility.ToJson(rd);
            Debug.Log(json);
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
        Debug.Log(playerColors.Length);
        Debug.Log(players.Length);
        int i = 0;
        foreach (var p in players)
        {
            p.GetComponent<PlayerController>().SetColor(playerColors[i]);
            i++;
        }
    }


    float TakeTime()
    {
        float time = Time.time - startTime;
        return time;
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