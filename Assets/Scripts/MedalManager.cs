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
        float[] g = new float[9] { 1.86f, 8.74f, 13.32f, 18.86f, 25.24f, 28.86f, 38.24f, 43.36f, 51.90f };
        float[] s = new float[9] { 2.122f, 9.74f, 15.32f, 21.86f, 29.24f, 32.86f, 43.24f, 48.96f, 59.90f };
        float[] b = new float[9] { 2.80f, 11.70f, 18.65f, 26.05f, 34.07f, 37.73f, 52.35f, 58.21f, 68.79f };

        MedalTime track1 = new MedalTime(g, s, b);
        this.medalTimes.Add(name, track1);

        string toka = "AntinScene";
        float[] g2 = new float[3] { 1.86f, 8.74f, 1.32f };
        float[] s2 = new float[3] { 2.122f, 9.74f, 1.32f };
        float[] b2 = new float[3] { 2.80f, 11.70f, 1.65f };

        MedalTime AntinScene = new MedalTime(g2, s2, b2);
        this.medalTimes.Add(toka, AntinScene);

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


    public MedalTime GetMedalTimes(string trackTag)
    {
        MedalTime mt;
        if (medalTimes.TryGetValue(trackTag, out mt))
            return mt;

        return null;
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
