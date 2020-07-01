using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour
{
    public Rigidbody2D rbody;
    Vector2 lastCheckpoint;
    Vector2 startPoint;
    private bool resetRequested = false;

    void Start()
    {

    }


    // Update is called once per frame
    void Update()
    {

    }


    void FixedUpdate()
    {
        if (resetRequested)
            ResetToLastCheckpoint();
    }


    public void ResetToLastCheckpoint()
    {
        transform.position = lastCheckpoint;
        rbody.velocity = Vector2.zero;
        resetRequested = false;
    }


    public void SetLastCheckpoint(Vector2 cp)
    {
        // Debug.Log("Uusi lastChekpoint: " + cp);
        lastCheckpoint = cp;
    }


    public void RequestReset()
    {
        resetRequested = true;
    }


    void DubugOncePerSec()
    {
        // Debug.Log(rbody.velocity);
    }


    public void SetStartPoint()
    {

    }
}
