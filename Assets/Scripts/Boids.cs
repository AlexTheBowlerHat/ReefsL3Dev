using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

public class Boids : MonoBehaviour
{
    Vector3 boidVelocity = Vector3.zero;
    public float boidSpeed = 5f;

    Quaternion boidOrientation;
    //===============
    float boidDectionRadius = 5f;

    Vector3 steeringDirection = Vector3.zero;
    Vector3 steeringForce = Vector3.zero;
    Rigidbody boidBody;
    SphereCollider boidNeighbourCollider;
    List<Vector3> nearBoidPositions;

    //steeringforce = truncate (steering_direction, max_force)
    //acceleration = steering_force / mass
    //float boidMaxForce = 0f;

    // Start is called before the first frame update
    void Start()
    {
        boidBody = gameObject.GetComponent<Rigidbody>();
        //boidMass = boidBody.mass;
        boidOrientation = quaternion.identity;

        boidNeighbourCollider = gameObject.GetComponent<SphereCollider>();
        boidNeighbourCollider.radius = boidDectionRadius;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMovement();

    }

    void UpdateMovement()
    {
        Vector3 seperateVector = Seperate();
        boidVelocity = (transform.forward.normalized + seperateVector.normalized) * boidSpeed;
        gameObject.transform.position += (boidVelocity * Time.deltaTime);


        //boidVelocity = truncate (velocity + acceleration, max_speed)
        //boidPosition = position + velocity
        //steeringForce = Vector3.ClampMagnitude(steeringForce, boidMaxForce);
    }
    Vector3 Seperate()
    {
        Vector3 seperateVector = Vector3.zero;
        foreach (Vector3 neighbourBoidPosition in nearBoidPositions) 
        {
            Vector3 toNeibourBoidVector = neighbourBoidPosition - gameObject.transform.position;
            //something about the magnitude (shorter is better priority)
            //maybe like find the smallest and largest vectors, then the magnitude difference is what its compared to
            //and adding them together before inversing the vector
        }

        return seperateVector;
    }
    void Align()
    {

    }
    void StickTogether()
    {

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
