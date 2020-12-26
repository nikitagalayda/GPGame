using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public float spawnCooldown;
    public GameObject[] borders;
    public GameObject spawnItem;

    private float spawnMinLocation;
    private float spawnMaxLocation;
    private float nextSpawnTime = 1.0f;
    private List<float> spawnLocations;
    private float lastSpawnLocation = 0f;

    void Start()
    {
        // TODO: add half of border width
        spawnMinLocation = borders[0].transform.position.x;
        spawnMaxLocation = borders[1].transform.position.x;
        spawnLocations = new List<float> {-2f, -1f, 0f, 1f, 2f};
    }

    void Update()
    {
        if(Time.time >= nextSpawnTime) {
            SpawnRandomItem();
            nextSpawnTime = Time.time + spawnCooldown;
        }
    }

    private void SpawnRandomItem() {
        List<float> modList = new List<float>(spawnLocations);
        modList.Remove(lastSpawnLocation);
        
        int randPos = Random.Range(0, modList.Count-1);
        float spawnLocation = modList[randPos];
        
        Instantiate(spawnItem, new Vector3(spawnLocation, gameObject.transform.position.y, 0), Quaternion.identity);
        lastSpawnLocation = spawnLocation;
    }
}
