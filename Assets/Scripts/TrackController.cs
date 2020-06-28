using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using UnityEngine;

public class TrackController : MonoBehaviour
{

    // public checkpoints;
    private float startTime;
    // private float[] checkpointRecordTimes = new float[] { 3, 4, 6 };
    private float[] checkpointRunTimes;
    private int checkpointsReached = 0;
    private bool trackComplete = false;

    public RecordData recordData;

    public GameObject[] checkpoints;

    void Start()
    {
        LoadRecords();
        var rec = recordData.GetCheckpointTimes();
        if (checkpoints.Length != rec.Length)
            Debug.LogError("Number of checkpoints and split times differ");
        Debug.Log(checkpoints.Length);
        for (int i = 0; i < checkpoints.Length; i++)
        {
            checkpoints[i].GetComponent<CheckpointController>().
                SetTime(rec[i]);
        }
        ResetToStart();
    }


    private void LoadRecords()
    {
        string fileName = @"./trackrecords.json";
        Debug.Log(File.Exists(fileName) ? "File exists." : "File does not exist.");

        FileStream fs = File.OpenRead("trackrecords.json");
        var ser = new DataContractJsonSerializer(typeof(RecordData));

        recordData = (RecordData)ser.ReadObject(fs);

        Debug.Log(recordData.GetName());
        float[] recordTi = recordData.GetCheckpointTimes();
        Debug.Log(recordTi[1]);
    }


    private void ResetToStart()
    {
        startTime = Time.time;
        checkpointRunTimes = new float[checkpoints.Length];
        trackComplete = false;
        for (int i = 0; i < checkpoints.Length; i++)
        {
            checkpoints[i].GetComponent<CheckpointController>().ResetStatus();
        }
    }


    /// Takes a split time and send it to UI
    public void SplitTime(float splitTime)
    {
        float splitT = TakeTime() - splitTime;
        checkpointRunTimes[checkpointsReached] = splitT;
        Debug.Log("Split time: " + splitT);
        checkpointsReached++;
        if (checkpointsReached == checkpoints.Length)
        {
            trackComplete = true;
            Debug.Log("Rata suoritettu");
        }
        // TODO: Send to UI
    }


    void Update()
    {
        float time = TakeTime();
    }


    float TakeTime()
    {
        float time = Time.time - startTime;
        return time;
    }

}