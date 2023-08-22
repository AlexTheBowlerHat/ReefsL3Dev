using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

public class Boids : MonoBehaviour
{
    Vector3 steeringDirection = Vector3.zero;
    float boidSpeed;
    float minBoidSpeed = 2f;
    public float maxBoidSpeed = 5f;
    //Velocity is speed with direction
    Vector3 boidVelocity = Vector3.zero;

    Vector3 boidAcceleration = Vector3.zero;
    float maxBoidAcceleration = 3f;

    float seperateWeight = 1f;
    float alignWeight = 1f;
    float cohesionWeight = 1f;

    Quaternion boidOrientation;
    //===============
    float boidDectionRadius = 5f;

    Vector3 steeringForce = Vector3.zero;
    Rigidbody boidBody;
    SphereCollider boidNeighbourCollider;
    List<Vector3> nearBoidPositions;

    private void Awake()
    {
        InitiliseValues();
    }

    void InitiliseValues()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        boidBody = gameObject.GetComponent<Rigidbody>();
        boidOrientation = quaternion.identity;

        boidNeighbourCollider = gameObject.GetComponent<SphereCollider>();
        boidNeighbourCollider.radius = boidDectionRadius;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateBoidMovement();

    }
    void UpdateBoidMovement()
    {
        if (nearBoidPositions.Capacity > 0) 
        {
            //ALL PLACEHOLDERS
            Vector3 seperateVector = SteerTowards(vector: Vector3.zero) * seperateWeight;
            Vector3 alignVector =  SteerTowards(vector: Vector3.zero) * alignWeight; 
            Vector3 cohesionVector = SteerTowards(vector: Vector3.zero) * cohesionWeight;
        }

        foreach (Vector3 neighbourBoidPosition in nearBoidPositions)
        {
            Vector3 toNeibourBoidVector = neighbourBoidPosition - gameObject.transform.position;
            //something about the magnitude (shorter is better priority)
            //maybe like find the smallest and largest vectors, then the magnitude difference is what its compared to
            //and adding them together before inversing the vector
        }

        boidSpeed = boidVelocity.magnitude;
        boidVelocity = (steeringDirection) * boidSpeed;
        gameObject.transform.position += (boidVelocity * Time.deltaTime);

        //boidVelocity = truncate (velocity + acceleration, max_speed)
        //boidPosition = position + velocity
        //steeringForce = Vector3.ClampMagnitude(steeringForce, boidMaxForce);
    }


    //Taken from https://github.com/SebLague/Boids/blob/master/Assets/Scripts/Boid.cs 
    Vector3 SteerTowards(Vector3 vector)
    {
        Vector3 v = vector.normalized * maxBoidSpeed - boidVelocity;
        return Vector3.ClampMagnitude(v, maxBoidAcceleration);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Boid") { return; }
        nearBoidPositions.Add(other.transform.position);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag != "Boid") { return; }
        nearBoidPositions.Remove(other.transform.position);
    }

}
