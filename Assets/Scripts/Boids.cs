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
    BoidSettings boidSettings;

    void InitiliseValues()
    {
        boidFlockInformation = gameObject.GetComponent<BoidFlockInformation>();
        nearBoids = boidFlockInformation.nearBoids;
        boidVelocity = transform.forward * boidSettings.minBoidSpeed;
        boidBody = gameObject.GetComponent<Rigidbody>();
        boidSettings = GameObject.Find("BoidSettingsHolder").GetComponent<BoidSettings>();
        boidNeighbourCollider = gameObject.GetComponent<SphereCollider>();
        boidNeighbourCollider.radius = boidSettings.boidDectionRadius;
    }
    // Start is called before the first frame update
    void Start()
    {
        InitiliseValues();

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
            Vector3 alignVector = SteerTowards(vector: Vector3.zero) * boidSettings.alignWeight;
            Vector3 cohesionVector = SteerTowards(vector: Vector3.zero) * boidSettings.cohesionWeight;

            boidAcceleration += seperateVector;
            boidAcceleration += alignVector;
            boidAcceleration += cohesionVector;
        }

        Debug.Log("after boid near check");
        /*
        foreach (Vector3 neighbourBoidPosition in nearBoidPositions)
        {
            Vector3 toNeibourBoidVector = neighbourBoidPosition - gameObject.transform.position;
            //something about the magnitude (shorter is better priority)
            //maybe like find the smallest and largest vectors, then the magnitude difference is what its compared to
            //and adding them together before inversing the vector
        }
        */
        Debug.Log("movement");
        //__Movement__  
        float boidSpeed;
        Vector3 direction;
        boidVelocity += boidAcceleration * Time.deltaTime;
        Debug.Log("*VELOCITY IS* " + boidVelocity);

        boidSpeed = boidVelocity.magnitude;
        direction = boidVelocity/boidSpeed;
        boidSpeed = Mathf.Clamp(boidSpeed, boidSettings.minBoidSpeed, boidSettings.maxBoidSpeed);

        Debug.Log("*DIRECTION* and *SPEED* are: " + direction + boidSpeed);
        boidVelocity = direction * boidSpeed;
        //boidVelocity = (steeringDirection) * boidSpeed;
        gameObject.transform.position += (boidVelocity * Time.deltaTime);

        Debug.Log("===============");
    }


    //Taken from https://github.com/SebLague/Boids/blob/master/Assets/Scripts/Boid.cs 
    Vector3 SteerTowards(Vector3 vector)
    {
        Vector3 v = vector.normalized * boidSettings.maxBoidSpeed - boidVelocity;
        return Vector3.ClampMagnitude(v, boidSettings.maxBoidAcceleration);
    }


}
