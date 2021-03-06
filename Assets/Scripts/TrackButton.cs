﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class TrackButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string trackTag;
    public Sprite ima;
    private Image medalsImage;



    private void Start()
    {
        // ima = Resources.Load<Sprite>("medals_gold_aquired.png");
        medalsImage = transform.GetChild(1).GetComponent<Image>();
        medalsImage.sprite = MedalManager.Instance.GetEarnedMedal(trackTag);
    }



    // Called when the pointer enters our GUI component.
    // Start tracking the mouse
    public void OnPointerEnter(PointerEventData eventData)
    {
        StartCoroutine("TrackPointer");
        Debug.Log("Pointer enter");
        TrackSelection.Instance.ShowResults(trackTag);
    }


    // Called when the pointer exits our GUI component.
    // Stop tracking the mouse
    public void OnPointerExit(PointerEventData eventData)
    {
        StopCoroutine("TrackPointer");
        Debug.Log("Pointer exit");
    }


    public void LoadTrack()
    {
        TrackSelection.Instance.LoadTrack(trackTag);
    }


    IEnumerator TrackPointer()
    {
        var ray = GetComponentInParent<GraphicRaycaster>();
        var input = FindObjectOfType<StandaloneInputModule>();

        if (ray != null && input != null)
        {
            while (Application.isPlaying)
            {
                Vector2 localPos; // Mouse position
                RectTransformUtility.ScreenPointToLocalPointInRectangle(transform as RectTransform, Input.mousePosition, ray.eventCamera, out localPos);

                // local pos is the mouse position.

                yield return 0;
            }
        }
        else
            UnityEngine.Debug.LogWarning("Could not find GraphicRaycaster and/or StandaloneInputModule");
    }

}
