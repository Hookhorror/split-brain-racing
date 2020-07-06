using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedalManager : MonoBehaviour
{
    public static MedalManager Instance;

    public Dictionary<string, MedalTime> medalTimes;

    public Sprite gold;
    public Sprite silver;
    public Sprite bronze;
    public Sprite noMedals;


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

        medalTimes = new Dictionary<string, MedalTime>();
        LoadMedalTimes();
    }


    private void LoadMedalTimes()
    {
        string name = "track1";
        float[] g = new float[9] { 7, 14, 21, 28, 35, 42, 49, 56, 51 };
        float[] s = new float[9] { 9, 18, 27, 36, 45, 54, 63, 72, 55 };
        float[] b = new float[9] { 10, 20, 30, 40, 50, 60, 70, 80, 58 };
        MedalTime track1 = new MedalTime(g, s, b);
        this.medalTimes.Add(name, track1);
    }


    public Sprite GetEarnedMedal(string trackTag)
    {
        float record = RecordManager.Instance.GetBestTime(trackTag);
        MedalTime mt;

        if (!medalTimes.TryGetValue(trackTag, out mt))
            return noMedals;

        if (record < mt.goldTime)
            return gold;

        if (record < mt.silverTime)
            return silver;

        if (record < mt.bronzeTime)
            return bronze;

        return noMedals;
    }


    public float GetGoldFinalTime(string trackTag)
    {
        MedalTime mt;
        if (medalTimes.TryGetValue(trackTag, out mt))
            return mt.goldTime;

        Debug.LogError("Gold time not found for trackTag " + trackTag);
        return 99999f;
    }


    public float GetSilverFinalTime(string trackTag)
    {
        MedalTime mt;
        if (medalTimes.TryGetValue(trackTag, out mt))
            return mt.silverTime;

        Debug.LogError("Silver time not found for trackTag " + trackTag);
        return 99999f;
    }


    public float GetBronzeFinalTime(string trackTag)
    {
        MedalTime mt;
        if (medalTimes.TryGetValue(trackTag, out mt))
            return mt.bronzeTime;

        Debug.LogError("Bronze time not found for trackTag " + trackTag);
        return 99999f;
    }


    public float[] GetGoldCheckpoints(string trackTag)
    {
        MedalTime mt;
        if (medalTimes.TryGetValue(trackTag, out mt))
            return mt.goldTimes;

        return null;
    }


    public float[] GetSilverCheckpoints(string trackTag)
    {
        MedalTime mt;
        if (medalTimes.TryGetValue(trackTag, out mt))
            return mt.silverTimes;

        return null;
    }


    public float[] GetBronzeCheckpoints(string trackTag)
    {
        MedalTime mt;
        if (medalTimes.TryGetValue(trackTag, out mt))
            return mt.bronzeTimes;

        return null;
    }

}
