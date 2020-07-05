using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

using UnityEngine;


[Serializable]
[DataContract]
public class TrackRecords
{
    // Class contains array of records TrackRecords and
    // Dictionary that can be used to find index of the track with
    // track's tag.
    [DataMember]
    public TrackRecord[] trackRecords;

    public Dictionary<string, int> indices;
    public int maxEntries;


    public void Init()
    {
        indices = new Dictionary<string, int>();
        if (trackRecords == null)
            trackRecords = new TrackRecord[0];
        for (int i = 0; i < trackRecords.Length; i++)
        {
            indices.Add(trackRecords[i].trackTag, i);
        }
        maxEntries = 10;
    }


    public Record[] GetRecords(string trackTag)
    {
        int index;
        if (indices.TryGetValue(trackTag, out index))
        {
            return trackRecords[index].GetRecords();
        }
        return new Record[0];

    }


    /// Adds a record to track and returns records placement on a list or
    /// -1 if record is not good enough to list.
    public int AddRecord(string trackTag, string name, float[] checkpoints)
    {
        int index = -1;
        bool trackExists = indices.TryGetValue(trackTag, out index);
        Debug.Log("Index " + index);


        if (!trackExists)
        {
            Debug.Log("TrackTag's Index not found. Adding a new.");
            index = AddNewTrack(trackTag);
        }

        return trackRecords[index].AddRecord(name, checkpoints, maxEntries);
    }


    private int AddNewTrack(string tag)
    {
        Debug.Log("Adding new track");
        Debug.Log("trackRecords pituus " + trackRecords.Length);
        TrackRecord[] tr = new TrackRecord[trackRecords.Length + 1];
        for (int i = 0; i < trackRecords.Length; i++)
        {
            tr[i] = trackRecords[i];
        }

        tr[tr.Length - 1] = new TrackRecord(tag);
        indices.Add(tag, tr.Length - 1);
        Debug.Log("tr pituus " + tr.Length);
        trackRecords = tr;

        return tr.Length - 1;
    }

}
