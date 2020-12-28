using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorController : MonoBehaviour
{
    public float speed = 1f;
    public GameObject meteorParticles;

    void Start()
    {
        float randRotation = Random.Range(0.0f, 360.0f);

        transform.localEulerAngles = new Vector3(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, Random.Range(0.0f, 360.0f));
        // transform.Rotate(new Vector3(0, 0, randRotation), relativeTo: Space.self);
        // transform.localEulerAngles = new
        // GameObject mPart = Instantiate(meteorParticles, transform.position, Quaternion.identity) as GameObject;
        // mPart.transform.parent = gameObject.transform;
        // mPart.transform.localRotation = Quaternion.identity;
        // mPart.transform.localPosition = new Vector3(0.3f, 0f, 0f);
    }

    void Update()
    {
        // transform.Translate(new Vector3(0, -speed, 0) * Time.deltaTime, Space.World);
    }
}
