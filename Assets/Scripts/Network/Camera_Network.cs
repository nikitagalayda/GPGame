using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Network : MonoBehaviour
{

    public Transform player = null;
    public Transform player2 = null;
    public Vector2 offset = new Vector2(0, 0 );
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float totalx = 0;
        float totaly = 0;
        float num = 0;
        if(player != null)
        {
            totalx += player.position.x;
            totaly += player.position.y;
            num++;
            
        }
        if (player2 != null)
        {
            totalx += player2.position.x;
            totaly += player2.position.y;
            num++;
    
           
        }
        if(num>0)
            transform.position = new Vector3(totalx/num, totaly/num, -8);

    }

    public void setPlayer(Transform trans)
    {
        if (player == null)
            player = trans;
        else
            setPlayer2(trans);
    }
    public void setPlayer2(Transform trans)
    {
        player2 = trans;
    }
}
