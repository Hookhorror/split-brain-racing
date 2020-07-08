using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using UnityEngine;

public class RecordManager : MonoBehaviour
{
    public static RecordManager Instance;

    private TrackRecords trackRecords;
    private string recordFile = "records.json";


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


    public float GetBestTime(string trackTag)
    {
        float[] times = GetBestRunCheckpoints(trackTag);
        return times[times.Length - 1];
    }


    public Record[] GetRecords(string trackTag)
    {
        return trackRecords.GetRecords(trackTag);
    }


    public string TopResultAsText(string trackTag, int highlighLine)
    {
        Record[] records = RecordManager.Instance.GetRecords(trackTag);
        if (records == null || records.Length == 0)
            return "";

        int hlIdx = highlighLine - 1;   // Line to highlight

        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < records.Length; i++)
        {
            if (i == hlIdx)
            {
                sb.Append("<color=green>");
                Debug.Log("Vihre' rivi");
            }
            sb.Append(i + 1);
            sb.Append(". ");
            sb.Append(records[i].name);
            sb.Append(" - ");
            sb.Append(records[i].finalTime());
            sb.Append("\n");
            if (i == hlIdx)
                sb.Append("</color>");
        }
        Debug.Log(sb.ToString());
        return sb.ToString();
    }


    public string TopResultAsText(string trackTag)
    {
        return TopResultAsText(trackTag, 0);
    }

}
