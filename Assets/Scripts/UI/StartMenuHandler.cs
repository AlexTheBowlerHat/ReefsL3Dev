using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Cinemachine;

public class StartMenuHandler : MonoBehaviour
{
    [SerializeField]
    UIDocument startMenuDocument;
    VisualElement rootUIElement;
    PlayerLogic playerLogic;
    CinemachineFreeLook freeLookCamera;
    
    public Vector3 startCamPosition;
    public Vector3 startCamRotation;

    // Start is called before the first frame update
    void Start()
    {
        rootUIElement = startMenuDocument.rootVisualElement;
        playerLogic = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerLogic>();
        freeLookCamera = playerLogic.gameObject.GetComponent<PlayerCameraControl>().freeLookCamera;
        freeLookCamera.enabled = false;

        Camera.main.transform.position = startCamPosition;
        Camera.main.transform.eulerAngles = startCamRotation;
    }

}
