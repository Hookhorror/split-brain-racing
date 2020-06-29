﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    private PlayerInput m_PlayerInput;
    // private InputAction m_LookAction;
    private InputAction m_MoveAction;
    private InputAction m_FireAction;
    public float motorPower;
    public float shipSize;  // Affects to the postion of flames
    Rigidbody2D playerRBody;
    SpriteRenderer sr;
    bool checkpointResetRequested;
    GameObject ship;
    Rigidbody2D shipRBody;
    private bool moveIsEnabled = false;
    private bool resetIsEnabled = false;


    void Start()
    {
        playerRBody = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        checkpointResetRequested = false;
        ship = GameObject.FindGameObjectWithTag("Ship");
        shipRBody = ship.GetComponent<Rigidbody2D>();

        if (m_PlayerInput == null)
        {
            m_PlayerInput = (PlayerInput)gameObject.GetComponent("PlayerInput");
            m_FireAction = m_PlayerInput.actions["ResetToCheckpoint"];
            m_MoveAction = m_PlayerInput.actions["move"];
        }

        // m_PlayerInput.enabled = false;
        // m_PlayerInput.SwitchCurrentActionMap();
    }


    void FixedUpdate()
    {
        if (!moveIsEnabled)
            return;
        if (checkpointResetRequested)
        {
            ship.GetComponent<ShipController>().ResetToLastCheckpoint();
            checkpointResetRequested = false;
        }
        var movement = GetNormalizedMovement();
        if (movement != Vector2.zero)
        {
            Vector2 forceToAdd = movement * motorPower;
            // Debug.Log(forceToAdd);
            shipRBody.AddForce(forceToAdd);
        }
    }


    public void EnableControls()
    {
        moveIsEnabled = true;
        resetIsEnabled = true;
    }


    public void DisableControls()
    {
        moveIsEnabled = false;
        resetIsEnabled = false;
    }


    // Update is called once per frame
    void Update()
    {
        FlameControl();
    }


    /// Controls the visibiliti, postition and angel if the player's flame
    void FlameControl()
    {
        var movement = GetNormalizedMovement();
        if (movement == Vector2.zero)
        {
            sr.enabled = false;
            return;
        }

        sr.enabled = true;


        // Position around ship
        Vector3 offset = (Vector3)GetNormalizedMovement() * shipSize;
        transform.position = ship.transform.position - offset;

        // Angle
        var angle = Mathf.Atan2(movement.y, movement.x) * Mathf.Rad2Deg - 180f;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }


    public void OnMove()
    {
    }


    /// Returns player's stick position as normalized Vector2 or Vector2.zero
    Vector2 GetNormalizedMovement()
    {
        var movement = m_MoveAction.ReadValue<Vector2>();
        if (movement != Vector2.zero)
            movement.Normalize();
        return movement;
    }


    public void OnResetToCheckpoint()
    {
        if (!resetIsEnabled)
            return;
        // Debug.Log("Checkpoint palautus");
        checkpointResetRequested = true;
    }


    public void OnResetToStart()
    {
        if (!resetIsEnabled)
            return;
        checkpointResetRequested = true;
    }


    public void Shoot()
    {
        // Debug.Log("Ampuu");
    }
}
