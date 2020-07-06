using System;
using System.Runtime.Serialization;

[Serializable]
[DataContract]
public class Record
{
    [DataMember]
    public string name;

    [DataMember]
    private float[] checkpointTimes;


    public Record(string name, float[] checkpointTimes)
    {
        this.name = name;
        this.checkpointTimes = checkpointTimes;
    }


    public float finalTime()
    {
        return checkpointTimes[checkpointTimes.Length - 1];
    }


    public float[] GetCheckpointTimes() => checkpointTimes;

}