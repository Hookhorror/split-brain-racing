using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using UnityEngine;

public class TrackController : MonoBehaviour
{

    private float startTime;
    private float[] cpRunTimes;
    private int cpReached = 0;
    public RecordData recordTime;
    public GameObject[] checkpoints;


    void Start()
    {
        LoadRecords();
        var rec = recordTime.GetCheckpointTimes();
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
        if (!File.Exists(fileName))
        {
            Debug.LogWarning("Record file does not exist");
            return;
        }

        FileStream fs = File.OpenRead(fileName);
        var ser = new DataContractJsonSerializer(typeof(RecordData));

        recordTime = (RecordData)ser.ReadObject(fs);
    }


    private void ResetToStart()
    {
        startTime = Time.time;
        cpRunTimes = new float[checkpoints.Length];
        for (int i = 0; i < checkpoints.Length; i++)
        {
            checkpoints[i].GetComponent<CheckpointController>().ResetStatus();
        }
    }


    /// Takes a split time and send it to UI
    public void SplitTime(float splitTimeRecord)
    {
        float splitTime = TakeTime() - splitTimeRecord;
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
        float finalTime = cpRunTimes[cpRunTimes.Length - 1];
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

    }


    float TakeTime()
    {
        float time = Time.time - startTime;
        return time;
    }

}