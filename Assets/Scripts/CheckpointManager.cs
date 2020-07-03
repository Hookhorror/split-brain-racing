using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// Collects checkpoints from it's childs
public class CheckpointManager : MonoBehaviour
{
    private GameObject[] checkpoints;

    void Awake()
    {
        checkpoints = GameObject.FindGameObjectsWithTag("Checkpoint");
    }


    public GameObject[] GetCheckpoints()
    {
        return checkpoints;
    }
}
