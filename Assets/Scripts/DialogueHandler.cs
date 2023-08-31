using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using UnityEngine;
using UnityEngine.UIElements;
using System.Text.RegularExpressions;

public class DialogueHandler : MonoBehaviour
{
    [SerializeField]
    private UIDocument DialogueUI;
    public UnityEngine.UIElements.Label dialogueLabel;
    Button dialogueButton;
    bool addingText = false;

    bool continueTextAlong = false;
    public PlayerLogic playerLogic;
    Coroutine textTypingCoroutine;
    // Start is called before the first frame update
    void Start()
    {
        var rootUIElement = DialogueUI.rootVisualElement;
        dialogueLabel = rootUIElement.Q<UnityEngine.UIElements.Label>("Dialogue");
        dialogueButton = rootUIElement.Q<UnityEngine.UIElements.Button>("ClickToContinue");

        dialogueButton.SetEnabled(false);
        dialogueButton.visible = false;
        dialogueButton.RegisterCallback<ClickEvent>(ev => ContinueText());
        dialogueLabel.visible = false;
        dialogueLabel.text = "";
    }
    public IEnumerator ChangeDialogue(List<string> dialogueList)
    {
        Debug.Log("Got to change dialogue");
        dialogueLabel.visible = true;
        dialogueLabel.style.opacity = 100;
        //dialogueSplitter(dialogueList);

        foreach (var line in dialogueList)
        {
            Debug.Log("Line supplied is: " + line);
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
        yield break;

    }
    IEnumerator TextTypingEffect(string dialogue)
    {
        addingText = true;

        Debug.Log("Dialogue being displayed is: " + dialogue);
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
        continueTextAlong = false;
        addingText = false;
        dialogueLabel.style.opacity = 0;
        dialogueLabel.visible = false;
        dialogueButton.visible = false;
        dialogueLabel.text = "";
        dialogueButton.SetEnabled(false);
    }
    
}
