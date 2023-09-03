using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using Cinemachine;

public class StartMenuHandler : MonoBehaviour
{
    [SerializeField]
    UIDocument startMenuDocument;
    MeterHandling meterHandlingScript;
    VisualElement rootUIElement;
    PlayerLogic playerLogic;
    PlayerInput playerInput;
    CinemachineFreeLook freeLookCamera;
    
    public Vector3 startCamPosition;
    public Vector3 startCamRotation;
    Button playButton;
    Button creditsButton;
    Button quitButton;
    Label creditsLabel;

    // Start is called before the first frame update
    void Start()
    {
        //Turning off other UI + Controls
        rootUIElement = startMenuDocument.rootVisualElement;
        meterHandlingScript = gameObject.transform.parent.Find("Default").GetComponent<MeterHandling>();
        meterHandlingScript.defaultGUIDocument.rootVisualElement.style.display = DisplayStyle.None;
        
        playerLogic = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerLogic>();
        playerInput = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInput>();
        playerInput.actions.FindActionMap("Player").Disable();

        //Changing Camera to start location
        Debug.Log("Got to camera stuff");
        freeLookCamera = playerLogic.gameObject.GetComponent<PlayerCameraControl>().freeLookCamera;
        freeLookCamera.enabled = false;
        Camera.main.transform.position = startCamPosition;
        Camera.main.transform.eulerAngles = startCamRotation;

        //Assigning variables
        creditsLabel = rootUIElement.Q<Label>("Credits");
        creditsLabel.style.display = DisplayStyle.None;
        playButton = rootUIElement.Q<Button>("Play");
        creditsButton = rootUIElement.Q<Button>("Credits");
        quitButton = rootUIElement.Q<Button>("Quit");

        playButton.RegisterCallback<ClickEvent>(ev => StartGame());
        creditsButton.RegisterCallback<ClickEvent>(ev => ShowCredits());
        quitButton.RegisterCallback<ClickEvent>(ev => Quit());
    }

    void StartGame()
    {
        rootUIElement.style.display = DisplayStyle.None;
        playButton.SetEnabled(false);
        creditsButton.SetEnabled(false);
        quitButton.SetEnabled(false);

        playButton.UnregisterCallback<ClickEvent>(ev => StartGame());
        creditsButton.UnregisterCallback<ClickEvent>(ev => ShowCredits());
        quitButton.UnregisterCallback<ClickEvent>(ev => Quit());

        meterHandlingScript.defaultGUIDocument.rootVisualElement.style.display = DisplayStyle.Flex;
        freeLookCamera.enabled = true;
        playerInput.actions.FindActionMap("Player").Enable();
    }
    void ShowCredits()
    {
        creditsLabel.style.display = DisplayStyle.Flex;
    }
    
    void Quit()
    {
        Application.Quit();
    }
}
