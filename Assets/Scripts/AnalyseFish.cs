using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;

public class AnalyseFish : MonoBehaviour
{
    [SerializeField] float raycastDistance = 0f;
    [SerializeField] float keyPresslength = 0f;
    [SerializeField] int layerMaskLayer = 0;
    GameObject playerGameObject;
    PlayerInput playerInput;

    public UIDocument defaultGUIDocument;
    VisualElement rootUIElement;

    void DisableAnalysis()
    {
        playerInput.actions.FindAction("Analyse").Disable();
        //UI transparency change here
    }
    void RayHitFishCheck()
    {
        RaycastHit analysisInfo;

       //Guard clause ends method early if nothing hit with the ray
       if (!Physics.Raycast(origin: gameObject.transform.position,
            direction: Camera.main.transform.forward,
            hitInfo: out analysisInfo,
            maxDistance: raycastDistance,
            layerMask: layerMaskLayer))
        {
            DisableAnalysis();
            return;
        }

        //Pulls up key indication UI + enable key
        playerInput.actions.FindAction("Analyse").Enable();
        //UI transparency change here

    }

    void Start()
    {
        playerGameObject = GameObject.FindGameObjectWithTag("Player");
        playerInput = playerGameObject.GetComponent<PlayerInput>();
        rootUIElement = defaultGUIDocument.rootVisualElement;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        RayHitFishCheck();
    }
}
