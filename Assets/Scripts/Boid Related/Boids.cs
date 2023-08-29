using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

public class Boids : MonoBehaviour
{
    Vector3 steeringDirection = Vector3.zero;
    //Velocity is speed with direction
    [SerializeField]
    Vector3 boidVelocity = Vector3.zero;

    [SerializeField]
    Vector3 boidAcceleration = Vector3.zero;


    Vector3 steeringForce = Vector3.zero;
    Rigidbody boidBody;
    SphereCollider boidNeighbourCollider;
    BoidFlockInformation boidFlockInformation;
    List<GameObject> nearBoids;
    [SerializeField]
    BoidSettings boidSettings;

    // Start is called before the first frame update
    void Start()
    {
        boidSettings = GameObject.Find("BoidSettingsHolder").GetComponent<BoidSettings>();
        boidFlockInformation = gameObject.GetComponent<BoidFlockInformation>();

        nearBoids = boidFlockInformation.nearBoids;
        boidVelocity = transform.forward * boidSettings.minBoidSpeed;
        boidBody = gameObject.GetComponent<Rigidbody>();
        boidNeighbourCollider = gameObject.GetComponent<SphereCollider>();

        //Typecast magic I think https://discussions.unity.com/t/c-changing-the-radius-of-the-sphere-collider/28045
        (boidNeighbourCollider as SphereCollider).radius = boidSettings.boidDectionRadius;
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateBoidMovement();
        boidFlockInformation = gameObject.GetComponent<BoidFlockInformation>();
    }
    void UpdateBoidMovement()
    {
        boidAcceleration = Vector3.zero;
        Debug.Log("before boid near check");

        //__Flock rule calculations__
        if (nearBoids.Capacity > 0)
        {
            //ALL PLACEHOLDERS
            Vector3 seperateVector = SteerTowards(vector: boidFlockInformation.CalcSeperationHeading()) * boidSettings.seperateWeight;
            Vector3 alignVector = SteerTowards(vector: boidFlockInformation.CalcAlignHeading()) * boidSettings.alignWeight;
            Vector3 cohesionVector = SteerTowards(vector: boidFlockInformation.CalcCohesionHeading()) * boidSettings.cohesionWeight;

            boidAcceleration += seperateVector;
            boidAcceleration += alignVector;
            boidAcceleration += cohesionVector;
        }

        Debug.Log("movement");
        //__Movement__  
        float boidSpeed;
        Vector3 direction;
        boidVelocity += boidAcceleration * Time.deltaTime;
        Debug.Log("*VELOCITY IS* " + boidVelocity);

        boidSpeed = boidVelocity.magnitude;
        direction = boidVelocity/boidSpeed; //Normalising the vector
        boidSpeed = Mathf.Clamp(boidSpeed, boidSettings.minBoidSpeed, boidSettings.maxBoidSpeed);

        Debug.Log("*DIRECTION* and *SPEED* are: " + direction + boidSpeed);
        boidVelocity = direction * boidSpeed;
        gameObject.transform.position += (boidVelocity * Time.deltaTime);
        gameObject.transform.forward = direction; //Effectively rotates the boid

        Debug.Log("===============");
    }


    //Taken from https://github.com/SebLague/Boids/blob/master/Assets/Scripts/Boid.cs 
    Vector3 SteerTowards(Vector3 vector)
    {
        Vector3 v = vector.normalized * boidSettings.maxBoidSpeed - boidVelocity;
        return Vector3.ClampMagnitude(v, boidSettings.maxBoidAcceleration);
    }


}
