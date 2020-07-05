using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    private float rotationSpeed;
    private float size;
    private float angleToRotate;

    private void Start()
    {
        rotationSpeed = ObstacleManager.instance.whirlRotationTime;
        size = transform.localScale.x;
        angleToRotate = rotationSpeed / (size * size);
        Debug.Log(angleToRotate);
    }
    void Update()
    {
        // float angle = Mathf.Lerp(0, angleToRotate, 0.5f);
        // Debug.Log(angle);
        // transform.Rotate(transform.rotation.x, transform.rotation.y, transform.rotation.z + angleToRotate);
        transform.RotateAround(Vector3.forward, angleToRotate);
        // transform.Rotate();

    }
}
