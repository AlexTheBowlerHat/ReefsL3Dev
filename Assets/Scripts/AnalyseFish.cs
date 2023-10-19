using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;

public class AnalyseFish : MonoBehaviour
{
    [SerializeField] float raycastDistance = 1000f;
    //[SerializeField] float keyPresslength = 0f;
    [SerializeField] int layerMaskLayer = 0;
    GameObject playerGameObject;
    PlayerInput playerInput;
    
    public UIDocument defaultGUIDocument;
    VisualElement rootUIElement;
    RaycastHit analysisInfo;

    string[] validAnalysisTargets = { "Clownfish", "Coral" };
    void DisableAnalysis()
    {
        playerInput.actions.FindAction("Analyse").Disable();
        //UI transparency change here
    }
    void RayHitFishCheck()
    {

        Ray mousePointRay = Camera.main.ScreenPointToRay(Input.mousePosition);
       //Guard clause ends method early if nothing hit with the ray
       if (!Physics.Raycast(ray: mousePointRay,
            hitInfo: out analysisInfo,
            maxDistance: raycastDistance,
            layerMask: layerMaskLayer) 
            || !validAnalysisTargets.Contains(analysisInfo.transform.tag))
        {
            DisableAnalysis();
            return;
        }

        //Pulls up key indication UI + enable key
        playerInput.actions.FindAction("Analyse").Enable();
        //UI transparency change here

    }

    public void AnalysisUI()
    {
        Debug.Log("Analysis reach hit " + analysisInfo.transform.tag);
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
