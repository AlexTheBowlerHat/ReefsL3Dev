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
    public bool isUnderwater;
    public int walkSpeed;
    public int jumpForce;
    public LayerMask playerLayerMask;

    // Start is called before the first frame update
    void Start()
    {
        playerBody = gameObject.GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
        playerCollider = GetComponent<Collider>();
    }

    void FixedUpdate()
    {
        //playerBody.AddForce(Physics.gravity,ForceMode.Acceleration);
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
                Vector2 contextMovementVector2 = movementContextInformation.ReadValue<Vector2>();
                movementVector = new Vector3(contextMovementVector2.x,0,contextMovementVector2.y);

                //Debug.Log("Movement vector is: " + movementVector.ToString());
                MovePlayer();
                break;

            case "Jump":
                if (!movementContextInformation.performed) return;
                //bool groundCheck = Physics.Raycast(transform.position, -transform.up, playerCollider.bounds.extents.y + 0.1f);

                RaycastHit boxCastInfo;
                float varMaxDist = 100;
                Vector3 halfExtentsValues = new Vector3(playerCollider.bounds.extents.x / 2, 0.1f, playerCollider.bounds.extents.z / 2);

                bool groundCheck = Physics.BoxCast(center: new Vector3(playerCollider.transform.position.x, 
                    playerCollider.bounds.max.y + 1f, 
                    playerCollider.transform.position.z),
                    halfExtents: halfExtentsValues,
                    direction: -Vector3.up, 
                    hitInfo: out boxCastInfo, 
                    orientation: quaternion.identity, 
                    maxDistance: varMaxDist,
                    layerMask: playerLayerMask, 
                    queryTriggerInteraction: QueryTriggerInteraction.Ignore);

                Debug.Log("Y Extents Is: " + playerCollider.bounds.extents.y + 0.1f.ToString());
                Debug.Log("Centre Position Is ->" + playerCollider.transform.position.x.ToString() +
                    ", " + playerCollider.bounds.min.y.ToString() +
                    ", " + playerCollider.transform.position.z.ToString());
                Debug.Log("Half Extents are ->" + halfExtentsValues);
                Debug.Log("boxCast Collider is -> " + boxCastInfo.collider);
                Debug.Log("boxCast Distance is ->" + boxCastInfo.distance);
                Debug.Log("boxCast Point is ->" + boxCastInfo.point);
                Debug.Log("boxCast mask is -> " );
                //Debug.Log("boxCast rotation is -> "+ quaternion.identity);
                Debug.Log("boxCast maxDistance is -> " + varMaxDist);

                //======================================================================
                /*Debug.Log("player bound min y is -> " + playerCollider.bounds.min.y + 
                    "// x half extents are half this -> " + playerCollider.bounds.extents.x.ToString() +
                    " // y half extents are ->" + playerCollider.bounds.extents.y.ToString() 
                    + " // z half extents are -> " + playerCollider.bounds.extents.z.ToString());
                    */
                Debug.Log("Player is on the ground? :" + groundCheck.ToString());

                if (!groundCheck) return;
                playerBody.AddForce(transform.up * jumpForce, ForceMode.VelocityChange);
                break;

            case "Swimming": 
                if (!isUnderwater) return;
                //Debug.Log("underwater");
                Vector3 contextMovementVector3 = movementContextInformation.ReadValue<Vector3>();
                movementVector = contextMovementVector3;
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
        //Debug.Log(movementVector);
        playerDirection = movementVector * walkSpeed * Time.fixedDeltaTime;
        playerBody.AddForce(playerDirection, ForceMode.VelocityChange);
    }

    //https://www.youtube.com/watch?v=CoTK39SZft8
    //maybe its overlap
    /*
    private void OnDrawGizmos()
    {
        RaycastHit boxCastInfo;
                //OH MY GOD IM A FOOL
                float varMaxDist = 100;

                bool groundCheck = Physics.BoxCast(center: new Vector3(playerCollider.transform.position.x, 
                    playerCollider.bounds.min.y, 
                    playerCollider.transform.position.z),
                    halfExtents: new Vector3(playerCollider.bounds.extents.x / 2, 0.1f , playerCollider.bounds.extents.z / 2),
                    direction: -transform.up, 
                    hitInfo: out boxCastInfo, 
                    orientation: quaternion.identity, 
                    maxDistance: varMaxDist,
                    layerMask: playerLayerMask, 
                    queryTriggerInteraction: QueryTriggerInteraction.Ignore);
        
        if (boxCastInfo.collider)
        {
            Debug.Log("hit something boxcast");
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(center: new Vector3(playerCollider.transform.position.x, 
                    playerCollider.bounds.min.y, 
                    playerCollider.transform.position.z),
                    size: new Vector3(playerCollider.bounds.extents.x, playerCollider.bounds.extents.y, playerCollider.bounds.extents.z)
                    );
        }
        else
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(center: new Vector3(playerCollider.transform.position.x, 
                playerCollider.bounds.min.y, 
                playerCollider.transform.position.z),
                size: new Vector3(playerCollider.bounds.extents.x, playerCollider.bounds.extents.y, playerCollider.bounds.extents.z)
        );
        }
        /*
        Gizmos.DrawCube(new Vector3(playerCollider.transform.position.x,
                    playerCollider.bounds.min.y,
                    playerCollider.transform.position.z),
            new Vector3(playerCollider.bounds.extents.x, playerCollider.bounds.extents.y , playerCollider.bounds.extents.z));\
            
            
    }*/
    
}
