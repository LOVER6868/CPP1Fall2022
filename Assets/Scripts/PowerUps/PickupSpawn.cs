using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupSpawn : MonoBehaviour
{
    public GameObject[] objectsToSpawn;

    // Start is called before the first frame update
    void Start()
    {
            int pickupType = Random.Range(0, objectsToSpawn.Length);

            Instantiate(objectsToSpawn[pickupType], this.transform.position, this.transform.rotation);
    }
}
