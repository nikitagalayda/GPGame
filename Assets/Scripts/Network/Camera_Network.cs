using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Network : MonoBehaviour
{

    public Transform player = null;
    public Vector3 offset = new Vector3(0, -20, -8);
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(player != null)
        {
            transform.position = new Vector3(player.position.x + offset.x, player.position.y + offset.y, offset.z);
        }
            
    }

    public void setPlayer(Transform trans)
    {
        player = trans;
    }
}
