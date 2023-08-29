using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidSettings : MonoBehaviour
{
    //NOTE TO SELF, DO NOT TRUST THESE VALUES, INSPECTOR OVERRIDES THESE
    public float minBoidSpeed = 2f;
    public float maxBoidSpeed = 5f;
    public float maxBoidAcceleration = 3f;

    //__SETTINGS__
    public float seperateWeight = 1f;
    public float alignWeight = 1f;
    public float cohesionWeight = 1f;

    [Range(0f, 10f)]
    public float boidDectionRadius = 5f;
    public float avoidRadius = 1f;

    //
}
