using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using UnityEngine;
using UnityEngine.UIElements;
using System.Text.RegularExpressions;

public class DialogueHandler : MonoBehaviour
{
    bool continueTextAlong = false;
    public PlayerLogic playerLogic;
    Coroutine textTypingCoroutine;
    public bool isTalking = false;
    bool addingText = false;
    [Space]
    [Header("UI Elements")]
    public UIDocument dialogueUI;
    public UnityEngine.UIElements.Label dialogueLabel;
    Button dialogueButton;
    UnityEngine.UIElements.Label nameLabel;
    VisualElement rootUIElement;
    GameObject defaultGUIHolder;
    // Start is called before the first frame update
    void Start()
    {
        defaultGUIHolder = GameObject.Find("Default");

        rootUIElement = dialogueUI.rootVisualElement;
        rootUIElement.style.display = DisplayStyle.None;
        dialogueLabel = rootUIElement.Q<UnityEngine.UIElements.Label>("Dialogue");
        dialogueButton = rootUIElement.Q<UnityEngine.UIElements.Button>("ClickToContinue");
        nameLabel = rootUIElement.Q<UnityEngine.UIElements.Label>("Name");

        dialogueButton.SetEnabled(false);
        dialogueButton.visible = false;
        dialogueButton.RegisterCallback<ClickEvent>(ev => ContinueText());

        dialogueLabel.visible = false;
        dialogueLabel.text = "";
        
        nameLabel.visible = false;
        nameLabel.text = "";
    }
    void LockCameraAndControls()
    {
        defaultGUIHolder.GetComponent<UIDocument>().rootVisualElement.style.display = DisplayStyle.None;
        var freeLookCamera = playerLogic.gameObject.GetComponent<PlayerCameraControl>().freeLookCamera;
        freeLookCamera.m_XAxis.m_MaxSpeed = 0;
        freeLookCamera.m_YAxis.m_MaxSpeed = 0;
        freeLookCamera.enabled = false;
        playerLogic.playerInput.actions.FindActionMap("Player").Disable();
        playerLogic.playerInput.actions.FindActionMap("Camera Control").Disable();
    }

    void UnlockCameraAndControls()
    {
        defaultGUIHolder.GetComponent<UIDocument>().rootVisualElement.style.display = DisplayStyle.Flex;
        var camControl = playerLogic.gameObject.GetComponent<PlayerCameraControl>();
        camControl.freeLookCamera.m_XAxis.m_MaxSpeed = camControl.xSpeed;
        camControl.freeLookCamera.m_YAxis.m_MaxSpeed = camControl.ySpeed;
        camControl.freeLookCamera.enabled = true;
        playerLogic.playerInput.actions.FindActionMap("Player").Enable();
        playerLogic.playerInput.actions.FindActionMap("Camera Control").Enable();
    }
    public IEnumerator ChangeDialogue(List<string> dialogueList, GameObject objectInteractedWith)
    {
        LockCameraAndControls();
        InteractableObject interactableObjectScript = objectInteractedWith.GetComponent<InteractableObject>();
        Camera.main.transform.position = interactableObjectScript.cameraPosition;
        Camera.main.transform.eulerAngles = interactableObjectScript.cameraRotation;


        InteractableObject interactObjectScript = objectInteractedWith.GetComponent<InteractableObject>();
        isTalking = true;
        //Debug.Log("Got to change dialogue");
        dialogueLabel.visible = true;

        nameLabel.visible = true;
        nameLabel.text = interactObjectScript.interactObjectName;
        //dialogueSplitter(dialogueList);

        foreach (var line in dialogueList)
        {
            //Debug.Log("Line supplied is: " + line);
            //Delay so that the next line isnt called until the first one is done
            while (addingText)
            {
                yield return new WaitForSeconds(0.2f);
            }
            textTypingCoroutine = StartCoroutine(TextTypingEffect(line));
        }
        while (addingText)
        {
            yield return new WaitForSeconds(0.2f);
        }
        dialogueLabel.visible = false;
        nameLabel.visible = false;
        isTalking = false;
        TextCancel();
        UnlockCameraAndControls();
        yield break;

    }
    IEnumerator TextTypingEffect(string dialogue)
    {
        addingText = true;

        //Debug.Log("Dialogue being displayed is: " + dialogue);
        //Typing effect
        foreach (char character in dialogue)
        {
            dialogueLabel.text += character;
            //Debug.Log("typing effect with char " + character);
            yield return new WaitForSeconds(0.05f);
        }

        //Pause Effect, allows dialogue to be clicked and waits till the player clicks
        dialogueButton.visible = true;
        dialogueButton.SetEnabled(true);
        while (!continueTextAlong)
        {
            yield return new WaitForSeconds(0.01f);
        }

        continueTextAlong = false;
        dialogueLabel.text = "";
        addingText = false;
        yield break;
    }
    void ContinueText()
    {
        continueTextAlong = true;
        dialogueLabel.style.opacity = 100;
        dialogueButton.SetEnabled(false);
        dialogueButton.visible = false;
    }
    public void TextCancel()
    {
        //Resets dialogue UI components
        playerLogic.StopCoroutine(playerLogic.dialogueCoroutine);
        StopCoroutine(textTypingCoroutine);
        rootUIElement.style.display = DisplayStyle.None;
        continueTextAlong = false;
        addingText = false;

        dialogueLabel.visible = false;
        dialogueLabel.text = "";

        nameLabel.visible = false;
        nameLabel.text = "";

        dialogueButton.visible = false;
        dialogueButton.SetEnabled(false);
    }
    
}
