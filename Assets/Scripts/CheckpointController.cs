using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointController : MonoBehaviour
{
    public Transform resetPoint;
    private bool isUsed = false;
    private GameObject ship;


    // Start is called before the first frame update
    void Start()
    {
        ship = GameObject.FindGameObjectWithTag("Ship");
    }


    public void OnTriggerEnter2D(Collider2D col)
    {
        // Each checkpoint can be used only once
        if (isUsed)
            return;

        if (!col.gameObject.CompareTag("Ship"))
            return;

        ship.GetComponent<ShipController>().SetLastCheckpoint(resetPoint.position);
        isUsed = true;
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
