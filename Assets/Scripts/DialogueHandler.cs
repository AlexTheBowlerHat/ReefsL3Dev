using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using UnityEngine;
using UnityEngine.UIElements;

public class DialogueHandler : MonoBehaviour
{
    [SerializeField]
    private UIDocument DialogueUI;
    UnityEngine.UIElements.Label dialogueLabel;
    List<string> splitDialogue;
    // Start is called before the first frame update
    void Start()
    {
        var rootUIElement = DialogueUI.rootVisualElement;
        dialogueLabel = rootUIElement.Q<UnityEngine.UIElements.Label>("Dialogue");
        dialogueLabel.text = "";
    }

    IEnumerator TextTypingEffect(string dialogue)
    {
        foreach (char character in dialogue)
        {
            dialogueLabel.text += character;
            Debug.Log("typing effect with char " + character);
            yield return new WaitForSeconds(0.1f);
        }
        yield break;
    }
    void LongStringSplitter(string sentence)
    {
        if (sentence.Length <= 135) return;
        string[] splitLongSentenceArray = sentence.Split(sentence[134]);
        foreach (string stringsplitLongSentence in splitLongSentenceArray)
        {
            splitDialogue.Add(stringsplitLongSentence);
        }
    }
    List<string> dialogueSplitter(string dialogueToBeSplit)
    {
        string[] sentenceSplitDialogue = dialogueToBeSplit.Split(".");

        foreach (string sentence in sentenceSplitDialogue)
        {
            splitDialogue.Add(sentence);
            LongStringSplitter(sentence);
        }
        return splitDialogue;
    }
    public void ChangeDialogue(string dialogueList)
    {
        List<string> dialogue = dialogueSplitter(dialogueList);

        foreach (var line in dialogue) 
        {
            Debug.Log("change dialogue foreach loop");
            StartCoroutine(TextTypingEffect(line));
        }

    }
}
