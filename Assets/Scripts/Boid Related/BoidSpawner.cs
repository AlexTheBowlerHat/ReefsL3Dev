using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidSpawner : MonoBehaviour
{
    public float boidSpawnAmount = 0f;
    float boidSpawnDelay = 0.5f;
    public GameObject boidPrefab;
    GameObject boidHome;
    // Start is called before the first frame update
    void Start()
    {
        boidHome = gameObject;
        StartCoroutine(BoidSpawn());
    }

    IEnumerator BoidSpawn()
    {
        for (int i = 0; i < boidSpawnAmount; i++ )
        {
            Instantiate(boidPrefab,gameObject.transform.localPosition,Quaternion.identity);
            //Debug.Log("Boidspawner home is: "+ boidHome.transform.position);

            yield return new WaitForSeconds(boidSpawnDelay);
        }
        yield break;
    }

}
