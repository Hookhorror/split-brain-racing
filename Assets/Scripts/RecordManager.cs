using System.Collections;
using System.Collections.Generic;
using System.IO;
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
        trackRecords.Init();
        return;
    }


    private void SaveRecords()
    {
        string resultJson = JsonUtility.ToJson(trackRecords, true);
        StreamWriter writer = new StreamWriter(recordFile);
        writer.Write(resultJson);
        writer.Close();

    }


    public int AddRecord(string trackTag, string name, float[] checkpoints)
    {
        int placement = trackRecords.AddRecord(trackTag, name, checkpoints);
        SaveRecords();
        return placement;
    }

}
