using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidFlockInformation : MonoBehaviour
{
    //Offset code + inspiration for various heading implementations from below
    //https://github.com/SebLague/Boids/blob/master/Assets/Scripts/BoidCompute.compute
    public List<GameObject> nearBoids;
    public Vector3 flockHeading;
    BoidSettings boidSettings;
    void Start()
    {
        boidSettings = GameObject.Find("BoidSettingsHolder").GetComponent<BoidSettings>();
    }
    //Seperation
    public Vector3 CalcSeperationHeading()
    {
        if (nearBoids.Count <= 0) { return Vector3.zero; }
        Vector3 seperationHeading = Vector3.zero;
        foreach (var boid in nearBoids) 
        {
            Vector3 offsetToNearBoid = boid.transform.position - gameObject.transform.position;
            float squareDistanceToBoid = (offsetToNearBoid.x * offsetToNearBoid.x) + (offsetToNearBoid.y + offsetToNearBoid.y) + (offsetToNearBoid.z + offsetToNearBoid.z);

            //Check to See If its close enough to seperate from
            if (squareDistanceToBoid > boidSettings.avoidRadius * boidSettings.avoidRadius) {continue;}
            seperationHeading -= (offsetToNearBoid /squareDistanceToBoid);//Normalising the vector
        }
        seperationHeading /= nearBoids.Count; //Averaging

        Debug.Log("__SEPERATION HEADING__ OF " + gameObject.name + " IS: " + seperationHeading);
        return seperationHeading;
    }
    //Alignment
    public Vector3 CalcAlignHeading()
    {
        if (nearBoids.Count <= 0) { return Vector3.zero; }
        Vector3 alignHeading = Vector3.zero;

        foreach (var boid in nearBoids)
        {
            alignHeading += boid.transform.forward;
        }
        alignHeading/= nearBoids.Count;
        Debug.Log("__ALIGN HEADING__ OF " + gameObject.name + " IS: " + alignHeading);
        return alignHeading;
    }
    //Cohesion
    public Vector3 CalcCohesionHeading()
    {
        if (nearBoids.Count <= 0) { return Vector3.zero; }
        Vector3 cohesionHeading = Vector3.zero;

        foreach (var boid in nearBoids)
        {
            cohesionHeading += boid.transform.position;
        }
        cohesionHeading /= nearBoids.Count;
        Debug.Log("__COHESION HEADING__ OF " + gameObject.name + " IS: " + cohesionHeading);
        return cohesionHeading;
    }
    void OnTriggerEnter(Collider other)
    {
        Debug.Log(other + "'s collider entered");
        if (other.tag != "Boid") { return; }
        nearBoids.Add(other.gameObject);
    }

    void OnTriggerExit(Collider other)
    {
        Debug.Log(other + "'s collider exited");
        if (other.tag != "Boid") { return; }
        nearBoids.Remove(other.gameObject);
    }
}

