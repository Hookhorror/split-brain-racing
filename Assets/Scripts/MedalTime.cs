using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedalTime
{
    public float[] goldTimes;
    public float[] silverTimes;
    public float[] bronzeTimes;
    public float goldTime;
    public float silverTime;
    public float bronzeTime;


    public MedalTime(float[] gold, float[] silver, float[] bronze)
    {
        goldTimes = gold;
        silverTimes = silver;
        bronzeTimes = bronze;

        goldTime = goldTimes[goldTimes.Length - 1];
        silverTime = silverTimes[silverTimes.Length - 1];
        bronzeTime = bronzeTimes[bronzeTimes.Length - 1];
    }
}
