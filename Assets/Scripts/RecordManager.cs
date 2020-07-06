using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using UnityEngine;

public class RecordManager : MonoBehaviour
{
    public static RecordManager Instance;

    private TrackRecords trackRecords;
    private string recordFile = "records.json";
    private bool recordsLoaded = false;


    void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        LoadRecords();
    }


    private void LoadRecords()
    {
        if (!File.Exists(recordFile))
        {
            Debug.LogWarning("Record file does not exist");
            trackRecords = new TrackRecords();
        }
        else
        {
            FileStream fs = File.OpenRead(recordFile);
            var ser = new DataContractJsonSerializer(typeof(TrackRecords));
            trackRecords = (TrackRecords)ser.ReadObject(fs);
            fs.Close();
        }
        trackRecords.Init();
        return;
    }


    private void SaveRecords()
    {
        // StreamWriter writer = new StreamWriter(recordFile);
        FileStream fs = File.OpenWrite(recordFile);

        string trackRecordsJson = JsonUtility.ToJson(trackRecords, true);
        var ser = new DataContractJsonSerializer(typeof(TrackRecords));

        ser.WriteObject(fs, trackRecords);
        fs.Close();

    }


    public int AddRecord(string trackTag, string name, float[] checkpoints)
    {
        int placement = trackRecords.AddRecord(trackTag, name, checkpoints);
        SaveRecords();
        return placement;
    }


    public float[] GetBestRunCheckpoints(string trackTag)
    {
        return trackRecords.GetBestRunCheckpoints(trackTag);
    }


    public Record[] GetRecords(string trackTag)
    {
        return trackRecords.GetRecords(trackTag);
    }

}
