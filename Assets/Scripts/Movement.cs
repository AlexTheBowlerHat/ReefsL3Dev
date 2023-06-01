using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    [SerializeField] 
    Vector3 movementVector;
    Vector2 contextMovementVector;
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

    public void ReadInputValue(InputAction.CallbackContext movementContextInformation)
    {
        Debug.Log(movementContextInformation);
        contextMovementVector = movementContextInformation.ReadValue<Vector2>();
        movementVector = new Vector3(contextMovementVector.x,0,contextMovementVector.y);
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
