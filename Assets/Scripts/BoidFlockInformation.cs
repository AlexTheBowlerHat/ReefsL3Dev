using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidFlockInformation : MonoBehaviour
{
    public List<GameObject> nearBoids;
    public Vector3 flockHeading;
    //ALSO NEED A SOMEHOW WAY TO TRACK NEARBY oh wait this can just be on every boid 

    //Need a method in the update function that goes over all the boids, 

    public Vector3 CalcSeperationHeading()
    {
        if (nearBoids.Count <= 0) { return Vector3.zero; }
        Vector3 seperationHeading = Vector3.zero;
        foreach (var boid in nearBoids) 
        {
            seperationHeading += (gameObject.transform.position - boid.transform.position);
        }
        seperationHeading /= nearBoids.Count;

        Debug.Log("__SEPERATION HEADING__ OF " + gameObject.name + " IS: " + seperationHeading);
        return seperationHeading;
    }

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

