using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    [SerializeField] Vector3 movementVector;
    Vector3 playerDirection;
    Rigidbody playerBody;
    public int walkSpeed = 100;

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
        movementVector = movementContextInformation.ReadValue<Vector3>();
        Debug.Log("Movement vector is: " + movementVector.ToString());
        
    }
   
    void MovePlayer()
    {
        if (movementVector == Vector3.zero) { return; }

        playerDirection = movementVector * walkSpeed * Time.fixedDeltaTime;
        Debug.Log("Player Direction is: " + playerDirection.ToString());
        Debug.Log("=================");
        playerBody.AddForce(playerDirection, ForceMode.VelocityChange);
    }
   
}
