using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    [SerializeField] Vector2 movementVector;
    Vector2 playerDirection;
    Rigidbody playerBody;
    [SerializeField] int walkSpeed = 1000;

    // Start is called before the first frame update
    void Start()
    {
        playerBody = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        MovePlayer();
    }

    public void MovementDetected(InputAction.CallbackContext movementContextInformation)
    {
        movementVector = movementContextInformation.ReadValue<Vector2>();
        Debug.Log("Movement vector is: " + movementVector.ToString());
        
    }
   
    void MovePlayer()
    {
        if (movementVector == Vector2.zero) { return; }

        playerDirection = movementVector * walkSpeed * Time.fixedDeltaTime;
        Debug.Log("Player Direction is: " + playerDirection.ToString());
        playerBody.velocity = playerDirection;

    }
   
}
