using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidDetection : MonoBehaviour
{
    [SerializeField]
    List<GameObject> nearBoids;
    // Start is called before the first frame update
    void Start()
    {
        nearBoids = transform.parent.GetComponent<BoidFlockInformation>().nearBoids;
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
