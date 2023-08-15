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
    UnityEngine.UIElements.Label dialogueLabel;
    Button dialogueButton;
    [SerializeField] 
    List<string> splitDialogue;
    bool addingText = false;

    bool continueTextAlong = false;
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
    void ContinueText()
    {
        continueTextAlong = true;
        dialogueLabel.style.opacity = 100;
        dialogueButton.SetEnabled(false);
        dialogueButton.visible = false;
    }
    IEnumerator TextTypingEffect(string dialogue)
    {
        addingText = true;

        Debug.Log("Dialogue is: " + dialogue);
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
    public IEnumerator ChangeDialogue(List<string> dialogueList)
    {
        dialogueLabel.visible = true;
        //dialogueSplitter(dialogueList);

        foreach (var line in dialogueList) 
        {
            //Delay so that the next line isnt called until the first one is done
            while (addingText)
            {
                yield return new WaitForSeconds(0.2f);
            }
            StartCoroutine(TextTypingEffect(line));
        }
        dialogueLabel.visible = false;
        yield break;

    }
    /*
    void dialogueSplitter(string dialogueToBeSplit)
    {
        string[] sentenceSplitDialogue = Regex.Split(dialogueToBeSplit, @"(?<=[.])");

        foreach (string sentence in sentenceSplitDialogue)
        {
            Debug.Log("Sentence in splitter is: " + sentence);
            splitDialogue.Add(sentence);
        }

    }
    */
    /*
    //I counted, the rough length of a line in the text box is 135
    void LongStringSplitter(string sentence)
    {
       

        foreach (string stringsplitLongSentence in splitLongSentenceArray)
        {
            Debug.Log("Long string bit split is: " + stringsplitLongSentence);
            splitDialogue.Add(stringsplitLongSentence);
        }
    }
    */
}
