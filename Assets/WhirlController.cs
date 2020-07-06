using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhirlController : MonoBehaviour
{
    public float pushPower = 0;
    private Vector2 distance; // Debug purpose


    void Start()
    {
        if (pushPower == 0)
            pushPower = 40;

    }


    void Update()
    {

    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag("Ship"))
            return;

        Vector2 shipPos = other.transform.position;
        distance = (Vector2)transform.position - shipPos;
        distance.Normalize();
        float temp = distance.x * -1;
        distance.x = distance.y;
        distance.y = temp;

        Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
        rb.AddForce(distance * pushPower);
    }


    private void OnTriggerStay2D(Collider2D other)
    {
        Vector2 shipPos = other.transform.position;
        distance = (Vector2)transform.position - shipPos;
        distance.Normalize();
    }

}