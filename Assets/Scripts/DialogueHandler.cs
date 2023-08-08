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
    // Start is called before the first frame update
    void Start()
    {
        var rootUIElement = DialogueUI.rootVisualElement;
        dialogueLabel = rootUIElement.Q<UnityEngine.UIElements.Label>("Dialogue");
    }

    public void ChangeDialogue()
    {

    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
