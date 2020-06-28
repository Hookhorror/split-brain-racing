using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

[Serializable]
[DataContract]
public class RecordData
{
    [DataMember]
    private string name = "";

    [DataMember]
    private float[] checkpointTimes = new float[0];


    public RecordData(string name, float[] checkpointTimes)
    {
        this.name = name;
        this.checkpointTimes = checkpointTimes;
    }


    public string GetName()
    {
        return name;
    }


    public float[] GetCheckpointTimes()
    {
        return checkpointTimes;
    }


    public float GetFinalTime()
    {
        return checkpointTimes[checkpointTimes.Length - 1];
    }


}