using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalMovement : MonoBehaviour
{
    public float minSpeed = 0.8f;
    public float maxSpeed = 2;
    
    private float size;
    private float speed;
    private float minRotationSpeed = -1f;
    private float maxRotationSpeed = 1f;
    private float rotationSpeed;

    void Start()
    {
        transform.localEulerAngles = new Vector3(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, Random.Range(0.0f, 360.0f));
        size = transform.localScale.x;
        speed = maxSpeed - ((maxSpeed - minSpeed) * (size - 0.1f)) / (0.6f - 0.1f);
        rotationSpeed = maxRotationSpeed - ((maxRotationSpeed - minRotationSpeed) * (size - 0.1f)) / (0.6f - 0.1f);
    }

    void Update()
    {
        transform.Translate(new Vector3(0, -speed, 0) * Time.deltaTime, Space.World);
    }
}
