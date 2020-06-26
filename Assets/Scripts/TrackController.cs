using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackController : MonoBehaviour
{

    // public checkpoints;

    public int goldTime;
    public int silverTime;
    public int bronzeTime;

    void Start()
    {
        // gameObject.GetComponentsInChildren(Checkpoint);
        // Find checkpoints
        var cpList = GameObject.FindGameObjectsWithTag("checkpoints");


        Debug.Log(transform.childCount);
        goldTime = 5;
        silverTime = 6;
        bronzeTime = 7;
    }
}