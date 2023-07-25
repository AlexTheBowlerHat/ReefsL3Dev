using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialoguePrompt : MonoBehaviour
{
    public string[] dialogue;
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
        Debug.Log("Dialogue wahoo");
    }
}
