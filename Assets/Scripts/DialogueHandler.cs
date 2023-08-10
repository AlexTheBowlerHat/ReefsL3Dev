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
    [SerializeField] 
    List<string> splitDialogue;
    bool addingText = false;
    // Start is called before the first frame update
    void Start()
    {
        var rootUIElement = DialogueUI.rootVisualElement;
        dialogueLabel = rootUIElement.Q<UnityEngine.UIElements.Label>("Dialogue");
        dialogueLabel.visible = false;
        dialogueLabel.text = "";
    }

    IEnumerator TextTypingEffect(string dialogue)
    {
        while (addingText)
        {
            yield return new WaitForSeconds(0.1f);
        }
        addingText = true;
        Debug.Log("Dialogue is: " + dialogue);
        foreach (char character in dialogue)
        {
            dialogueLabel.text += character;
            //Debug.Log("typing effect with char " + character);
            yield return new WaitForSeconds(0.1f);
        }
        //TODO ADD PAUSE HERE WHERE YOU CLICK TO CONTINUE
        dialogueLabel.text = "";
        addingText = false;
        yield break;
    }
    public IEnumerator ChangeDialogue(string dialogueList)
    {
        dialogueLabel.visible = true;
        dialogueSplitter(dialogueList);

        foreach (var line in splitDialogue) 
        {
            Debug.Log("change dialogue foreach loop");
            StartCoroutine(TextTypingEffect(line));
            yield return new WaitForSeconds(0.2f);
        }
        yield break;

    }
    void dialogueSplitter(string dialogueToBeSplit)
    {
        string[] sentenceSplitDialogue = Regex.Split(dialogueToBeSplit, @"(?<=[.])");

        foreach (string sentence in sentenceSplitDialogue)
        {
            splitDialogue.Add(sentence);
            LongStringSplitter(sentence);
        }

    }
    //I counted, the rough length of a line in the text box is 135
    void LongStringSplitter(string sentence)
    {
        if (sentence.Length <= 135) return;
        string[] splitLongSentenceArray = sentence.Split(sentence[134]);
        foreach (string stringsplitLongSentence in splitLongSentenceArray)
        {
            splitDialogue.Add(stringsplitLongSentence);
        }
    }
}
