using Mono.Cecil;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class WaterPlayerDetection : MonoBehaviour
{
    Camera sceneCamera;
    public GameObject player;
    Transform playerTransform;
    Collider playerCollider;
    

    public Transform oceanPlaneTransform;

    public Volume URPVolume;
    ArrayList effectList;

    private ChannelMixer underwaterChannelMixer;
    private LiftGammaGain underwaterLGG;
    private WhiteBalance underwaterWB;
    private DepthOfField underwaterDepthOfField;
    private FilmGrain underwaterFilmGrain;
    

    // Start is called before the first frame update
    void Start()
    {
        sceneCamera = Camera.main;
        playerCollider = player.GetComponent<Collider>();
        playerTransform = player.transform;

        effectList = new ArrayList
        {
            URPVolume.profile.TryGet<ChannelMixer>(out underwaterChannelMixer),
            URPVolume.profile.TryGet<LiftGammaGain>(out underwaterLGG),
            URPVolume.profile.TryGet<WhiteBalance>(out underwaterWB),
            URPVolume.profile.TryGet<DepthOfField>(out underwaterDepthOfField),
            URPVolume.profile.TryGet<FilmGrain>(out underwaterFilmGrain)
        };



        //I can't find a better way to do this so this will do!      
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void EffectSwitcher()
    {

    }

    private void OnTriggerEnter (Collider otherCollider)
    {
        if (!otherCollider == playerCollider ) return;
        Vector3 cameraWorldSpacePosition = sceneCamera.ScreenToWorldPoint(new Vector3(Screen.width/2, Screen.height/2, sceneCamera.transform.position.z));

        EffectSwitcher();

        Debug.Log("oog");
        underwaterChannelMixer.active = true;
        foreach (var effect in effectList)
        {
            Debug.Log(effect.ToString());
        }
    }
}
