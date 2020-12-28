using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
public class ObjectSpawner : MonoBehaviourPunCallbacks
{
    public float spawnCooldown;
    public GameObject[] borders;
    public GameObject spawnItem;
    public GameObject[] spawnItems;
    public float[] spawnItemsRate;

    private float spawnMinLocation;
    private float spawnMaxLocation;
    private float nextSpawnTime = 1.0f;
    private float nextItemSpawnTime = 1.0f;
    private List<float> spawnLocations;
    private float lastSpawnLocation = 0f;

    void Start()
    {
        // TODO: add half of border width
       // spawnMinLocation = borders[0].transform.position.x;
        //spawnMaxLocation = borders[1].transform.position.x;
        spawnLocations = new List<float> {-2f, -1f, 0f, 1f, 2f};
    }

    void Update()
    {
        if(PhotonNetwork.IsMasterClient){
            if(Time.time >= nextSpawnTime) {
                    SpawnMeteor();
                    nextSpawnTime = Time.time + spawnCooldown;
                }
                if(Time.time >= nextItemSpawnTime) {
                    int itemIdx = Random.Range(0, spawnItems.Length-1);
                    SpawnRandomItem(itemIdx);
                    nextItemSpawnTime = Time.time + spawnCooldown;
                }
            }
        
    }

    private void SpawnMeteor() {
        List<float> modList = new List<float>(spawnLocations);
        modList.Remove(lastSpawnLocation);
        
        int randPos = Random.Range(0, modList.Count-1);
        float spawnLocation = modList[randPos];

        PhotonNetwork.Instantiate(spawnItem.name, new Vector3(spawnLocation, gameObject.transform.position.y, -2), Quaternion.identity, 0);
        //Instantiate(spawnItem, new Vector3(spawnLocation, gameObject.transform.position.y, -2), Quaternion.identity);
        
        lastSpawnLocation = spawnLocation;
    }
    private void SpawnRandomItem(int itemIdx) {
        List<float> modList = new List<float>(spawnLocations);
        modList.Remove(lastSpawnLocation);
        
        int randPos = Random.Range(0, modList.Count-1);
        float spawnLocation = modList[randPos];

        PhotonNetwork.Instantiate(spawnItems[itemIdx].name,  new Vector3(spawnLocation, gameObject.transform.position.y, -2), Quaternion.identity, 0);
        //Instantiate(spawnItem, new Vector3(spawnLocation, gameObject.transform.position.y, -2), Quaternion.identity);
        
        lastSpawnLocation = spawnLocation;
    }
}
