using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class outoftracktest : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D other)
    {
        // Debug.Log("JEE");
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log("PÖÖ");
    }
}
