using System;
using System.Runtime.Serialization;
using UnityEngine;

[Serializable]
[DataContract]
public class TrackRecord
{
    [DataMember]
    public string trackTag;

    [DataMember]
    Record[] TopResults;


    public TrackRecord(string trackTag)
    {
        this.trackTag = trackTag;
        this.TopResults = new Record[0];
    }


    /// Adds a record to record list if it belongs there. Returns postion
    /// of new record or -1.
    public int AddRecord(string name, float[] checkpoints, int maxEntries)
    {
        Debug.Log("TopResults.Length " + TopResults.Length);
        Debug.Log("TrackRecord.AddRecord alussa");
        Debug.Log("maxEntries " + maxEntries);
        Record r = new Record(name, checkpoints);

        Debug.Log("Record " + name + " " + checkpoints.Length);

        // If there is no previous records, just add this to first place
        if (TopResults == null || TopResults.Length < 1)
        {
            TopResults = new Record[1] { r };
            return 1;
        }

        // If new record is good enough to list, it will be placed on the
        // last spot and then moved up to correct place.

        // If list is full, compare to the entry on the last place
        if (TopResults.Length >= maxEntries)
        {
            float weakestTime = TopResults[TopResults.Length - 1].finalTime();
            if (weakestTime <= r.finalTime())
            {
                // Did not make to top list
                return -1;
            }
            // Better than previous last
            TopResults[maxEntries - 1] = r;
        }
        else // if (TopResults.Length < maxEntries)
        {
            // List is not full. Make a new bigger array and add Record to last spot
            Record[] newTR = new Record[TopResults.Length + 1];
            for (int i = 0; i < TopResults.Length; i++)
            {
                newTR[i] = TopResults[i];
            }
            newTR[TopResults.Length] = r;
            TopResults = newTR;
        }

        // Move added record from last spot to it's correct place
        for (int i = TopResults.Length - 1; i > 0; i--)
        {
            if (TopResults[i].finalTime() < TopResults[i - 1].finalTime())
            {
                Record temp = TopResults[i - 1];
                TopResults[i - 1] = TopResults[i];
                TopResults[i] = temp;
            }
            else
            {
                // Return the placement of the result
                return i + 1;
            }
        }

        // Player got new best result
        return 1;
    }

}
