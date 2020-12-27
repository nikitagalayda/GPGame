using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;

public class GoDestination : MonoBehaviourPunCallbacks
{
    public GameObject DestObject;
    public float speed = 0.01f;
    // Start is called before the first frame update
    void Awake()
    {
        DestObject = this.gameObject; 
    }

    // Update is called once per frame
    void Update()
    {
        
        // Move our position a step closer to the target.
        float step =  speed * Time.deltaTime; // calculate distance to move
        //Debug.Log("asdasd"+DestObject.name);
        transform.position =  Vector3.Lerp (transform.position,DestObject.transform.position ,step);
    
        //this.transform.position = new Vector3(this.transform.position.x - 1, this.transform.position.y, this.transform.position.z);
        // Check if the position of the cube and sphere are approximately equal.
        //if (Vector3.Distance(transform.position, target.position) < 0.001f)
        //{
            // Swap the position of the cylinder.
        //    target.position *= -1.0f;
        //}
    }
    
    public void SetDest(GameObject target)
    {
        
        DestObject = target;
        Debug.Log("Target 123:"+DestObject.name);
    }

}
