using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectInitial : MonoBehaviour
{
    public float speed = 1f;
    public GameObject meteorParticles;
    
    private Transform meteorTransform;

    void Start()
    {
        float randRotation = Random.Range(0.0f, 360.0f);

        meteorTransform = GetComponentInChildren<Transform>();
        meteorTransform.localEulerAngles = new Vector3(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, Random.Range(0.0f, 360.0f));
                // mPart.transform.parent = gameObject.transform;
        // mPart.transform.localPosition = new Vector3(0.3f, 0f, 0f);
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector3(0, -speed, 0) * Time.deltaTime, Space.World);
    }
}
