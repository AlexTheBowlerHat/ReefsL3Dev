using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MeterHandling : MonoBehaviour
{
    VisualElement depthBar;
    Label depthNumber;
    [SerializeField]
    UIDocument defaultGUIDocument;
    VisualElement rootUIElement;
    GameObject playerGameObject;
    GameObject seaFloor;

    // Start is called before the first frame update
    void Start()
    {
        rootUIElement = defaultGUIDocument.rootVisualElement;
        playerGameObject = GameObject.FindGameObjectWithTag("Player");
        seaFloor = GameObject.Find("SeaFloor");
        depthBar = rootUIElement.Q<VisualElement>("FillBar");
        depthNumber = rootUIElement.Q<Label>("DepthAmount");
    }

    void DepthBarChange()
    {
        if(Mathf.Sign(playerGameObject.transform.position.y) == 1) 
        {
            depthBar.style.height = Length.Percent(50);
            return;
        }
        float depthPercentage =  50 - (Mathf.Abs(playerGameObject.transform.position.y) / Mathf.Abs(seaFloor.transform.position.y) * 50) ; 
        depthBar.style.height = Length.Percent(Mathf.Clamp(depthPercentage,0f,50f)); 
        //Debug.Log("Depth is: " + Length.Percent(Mathf.Clamp(depthPercentage,0f,50f)));
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        DepthBarChange();
        depthNumber.text = Mathf.Round(playerGameObject.transform.position.y).ToString() + "m";
    }
}
