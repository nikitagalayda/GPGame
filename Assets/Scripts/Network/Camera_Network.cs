using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;
public class Camera_Network : MonoBehaviourPunCallbacks
{
    
    public Transform player = null;
    public Vector2 offset = new Vector2(0, 0 );
    public GameObject myPlayer;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if (photonView.IsMine == false && PhotonNetwork.IsConnected == true)
        {
            return;
        }

        foreach (GameObject item in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (item.GetComponents<PhotonView>()[0].IsMine == false && PhotonNetwork.IsConnected == true)
            {
                return;
            }
            transform.position = new Vector3(item.transform.position.x, item.transform.position.y, -8);
        }


        /*
        if(player != null)
        {
            // transform.position = new Vector2(player.position.x + offset.x, player.position.y + offset.y);
            transform.position = new Vector3(player.position.x, player.position.y, -8);
        }
        */
        /*
        if (photonView.IsMine == true)
        {
            transform.position = new Vector3(myPlayer.transform.position.x, myPlayer.transform.position.y, -8);
        }
        */
            
    }


    public void setPlayer(Transform trans)
    {
        player = trans;
    }
}
