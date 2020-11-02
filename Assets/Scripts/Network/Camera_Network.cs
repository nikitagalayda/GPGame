using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Network : MonoBehaviour
{

    public Transform player = null;
    public Vector2 offset = new Vector2(0, 0 );
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(player != null)
        {
            // transform.position = new Vector2(player.position.x + offset.x, player.position.y + offset.y);
            transform.position = new Vector3(player.position.x, player.position.y, -8);
        }
            
    }

    public void setPlayer(Transform trans)
    {
        player = trans;
    }
}
