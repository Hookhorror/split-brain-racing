using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    public static ObstacleManager instance;

    private void Awake()
    {
        instance = this;
    }

    public float whirlRotationTime;
}
