using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnalyseFish : MonoBehaviour
{
    [SerializeField] float raycastDistance = 0f;
    [SerializeField] float keyPresslength = 0f;

    void RayHitFishCheck()
    {
        Vector3 distanceToFish = Vector3.zero;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        RayHitFishCheck();
    }
}
