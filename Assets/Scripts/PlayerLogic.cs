using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLogic : MonoBehaviour
{
    [SerializeField] Vector3 movementVector;
    [SerializeField] Vector3 playerDirection;
    [SerializeField] Rigidbody playerBody;
    public PlayerInput playerInput;
    [SerializeField] Collider playerCollider;
    public bool isUnderwater;
    public int walkSpeed;
    public int jumpForce;
    int boxCastLayerMask = 3;

    public GameObject floor;
    public GameObject[] CloseInteractObjects;

    // Start is called before the first frame update
    void Start()
    {
        playerBody = gameObject.GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
        playerCollider = GetComponent<Collider>();
        playerInput.actions.FindActionMap("UnderWater").Disable();
        playerInput.actions.FindActionMap("Interacting").Disable();
        
    }

    void FixedUpdate()
    {
        //playerBody.AddForce(Physics.gravity,ForceMode.Acceleration);
        MovePlayer();
    }
    RaycastHit[] JumpBoxCastCheck()
    {
        float varMaxDist = playerCollider.bounds.max.y - playerCollider.bounds.min.y + 0.2f;
        Vector3 halfExtentsValues = new Vector3(playerCollider.bounds.extents.x / 2, 0.1f, playerCollider.bounds.extents.z / 2);

        RaycastHit[] boxCastHits = Physics.BoxCastAll(center: new Vector3(playerCollider.transform.position.x,
            playerCollider.bounds.max.y + 0.1f,
            playerCollider.transform.position.z),
            halfExtents: halfExtentsValues,
            direction: -Vector3.up,
            orientation: quaternion.identity,
            maxDistance: varMaxDist,
            layerMask: boxCastLayerMask,
            queryTriggerInteraction: QueryTriggerInteraction.Ignore);
        return boxCastHits;
    }

    void PlayerJump(RaycastHit[] boxCastHits)
    {
        foreach (RaycastHit hit in boxCastHits)
        {
            Debug.Log(hit.collider);
            Debug.Log("ground woo");
            playerBody.AddForce(transform.up * jumpForce, ForceMode.VelocityChange);
            break;
        }
    }

    GameObject CloserIObjectCalculation(Vector3 rayPoint)
    {
        float baseMagnitude = (CloseInteractObjects[0].transform.position - rayPoint).magnitude;
        GameObject closestGameObject = CloseInteractObjects[0];

        foreach (GameObject closeInteractObject in CloseInteractObjects)
        {
            if ((closeInteractObject.transform.position - rayPoint).magnitude < baseMagnitude)
            {
                closestGameObject = closeInteractObject;
            }
        }
        Debug.Log("Closest interaction object is " + closestGameObject.ToString());
        return closestGameObject;
    }

    public void ReadInputValue(InputAction.CallbackContext movementContextInformation)
    {
        string actionTriggeredName = movementContextInformation.action.name;
        //Debug.Log(movementContextInformation.action);
        switch (actionTriggeredName)
        {
            case "Base Movement":
                //Debug.Log("movement");
                Vector2 contextMovementVector2 = movementContextInformation.ReadValue<Vector2>();
                movementVector = new Vector3(contextMovementVector2.x,0,contextMovementVector2.y);

                //Debug.Log("Movement vector is: " + movementVector.ToString());
                MovePlayer();
                break;

            case "Jump":
                if (!movementContextInformation.performed) return;
                RaycastHit[] boxCastHits = JumpBoxCastCheck();
                PlayerJump(boxCastHits);
                break;
                
            case "Swimming": 
                if (!isUnderwater) return;
                Vector3 contextMovementVector3 = movementContextInformation.ReadValue<Vector3>();
                movementVector = contextMovementVector3;

                MovePlayer();
                break;

            case "Interact":
                Debug.Log("case interact hit thanks mr");
                RaycastHit interactableObjectRayInfo;
                bool interactableObjectRayHit = Physics.Raycast(playerBody.transform.position, 
                    -transform.up, 
                    out interactableObjectRayInfo, 
                    maxDistance: 10f,
                    boxCastLayerMask,
                    QueryTriggerInteraction.Ignore);

                if (!interactableObjectRayHit) return;
                CloserIObjectCalculation(interactableObjectRayInfo.point);
                break;

            default:
                Debug.Log("Default case");
                break;
        }
    }
   
    void MovePlayer()
    {
        if (movementVector == Vector3.zero) return;
        //Debug.Log(movementVector);
        playerDirection = movementVector * walkSpeed * Time.fixedDeltaTime;
        playerBody.AddForce(playerDirection, ForceMode.VelocityChange);
    }
}
