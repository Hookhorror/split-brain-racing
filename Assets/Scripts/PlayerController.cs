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
    public float motorPower;
    public float shipSize;  // Affects to the postion of flames
    Rigidbody2D rbody;
    SpriteRenderer sr;

    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        if (m_PlayerInput == null)
        {
            m_PlayerInput = (PlayerInput)gameObject.GetComponent("PlayerInput");
            m_FireAction = m_PlayerInput.actions["fire"];
            m_MoveAction = m_PlayerInput.actions["move"];
        }
    }


    void FixedUpdate()
    {
        var movement = GetNormalizedMovement();
        if (movement != Vector2.zero)
        {
            // Get ship's RigidBody
            Rigidbody2D shipRBody = GameObject.FindGameObjectWithTag("Ship")
                .GetComponent<Rigidbody2D>();

            Vector2 forceToAdd = movement * motorPower;
            Debug.Log(forceToAdd);
            shipRBody.AddForce(forceToAdd);
            // rbody.AddForce(forceToAdd);
            // transform.position = Vector2.zero;
        }
    }

    // Update is called once per frame
    void Update()
    {
        {
            FlameControl();
        }
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
        Rigidbody2D shipRBody = GameObject.FindGameObjectWithTag("Ship")
            .GetComponent<Rigidbody2D>();
        GameObject ship = GameObject.FindGameObjectWithTag("Ship");
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


    public void Move(string str)
    {
        Debug.Log("Liikutaan" + str);
        Vector2 pos = gameObject.transform.position;
        // transform.position = pos + new Vector2(1, 1);
    }


    public void run(string teksti)
    {
        Debug.Log("Juostaan");
    }


    public void Shoot()
    {
        // Debug.Log("Ampuu");
    }
}
