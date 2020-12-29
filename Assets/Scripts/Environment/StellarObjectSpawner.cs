using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StellarObjectSpawner : MonoBehaviour
{
    public Sprite[] sprites;
    public GameObject[] Items;
    public float spawnCooldown;
    public GameObject[] borders;
    public float minSize = 0.15f;
    public float maxSize = 1.75f;

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
        // spawnLocations = new List<float> {-16f, -14f, -12f, -10f, -8f, -6f, -4f, -2f, 0f, 2f, 4f, 6f, 8f, 10f, 12f, 14f, 16f};
        spawnLocations = new List<float> {-6.5f, -5f, -3.5f, -2f, -0.5f};
    }

    void Update()
    {
        if(Time.time >= nextSpawnTime) {
            SpawnRandomItem();
            nextSpawnTime = Time.time + spawnCooldown;
        }
    }

    private void SpawnRandomItem() {
        // TODO: improve spawn to prevent overlapping
        int randIdx = Random.Range(0, sprites.Length);
        float randSize = Random.Range(minSize, maxSize);
        List<float> modList = new List<float>(spawnLocations);
        modList.Remove(lastSpawnLocation);
        
        int randPos = Random.Range(0, modList.Count-1);
        float spawnLocation = modList[randPos];
        
        GameObject newObject = new GameObject();
        newObject.AddComponent(typeof(VerticalMovement));
        Rigidbody2D rb = newObject.AddComponent(typeof(Rigidbody2D)) as Rigidbody2D;
        rb.bodyType = RigidbodyType2D.Kinematic;
        SpriteRenderer sr = newObject.AddComponent(typeof(SpriteRenderer)) as SpriteRenderer;
        CircleCollider2D cc = newObject.AddComponent(typeof(CircleCollider2D)) as CircleCollider2D;
        cc.isTrigger = true;
        sr.sprite = sprites[randIdx];
        newObject.transform.localScale = new Vector3(randSize, randSize, randSize);
        newObject.transform.localPosition = new Vector3(spawnLocation, 7f, -2);
        sr.sortingOrder = 0;

        GameObject randItem = newObject;
        
        Instantiate(randItem, new Vector3(spawnLocation, gameObject.transform.position.y, -2), Quaternion.identity);
        lastSpawnLocation = spawnLocation;
    }
}
