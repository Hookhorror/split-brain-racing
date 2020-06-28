using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;


[DataContract]
public class RecordData
{
    [DataMember]
    private string name = "";

    [DataMember]
    private float[] checkpointTimes = new float[0];

    public string GetName()
    {
        return name;
    }

    public float[] GetCheckpointTimes()
    {
        return checkpointTimes;
    }
}