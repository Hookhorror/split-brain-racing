using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointController : MonoBehaviour
{

    public Vector2 resetCoords;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    public void OnTriggerEnter2D(Collider2D col)
    {
        var ship = GameObject.FindGameObjectWithTag("Ship");
        ship.GetComponent<ShipController>().SetLastCheckpoint(resetCoords);
        // Set Last checkpoint for the ship
    }
}
