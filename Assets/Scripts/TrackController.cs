using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackController : MonoBehaviour
{

    // public checkpoints;

    public int goldTime;
    public int silverTime;
    public int bronzeTime;
    private float startTime;
    private float[] checkpointTimes = new float[] { 3, 4, 6 };

    void Start()
    {
        // gameObject.GetComponentsInChildren(Checkpoint);
        // Find checkpoints
        var cpList = GameObject.FindGameObjectsWithTag("Checkpoint");

        Debug.Log(cpList.Length);
        foreach (var cp in cpList)
        {
            Debug.Log(cp.name);
        }
        startTime = Time.time;

    }


    void Update()
    {
        float time = TakeTime();
    }


    float TakeTime()
    {
        float time = Time.time - startTime;
        return time;
    }

}