using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

public class Boids : MonoBehaviour
{
    float boidMass = 0f;
    //Vector 3 boidPosition gameObject.transform.position
    Vector3 boidVelocity = Vector3.zero; 
    float boidMaxForce = 0f;
    float boidMaxSpeed = 0f;
    //Quaternion Orientation quaternion.identity;
    //===============

    Vector3 steeringDirection = Vector3.zero;
    Vector3 steeringForce = Vector3.zero; 

    // Start is called before the first frame update
    void Start()
    {
        boidMass = gameObject.GetComponent<Rigidbody>().mass;
    }

    // Update is called once per frame
    void Update()
    {
        steeringForce = Vector3.ClampMagnitude(steeringForce, boidMaxForce);
        UpdateMovement();
    }

    void UpdateMovement()
    {
        //steeringforce = truncate (steering_direction, max_force)
        //acceleration = steering_force / mass
        //boidVelocity = truncate (velocity + acceleration, max_speed)
        //boidPosition = position + velocity
    }
}
