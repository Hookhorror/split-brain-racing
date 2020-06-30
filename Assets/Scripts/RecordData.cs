using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

public class RecordData
{
    private string date = "";

    private float[] checkpointTimes = new float[0];

    public RecordData(string name, float[] checkpointTimes)
    {
        this.date = name;
        this.checkpointTimes = checkpointTimes;
    }


    public string GetDate()
    {
        return date;
    }


    public float[] GetCheckpointTimes()
    {
        return checkpointTimes;
    }


    public float GetFinalTime()
    {
        return checkpointTimes[checkpointTimes.Length - 1];
    }


    public void SetCheckpointTimes(float[] times)
    {
        this.checkpointTimes = times;
    }


}