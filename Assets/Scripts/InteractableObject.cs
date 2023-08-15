using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

public class InteractableObject : MonoBehaviour
{
    public List<string> dialogue;
    public PlayerLogic playerLogic;
    public string interactableObjectType;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger entered");
        if (other.gameObject != playerLogic.gameObject) return;
        playerLogic.playerInput.actions.FindActionMap("Interacting").Enable();
        if (playerLogic.CloseInteractObjects.Find(x => gameObject)) return;
        playerLogic.CloseInteractObjects.Add(gameObject);
        
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Trigger Exited");
        if (other.gameObject != playerLogic.gameObject) return;
        //remove object from array
        playerLogic.playerInput.actions.FindActionMap("Interacting").Disable();
        if (!playerLogic.CloseInteractObjects.Find(x => gameObject)) return;
        playerLogic.CloseInteractObjects.Remove(gameObject);
    }
}
