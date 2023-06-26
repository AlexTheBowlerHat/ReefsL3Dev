using Mono.Cecil;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class WaterPlayerDetection : MonoBehaviour
{
    Camera sceneCamera;
    public GameObject player;
    Transform playerTransform;
    Collider playerCollider;
    
    public Transform oceanPlaneTransform;
    public Volume waterVolume;

    // Start is called before the first frame update
    void Start()
    {
        sceneCamera = Camera.main;
        playerCollider = player.GetComponent<Collider>();
        playerTransform = player.transform;
        //I can't find a better way to do this so this will do!      
    }

    IEnumerator PlayerAndPlayerCameraPositionCheck(float playerPositionToCheckAgainst, float camPosToCheckAgainst)
    {
        while (playerTransform.position.y > playerPositionToCheckAgainst)
        {
            if (sceneCamera.transform.position.y < camPosToCheckAgainst) break;
            yield return new WaitForSeconds(1);
        }
        yield break;
    }

    private void OnTriggerEnter (Collider otherCollider)
    {
        if (otherCollider != playerCollider) return;
        StartCoroutine(PlayerAndPlayerCameraPositionCheck(-10f,0f));
        Debug.Log("oog");
        waterVolume.enabled = true;
    }

    private void OnTriggerExit(Collider otherCollider)
    {
        if (otherCollider != playerCollider) return;
        //StartCoroutine(PlayerAndPlayerCameraPositionCheck(-10f, 0f));
        waterVolume.enabled = false;
    }
}
