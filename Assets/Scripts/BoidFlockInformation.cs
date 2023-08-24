using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidFlockInformation : MonoBehaviour
{
    public List<Vector3> nearBoidPositions;
    public Vector3 flockHeading;
    //ALSO NEED A SOMEHOW WAY TO TRACK NEARBY oh wait this can just be on every boid 

    //Need a method in the update function that goes over all the boids, 

    public Vector3 CalcSeperationHeading()
    {
        Vector3 seperationHeading = Vector3.zero;
        foreach (var position in nearBoidPositions) 
        {
            seperationHeading += position;
        }
        seperationHeading /= nearBoidPositions.Count;
        return seperationHeading;
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
