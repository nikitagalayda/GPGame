using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDestroyer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // void OnCollisionEnter2D(Collision2D col)
    // {
    //     // if(col.gameObject.tag == "EnvironmentObject"){
    //         Debug.Log(col.gameObject.name);
    //         Destroy(col.gameObject);
    //     // }
    // }

    void OnTriggerEnter2D(Collider2D col) {
        Debug.Log(col.gameObject.name);
        Destroy(col.gameObject);
    }
}
