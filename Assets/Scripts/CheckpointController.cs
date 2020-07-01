using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointController : MonoBehaviour
{
    public Transform resetPoint;
    private bool isUsed = false;
    private float splitTime;
    private GameObject ship;
    private GameObject track;


    // Start is called before the first frame update
    void Start()
    {
        ship = GameObject.FindGameObjectWithTag("Ship");
        track = GameObject.FindGameObjectWithTag("Track");
    }


    public void OnTriggerEnter2D(Collider2D col)
    {
        // Each checkpoint can be used only once
        if (isUsed)
            return;

        if (!col.gameObject.CompareTag("Ship"))
            return;

        // Set a new resetpoint for the ship
        ship.GetComponent<ShipController>().SetLastCheckpoint(resetPoint.position);
        // Inform the track that this checkpoint has been reached
        var tc = track.GetComponent<TrackController>();
        tc.PlaySound("Checkpoint");
        tc.SplitTime(splitTime);

        isUsed = true;
    }


    public void SetTime(float time)
    {
        splitTime = time;
        // Debug.Log("Checkpoints split time" + splitTime);
    }


    public void ResetStatus()
    {
        isUsed = false;
    }


    // Debug

    /// Draw an attack area for a weapon in editor
    void OnDrawGizmosSelected()
    {
        if (resetPoint == null)
            return;
        Gizmos.DrawWireSphere(resetPoint.position, 0.25f);
    }
}
