using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    [SerializeField] Vector3 movementVector;
    [SerializeField] Vector3 playerDirection;
    [SerializeField] Rigidbody playerBody;
    [SerializeField] PlayerInput playerInput;
    [SerializeField] Collider playerCollider;
    bool isUnderwater;
    public int walkSpeed;
    public int jumpForce;
    public LayerMask playerLayerMask;

    // Start is called before the first frame update
    void Start()
    {
        playerBody = gameObject.GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
        playerInput.actions["Underwater Bindings"].Disable();
        playerCollider = GetComponent<Collider>();
    }

    void FixedUpdate()
    {
        playerBody.AddForce(Physics.gravity,ForceMode.Acceleration);
        MovePlayer();
    }
    public void ReadInputValue(InputAction.CallbackContext movementContextInformation)
    {
        string actionTriggeredName = movementContextInformation.action.name;
        //Debug.Log(movementContextInformation.action);
        switch (actionTriggeredName)
        {
            case "Base Movement":
                //Debug.Log("movement");
                Vector2 contextMovementVector = movementContextInformation.ReadValue<Vector2>();
                movementVector = new Vector3(contextMovementVector.x,0,contextMovementVector.y);

                //Debug.Log("Movement vector is: " + movementVector.ToString());
                MovePlayer();
                break;

            case "Jump":
                if (!movementContextInformation.performed) return;
                //bool groundCheck = Physics.Raycast(transform.position, -transform.up, playerCollider.bounds.extents.y + 0.1f);
                //Debug.Log("Player is on the ground? :" + groundCheck.ToString());

                //if (!groundCheck) return;
                playerBody.AddForce(transform.up * jumpForce, ForceMode.VelocityChange);
                break;

            case "Underwater Bindings": 
                if (!isUnderwater) return;
                //Debug.Log("underwater");
                float contextMovementFloat = movementContextInformation.ReadValue<float>();
                movementVector = new Vector3(0, contextMovementFloat, 0);
                MovePlayer();
                break;

            default:
                Debug.Log("uh oh god i dont know its none of the action names");
                break;
        }
    }
   
    void MovePlayer()
    {
        if (movementVector == Vector3.zero) return; 

        playerDirection = movementVector * walkSpeed * Time.fixedDeltaTime;

        playerBody.AddForce(playerDirection, ForceMode.VelocityChange);
    }

}
