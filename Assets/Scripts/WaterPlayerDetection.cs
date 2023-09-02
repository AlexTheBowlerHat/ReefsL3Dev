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
    float loopDelay = 0.05f;

    public PlayerInput playerInput;
    public PlayerLogic playerLogic;

    [SerializeField] float playerPositionToCheckAgainst = 0f;
    [SerializeField] float camPosToCheckAgainst = 0.1f;

    [SerializeField]
    GameObject playerCentre;
    AudioSource backgroundMusicReference;
    AudioLowPassFilter lowPassFilter;
    float lowPassCutOffFrequency = 1000;

    // Start is called before the first frame update
    void Start()
    {
        backgroundMusicReference = GameObject.FindGameObjectWithTag("SoundRelated").GetComponent<AudioSource>();
        lowPassFilter = backgroundMusicReference.gameObject.GetComponent<AudioLowPassFilter>();
        lowPassFilter.enabled = false;
        lowPassFilter.cutoffFrequency = lowPassCutOffFrequency;

        sceneCamera = Camera.main;
        playerCollider = player.GetComponent<Collider>();
        playerTransform = player.transform;
        playerCentre = GameObject.Find("Lookat").gameObject;
        playerRigidBody = player.GetComponent<Rigidbody>();
        //I can't find a better way to do this so this will do!      
    }
    void FixedUpdate()
    {
        WaterVolumeSwitcher();
    }
    void WaterVolumeSwitcher()
    {
        if (sceneCamera.transform.position.y > camPosToCheckAgainst) 
        {
            waterVolume.enabled = false;
            lowPassFilter.enabled = false;
        }
        else 
        {
            waterVolume.enabled = true;
            lowPassFilter.enabled = true;
        }
    }

    IEnumerator PlayerAndPlayerCameraPositionCheck(float playerPositionToCheckAgainst)
    {
        //Checks player position
        while (playerCentre.transform.position.y > playerPositionToCheckAgainst)
        {
            if (!playerTouchingWater) yield return null;
            if ( playerCentre.transform.position.y < playerPositionToCheckAgainst) break;
            yield return new WaitForSeconds(loopDelay);
        }
        //Debug.Log("oog, cam and player under");
        //Control change
        PlayerInWater();

        yield break;
        
    }
    void PlayerInWater()
    {
        playerInput.actions.FindActionMap("Player").Disable();
        playerInput.actions.FindActionMap("UnderWater").Enable();
        playerLogic.isUnderwater = true;
        playerRigidBody.velocity = Vector3.zero;
        playerRigidBody.useGravity = false;
        
    }
    public void PlayerOutOfWater()
    {
        playerTouchingWater = false;
        playerInput.actions.FindActionMap("Player").Enable();
        playerInput.actions.FindActionMap("UnderWater").Disable();

        playerRigidBody.useGravity = true;
        waterVolume.enabled = false;
        playerLogic.isUnderwater = false;
    }
    private void OnTriggerEnter (Collider otherCollider)
    {
        if (otherCollider != playerCollider || playerCentre.transform.position.y < playerPositionToCheckAgainst) return;
        StartCoroutine(PlayerAndPlayerCameraPositionCheck(playerPositionToCheckAgainst));
        //Debug.Log("oog, player enter detected");
    }

    private void OnTriggerExit(Collider otherCollider)
    {
        //Debug.Log("out oog detected");
        if (otherCollider != playerCollider || playerCentre.transform.position.y < oceanPlaneTransform.position.y) return;
        //Debug.Log("player properly above the water");
        //StartCoroutine(PlayerAndPlayerCameraPositionCheck(-10f, 0f));
        PlayerOutOfWater();
    }
}
