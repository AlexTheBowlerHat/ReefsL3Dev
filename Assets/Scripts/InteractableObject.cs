using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public string[] dialogue;
    public Movement movementScript;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag != "Player") return;
        movementScript.CloseDialogueObjects.Append(gameObject);
        movementScript.playerInput.actions.FindActionMap("Interacting").Enable();
    }
}
