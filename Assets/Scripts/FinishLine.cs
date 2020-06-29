using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishLine : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Finish line!");
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        Debug.Log("Finish line!");
    }
}
