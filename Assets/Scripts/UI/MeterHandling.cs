using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;

public class MeterHandling : MonoBehaviour
{
    [SerializeField]
    public UIDocument defaultGUIDocument;
    VisualElement rootUIElement;
    GameObject playerGameObject;
    PlayerLogic playerLogic;
    WaterPlayerDetection waterPlayerDetection;
    PlayerInput playerInput;
    GameObject seaFloor;
    VisualElement depthBar;
    Label depthNumber;
    VisualElement oxygenBar;
    float oxygenDecayRate = 1f;
    float oxygenGainRate = 10f;
    float lastTime = 0f;
    float timeSinceLastCheck = 0f;
    float respawnTime = 5f;
    // Start is called before the first frame update
    void Start()
    {
        rootUIElement = defaultGUIDocument.rootVisualElement;
        playerGameObject = GameObject.FindGameObjectWithTag("Player");
        playerLogic = playerGameObject.GetComponent<PlayerLogic>();
        waterPlayerDetection = GameObject.Find("Ocean Plane").GetComponent<WaterPlayerDetection>();

        playerInput = playerGameObject.GetComponent<PlayerInput>();
        seaFloor = GameObject.Find("SeaFloor");
        depthBar = rootUIElement.Q<VisualElement>("FillBar");
        depthNumber = rootUIElement.Q<Label>("DepthAmount");
        oxygenBar = rootUIElement.Q<VisualElement>("OxygenBar");
        oxygenBar.style.height = Length.Percent(50);
    }

    void DepthBarChange()
    {
        depthNumber.text = Mathf.Round(playerGameObject.transform.position.y).ToString() + "m";
        //Sets bar height when player is above water to full
        if(Mathf.Sign(playerGameObject.transform.position.y) == 1) 
        {
            depthBar.style.height = Length.Percent(50);
            return;
        }
        float depthPercentage =  50 - (Mathf.Abs(playerGameObject.transform.position.y) / Mathf.Abs(seaFloor.transform.position.y) * 50) ; 
        depthBar.style.height = Length.Percent(Mathf.Clamp(depthPercentage,0f,50f)); 
        //Debug.Log("Depth is: " + Length.Percent(Mathf.Clamp(depthPercentage,0f,50f)));
    }

    void OxygenGain()
    {
        if (oxygenBar.style.height == Length.Percent(50)) {return;}
        timeSinceLastCheck = Time.time - lastTime;
        if (timeSinceLastCheck < 1/oxygenGainRate) {return;}
        lastTime = Time.time;
        float newOxygenNumber = oxygenBar.style.height.value.value + 1f;
        oxygenBar.style.height = Length.Percent(newOxygenNumber);
        //Debug.Log("new oxygen gain number is : " + newOxygenNumber);
    }
    void OxygenDecay()
    {
        if (oxygenBar.style.height == Length.Percent(0)) 
        {
            StartCoroutine(PlayerReset());
            return;
        }
        timeSinceLastCheck = Time.time - lastTime;
        if (timeSinceLastCheck < 1/oxygenDecayRate) {return;}
        lastTime = Time.time;

        float newOxygenNumber = oxygenBar.style.height.value.value - 1f;
        //Debug.Log("new oxygen number is: " + newOxygenNumber);
        oxygenBar.style.height = Length.Percent(newOxygenNumber);
    }
    IEnumerator PlayerReset()
    {
        VisualElement background = rootUIElement.Q<VisualElement>("Background");
        background.style.backgroundColor = Color.black;

        waterPlayerDetection.PlayerOutOfWater();
        playerInput.actions.FindActionMap("Player").Disable();
        playerGameObject.transform.position = GameObject.Find("SpawnPoint").transform.position;
        oxygenBar.style.height = Length.Percent(50);

        oxygenBar.style.display = DisplayStyle.None;
        depthBar.style.display = DisplayStyle.None;

        yield return new WaitForSeconds(respawnTime);
        background.style.backgroundColor = new Color(0,0,0,0);
        playerInput.actions.FindActionMap("Player").Enable();
        playerInput.actions.FindActionMap("Interacting").Enable();
        oxygenBar.style.display = DisplayStyle.Flex;
        depthBar.style.display = DisplayStyle.Flex;

    }
    void OxygenBarChange()
    {
        if (!playerLogic.isUnderwater)
        {
           OxygenGain();
           return;
        }
        OxygenDecay();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        DepthBarChange();
        OxygenBarChange();
    }
}
