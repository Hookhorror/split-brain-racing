using System;

[Serializable]
public class Record
{
    public string name;
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

}