using Mono.Cecil;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerDetection : MonoBehaviour
{
    Camera sceneCamera;
    public GameObject player;
    Transform playerTransform;
    Collider playerCollider;
    

    public Transform oceanPlaneTransform;
    public GameObject globalVolume;

    public Component[] underwaterFilters;

    // Start is called before the first frame update
    void Start()
    {
        sceneCamera = Camera.main;
        playerCollider = player.GetComponent<Collider>();
        playerTransform = player.transform;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter (Collider otherCollider)
    {
        if (!otherCollider == playerCollider ) return;
        Vector3 cameraWorldSpacePosition = sceneCamera.ScreenToWorldPoint(new Vector3(Screen.width/2, Screen.height/2, sceneCamera.transform.position.z));


        Debug.Log("oog");
    }
}
