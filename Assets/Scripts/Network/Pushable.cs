using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class Pushable : MonoBehaviourPunCallbacks
{
    [PunRPC]
    public void bePushedByforce(Vector3 force){
        if (photonView.IsMine == false && PhotonNetwork.IsConnected == true)
        {
            return;
        }
        this.GetComponent<Rigidbody2D>().AddForce(force,ForceMode2D.Impulse);
    }
}
