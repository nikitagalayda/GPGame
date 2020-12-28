using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinFloat : MonoBehaviour
{
    public float amplitude = 1f;

    private float period;

    void Start()
    {
        period = Random.Range(0f, 9999f);    
    }

    void Update()
    {
        gameObject.transform.position += new Vector3(0f, amplitude*Mathf.Sin(Time.time + period), 0f)*Time.deltaTime;
    }
}
