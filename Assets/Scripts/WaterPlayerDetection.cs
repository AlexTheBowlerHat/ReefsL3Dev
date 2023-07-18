using Mono.Cecil;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class WaterPlayerDetection : MonoBehaviour
{
    Camera sceneCamera;
    public GameObject player;
    Transform playerTransform;
    Collider playerCollider;
    Rigidbody playerRigidBody;
    
    public Transform oceanPlaneTransform;
    public Volume waterVolume;
    bool playerTouchingWater = true;
    float loopDelay = 0.1f;

    public PlayerInput playerInput;
    public Movement movement;


    // Start is called before the first frame update
    void Start()
    {
        sceneCamera = Camera.main;
        playerCollider = player.GetComponent<Collider>();
        playerTransform = player.transform;
        playerRigidBody = player.GetComponent<Rigidbody>();
        //I can't find a better way to do this so this will do!      
    }

    IEnumerator PlayerAndPlayerCameraPositionCheck(float playerPositionToCheckAgainst, float camPosToCheckAgainst)
    {
        //Checks player position
        while (playerTransform.position.y > playerPositionToCheckAgainst)
        {
            if (!playerTouchingWater) yield return null;
            if (sceneCamera.transform.position.y < camPosToCheckAgainst || playerTransform.position.y < playerPositionToCheckAgainst) break;
            yield return new WaitForSeconds(loopDelay);
        }
        Debug.Log("oog, both cam and player under");
        //Control change
        /*
        playerInput.actions["Underwater Bindings"].Enable();
        playerInput.actions["Jump"].Disable();
        */
        playerInput.actions.FindActionMap("Player").Disable();
        playerInput.actions.FindActionMap("UnderWater").Enable();

        waterVolume.enabled = true;
        movement.isUnderwater = true;
        playerRigidBody.useGravity = false;
 
        yield break;
        
    }

    private void OnTriggerEnter (Collider otherCollider)
    {
        if (otherCollider != playerCollider || playerTransform.position.y < -1f) return;
        StartCoroutine(PlayerAndPlayerCameraPositionCheck(-1f,0.4f));
        Debug.Log("oog, player enter detected");
    }

    private void OnTriggerExit(Collider otherCollider)
    {
        Debug.Log("out oog detected");
        if (otherCollider != playerCollider || playerTransform.position.y < oceanPlaneTransform.position.y) return;
        Debug.Log("player properly above the water");
        //StartCoroutine(PlayerAndPlayerCameraPositionCheck(-10f, 0f));

        playerTouchingWater = false;
        playerInput.defaultActionMap = "Player";
        /*
        playerInput.actions["Underwater Bindings"].Disable();
        playerInput.actions["Jump"].Enable();
        */
        playerRigidBody.useGravity = true;

        waterVolume.enabled = false;
        
        movement.isUnderwater = false;
    }
}
