using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerLogic : MonoBehaviour
{
    [SerializeField] Vector3 movementVector;
    [SerializeField] float angleToMoveVector;
    [SerializeField] Vector3 playerVelocity;
    float rotationSpeed = 125f;
    public int walkSpeed;
    public int jumpForce;
    float gravityMultiplier = 2f;
        
    int boxCastLayerMask = 3;
    float rayCastMaxDist = 10f;
    [SerializeField] Rigidbody playerBody;
    public PlayerInput playerInput;
    [SerializeField] Collider playerCollider;
    public bool isUnderwater;

    public List<GameObject> CloseInteractObjects;

    [SerializeField] private DialogueHandler dialogueHandler;
    public Coroutine dialogueCoroutine;
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
            //Debug.Log(hit.collider);
            //Debug.Log("ground woo");
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
                dialogueHandler.dialogueUI.rootVisualElement.style.display = DisplayStyle.Flex;
                //Debug.Log("dialogue case");
                dialogueCoroutine = StartCoroutine(dialogueHandler.ChangeDialogue(CloseInteractObjects[0].GetComponent<InteractableObject>().dialogue,interactObject));
                playerInput.actions.FindActionMap("Interacting").Disable();
                break;

            case "Pickup":
                //Debug.Log("pickup case");
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

        Vector3 playerDirection = (transform.forward * movementVector.z) + new Vector3(0,movementVector.y,0); 
        //transform.Rotate(Vector3.up * movementVector.x * (rotationSpeed));
        transform.rotation = Quaternion.Euler(0,transform.eulerAngles.y + (movementVector.x * rotationSpeed * Time.deltaTime),0);

        playerVelocity = playerDirection * walkSpeed * Time.fixedDeltaTime;
        playerBody.AddForce(playerVelocity, ForceMode.VelocityChange);

        
    }
}
