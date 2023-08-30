using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
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
    float gravityMultiplier = 2f;
    public List<GameObject> CloseInteractObjects;
    float rayCastMaxDist = 10f;
    [SerializeField] private DialogueHandler dialogueHandler;
    //InputAction mouseInformation;

    // bool cameraMoveCancelled = false;
    //[SerializeField]
    //float cameraSensitivity = 2f;

    // Start is called before the first frame update
    void Start()
    {
        Physics.gravity *= gravityMultiplier;
        playerBody = gameObject.GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
        playerCollider = GetComponent<Collider>();
        playerInput.actions.FindActionMap("UnderWater").Disable();
        playerInput.actions.FindActionMap("Interacting").Disable();
        //mouseInformation = playerInput.actions.FindAction("Mouse Information");
    }

    void FixedUpdate()
    {
        //playerBody.AddForce(Physics.gravity * gravityMultiplier,ForceMode.Acceleration);
        MovePlayer();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(Camera.main.transform.position, new Vector3(playerBody.transform.position.x, playerBody.transform.position.y, playerBody.transform.position.z + rayCastMaxDist));
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
    public void ReadInputValue(InputAction.CallbackContext inputContextInformation)
    {
        string actionTriggeredName = inputContextInformation.action.name;
        //Debug.Log(inputContextInformation.action);
        switch (actionTriggeredName)
        {
            case "Base Movement":
                //Debug.Log("movement");
                Vector2 contextMovementVector2 = inputContextInformation.ReadValue<Vector2>();
                movementVector = new Vector3(contextMovementVector2.x,0,contextMovementVector2.y);

                //Debug.Log("Movement vector is: " + movementVector.ToString());
                MovePlayer();
                break;

            case "Jump":
                if (!inputContextInformation.performed) return;
                RaycastHit[] boxCastHits = JumpBoxCastCheck();
                PlayerJump(boxCastHits);
                break;
                
            case "Swimming": 
                if (!isUnderwater) return;
                Vector3 contextMovementVector3 = inputContextInformation.ReadValue<Vector3>();
                movementVector = contextMovementVector3;

                MovePlayer();
                break;

            case "Interact":
                if (!inputContextInformation.performed) return;
                //Debug.Log("=====");
                //Debug.Log("case interact ");
                InteractionLogic(CloseInteractObjects[0]);
                break;

            case "Camera Control":
                /*
                if (inputContextInformation.canceled)
                {
                    cameraMoveCancelled = true;
                }
                if (!inputContextInformation.performed) { return; }
                cameraMoveCancelled = false;
                StartCoroutine(MoveCamera());
                break;
                */

            default:
                Debug.Log("Default case");
                break;
        }
    }
    void InteractionLogic(GameObject interactObject)
    {
        
        switch (interactObject.GetComponent<InteractableObject>().interactableObjectType)
        {
            case "Dialogue":
                Debug.Log("dialogue case");
                StartCoroutine(dialogueHandler.ChangeDialogue(CloseInteractObjects[0].GetComponent<InteractableObject>().dialogue));
                playerInput.actions.FindActionMap("Interacting").Disable();
                break;

            case "Pickup":
                Debug.Log("pickup case");
                break;

            default:
                Debug.LogWarning("default interact case");
                break;
        }
    }
    /*
    IEnumerator MoveCamera()
    {
        while (!cameraMoveCancelled)
        {
            Vector2 mouseDelta = mouseInformation.ReadValue<Vector2>();
            Vector2 cameraChange = mouseDelta * cameraSensitivity;

            Debug.Log("Camera Change is: "+ cameraChange);
            Camera.main.transform.eulerAngles += new Vector3(cameraChange.x,cameraChange.y,0);
            yield return new WaitForSeconds(0.1f);
        }
        yield break;

    }
    */
    
    void MovePlayer()
    {
        if (movementVector == Vector3.zero) return;
        //Debug.Log(movementVector);
        //Dot product mess from: https://forum.unity.com/threads/how-do-i-get-a-vector-in-relation-to-another-vector.105723/
        float dotProduct = Vector3.Dot(playerDirection,transform.forward);
        Vector3 _playerDirection = (playerDirection + new Vector3(dotProduct,dotProduct,dotProduct)) + transform.forward;

        playerDirection = _playerDirection.normalized * walkSpeed * Time.fixedDeltaTime;
        playerBody.AddForce(playerDirection, ForceMode.VelocityChange);
        transform.eulerAngles += new Vector3(0,movementVector.x,0);
    }
}
