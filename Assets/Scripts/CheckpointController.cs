using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointController : MonoBehaviour
{
    public Transform resetPoint;


    // Start is called before the first frame update
    void Start()
    {

    }


    public void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag != "Ship")
            return;
        var ship = GameObject.FindGameObjectWithTag("Ship");
        ship.GetComponent<ShipController>().SetLastCheckpoint(resetPoint.position);
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
