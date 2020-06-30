﻿using UnityEngine;
using TMPro;

public class UiManager : MonoBehaviour
{
    public static UiManager Instance;
    public TextMeshProUGUI currentTime;
    public TextMeshProUGUI splitTime;
    public GameObject countdown;
    public Animator animator;


    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
        countdown.SetActive(false);
    }

    public void SetCurrentTime(float time)
    {
        currentTime.text = (string.Format("{0:0.00}", time));
    }


    public void SetSplitTime(float time)
    {
        splitTime.color = (time < 0) ? Color.green : Color.red;
        splitTime.text = (string.Format("{0:+0.00;-0.00}", time));
    }


    public void PlayCountdown()
    {
        countdown.SetActive(true);
        Invoke("DisableCountdown", 4);
        // animator.Play("Countdown_Countdown");
    }


    private void DisableCountdown()
    {
        countdown.SetActive(false);
    }
}