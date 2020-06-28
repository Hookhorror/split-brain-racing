using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiController : MonoBehaviour
{
    public void UpdateCurrentTime(string time)
    {
        UiManager.instance.currentTime.text = time;
    }

    public void UpdateSplitTime(string time)
    {
        UiManager.instance.splitTime.text = time;
    }

    public void GreenSplitTime()
    {
        UiManager.instance.splitTime.color = Color.green;
    }

    public void RedSplitTime()
    {
        UiManager.instance.splitTime.color = Color.red;
    }
}
