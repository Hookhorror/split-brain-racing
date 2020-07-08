using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// Collects checkpoints from it's childs
public class CheckpointManager : MonoBehaviour
{
    public GameObject[] checkpoints;

    void Awake()
    {
        // checkpoints = GameObject.FindGameObjectsWithTag("Checkpoint");
    }


    public GameObject[] GetCheckpoints()
    {
        return checkpoints;
    }
}
