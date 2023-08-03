using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

public class InteractableObject : MonoBehaviour
{
    public string[] dialogue;
    public PlayerLogic playerLogic;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger entered");
        if (other.transform.tag != "Player") return;
        playerLogic.CloseInteractObjects.Append(gameObject);
        playerLogic.playerInput.actions.FindActionMap("Interacting").Enable();
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Trigger Exited");
        if (other.transform.tag != "Player") return;
        //remove object from array
        playerLogic.playerInput.actions.FindActionMap("Interacting").Disable();
    }
}
