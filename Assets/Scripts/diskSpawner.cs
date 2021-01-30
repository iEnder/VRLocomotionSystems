using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class diskSpawner : MonoBehaviour
{
    public GameObject diskPrefab;

    private GameObject diskInstance;
    private GameObject newDiskInstance;

    public void spawnDisk() {

        if(diskInstance) {
            Destroy(diskInstance);
        }

        newDiskInstance = Instantiate(diskPrefab, transform, false);
        newDiskInstance.transform.position = transform.position;
        
        diskInstance = newDiskInstance;

    }
}
