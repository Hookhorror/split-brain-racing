﻿using System.Collections;
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
    }


    [System.Obsolete]
    void Update()
    {
        // transform.RotateAround(Vector3.forward, angleToRotate);
        transform.Rotate(0, 0, 3f);
    }
}
