using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour
{
    Rigidbody2D rbody;
    Vector2 lastCheckpoint;

    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
        lastCheckpoint = Vector2.zero;
        InvokeRepeating("DubugOncePerSec", 1, 1);
    }


    // Update is called once per frame
    void Update()
    {

    }


    void FixedUpdate()
    {

    }


    public void ResetToLastCheckpoint()
    {
        transform.position = lastCheckpoint;
        rbody.velocity = Vector2.zero;
    }


    public void SetLastCheckpoint(Vector2 cp)
    {
        Debug.Log("Uusi lastChekpoint: " + cp);
        lastCheckpoint = cp;
    }


    void DubugOncePerSec()
    {
        Debug.Log(rbody.velocity);
    }
}
