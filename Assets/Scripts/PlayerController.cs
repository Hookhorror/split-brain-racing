using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerController : MonoBehaviour
{

    private PlayerInput m_PlayerInput;
    private InputAction m_LookAction;
    private InputAction m_MoveAction;
    private InputAction m_FireAction;
    public float motorPower;

    Rigidbody2D rbody;

    // Start is called before the first frame update
    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
        if (m_PlayerInput == null)
        {
            // m_PlayerInput = GetComponent<PlayerInput>();
            m_PlayerInput = (PlayerInput)gameObject.GetComponent("PlayerInput");
            m_FireAction = m_PlayerInput.actions["fire"];
            m_LookAction = m_PlayerInput.actions["look"];
            m_MoveAction = m_PlayerInput.actions["move"];
        }
    }


    void FixedUpdate()
    {
        Rigidbody2D shipRBody = GameObject.FindGameObjectWithTag("Ship")
            .GetComponent<Rigidbody2D>();
        var movement = m_MoveAction.ReadValue<Vector2>();
        if (movement != Vector2.zero)
        {
            Vector2 forceToAdd = movement * motorPower;
            Vector2 pos = transform.position;
            forceToAdd.Normalize();
            Debug.Log(forceToAdd);
            shipRBody.AddForce(forceToAdd);
            rbody.AddForce(forceToAdd);
            transform.position = Vector2.zero;
        }
    }

    // Update is called once per frame
    void Update()
    {
        {
            if (m_FireAction.triggered)
                Debug.Log("Ammutaan");

            var look = m_LookAction.ReadValue<Vector2>();
            if (look != Vector2.zero)
                Debug.Log("Look:" + look);
            /* Update transform from move&look... */
        }
    }


    public void OnMove()
    {
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


    public void look()
    {
        // Debug.Log("Katsoo");
    }



    public void Shoot()
    {
        // Debug.Log("Ampuu");
    }
}
