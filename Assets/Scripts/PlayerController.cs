using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    private PlayerInput m_PlayerInput;
    // private InputAction m_LookAction;
    private InputAction m_MoveAction;
    private InputAction m_FireAction;
    // public AudioManager audioManager;
    public float motorPower;
    public float shipSize;  // Affects to the postion of flames
    // Rigidbody2D playerRBody;
    SpriteRenderer sr;
    bool resetToCpRequested;
    bool resetToStartRequested;
    GameObject ship;
    Rigidbody2D shipRBody;
    private bool moveIsEnabled = false;
    private bool resetIsEnabled = false;
    private Sound motorSound;
    public AudioClip motorClip;


    void Awake()
    {
        // This is in Awake. In start is was not selected quick enough
        // for trackController's calls to SetColor().
        sr = GetComponent<SpriteRenderer>();
    }


    void SetUpSound()
    {
        motorSound = new Sound();
        motorSound.source = gameObject.AddComponent<AudioSource>();
        motorSound.source.clip = motorClip;
        motorSound.source.loop = true;
    }


    void Start()
    {
        // playerRBody = GetComponent<Rigidbody2D>();
        // sr = GetComponent<SpriteRenderer>();
        resetToCpRequested = false;
        ship = GameObject.FindGameObjectWithTag("Ship");
        shipRBody = ship.GetComponent<Rigidbody2D>();

        if (m_PlayerInput == null)
        {
            m_PlayerInput = (PlayerInput)gameObject.GetComponent("PlayerInput");
            m_FireAction = m_PlayerInput.actions["ResetToCheckpoint"];
            m_MoveAction = m_PlayerInput.actions["move"];
        }

        // InputActionMap iam = m_PlayerInput.actions.FindActionMap("UI");
        // iam.Disable();

        SetUpSound();
    }


    void FixedUpdate()
    {
        if (resetToStartRequested)
        {
            // ship.GetComponent<ShipController>().ResetToLastCheckpoint();
            resetToStartRequested = false;
            resetToCpRequested = false;

            // Ask trackControl to reset the track
            GameObject.FindGameObjectWithTag("Track")
                .GetComponent<TrackController>().ResetToStart();
        }

        if (resetIsEnabled && resetToCpRequested)
        {
            ship.GetComponent<ShipController>().ResetToLastCheckpoint();
            resetToCpRequested = false;
        }

        if (!moveIsEnabled)
            return;

        var movement = GetNormalizedMovement();
        if (movement != Vector2.zero)
        {
            Vector2 forceToAdd = movement * motorPower;
            // Debug.Log(forceToAdd);
            shipRBody.AddForce(forceToAdd);
        }
    }


    public void SetColor(Color color)
    {
        sr.color = color;
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


    /// Controls the visibility, postition and angel of the player's flame
    void FlameControl()
    {
        var movement = GetNormalizedMovement();

        // Flame is not visible if player is not turning the stick
        if (movement == Vector2.zero || !moveIsEnabled)
        {
            sr.enabled = false;
            StopMotorSound();
            return;
        }

        sr.enabled = true;
        PlayMotorSound();

        // Position around ship
        Vector3 offset = (Vector3)GetNormalizedMovement() * shipSize;
        transform.position = ship.transform.position - offset;

        // Angle
        var angle = Mathf.Atan2(movement.y, movement.x) * Mathf.Rad2Deg + 90f;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }


    public void OnMove()
    {
    }


    private void PlayMotorSound()
    {
        if (motorSound.source.isPlaying)
            return;

        motorSound.source.Play();
    }


    private void StopMotorSound()
    {
        if (motorSound.source.isPlaying)
            motorSound.source.Stop();
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
        resetToCpRequested = true;
    }


    public void OnResetToStart()
    {
        // Reset to start is controlled by trackController.
        resetToStartRequested = true;
        Debug.Log("Reset to START REQUESTED " + resetToStartRequested);
    }


    public void Shoot()
    {
        // Debug.Log("Ampuu");
    }
}
