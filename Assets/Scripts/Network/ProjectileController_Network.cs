using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class ProjectileController_Network : MonoBehaviourPunCallbacks
{
    public float projectileSpeed = 1000.0f;
    public GameObject parentObject = null;
    public Vector3 initialPos;
    private Vector2 target;
    private bool movingToTarget = false;
    private float liveSecond = 0.7f;

    void Start()
    {
        initialPos = this.transform.position;
    }
    void Awake()
    {
        
    }
    void FixedUpdate()
    {
        if (movingToTarget)
        {
            this.transform.Translate(target * projectileSpeed * Time.deltaTime);
        }
    }

    public void moveToTarget(Vector2 tg)
    {
        projectileSpeed += parentObject.GetComponent<Rigidbody2D>().velocity.magnitude;
        target = tg;
        movingToTarget = true;
    }

    void OnBecameInvisible()
    {
        PhotonNetwork.Destroy(this.gameObject);
    }
    /*
        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.tag == "Item")
            {
                Debug.Log("COLLISION WITH " + other.gameObject.name);
                // PhotonView photonViewOther = PhotonView.Get(other.gameObject);
                // photonViewOther.RPC("TransitionToPosition", RpcTarget.All, parentObject.transform.position);
                PhotonView photonViewOther = PhotonView.Get(other.gameObject);
                // other.gameObject.GetComponent<ObjectController_Network>().TransitionToPosition(parentObject.transform.position);
                photonViewOther.RPC("TransitionToPosition", RpcTarget.All, parentObject.transform.position);
                //parentObject.GetComponent<SimpleTeleport_NetworkVersion>().TransitionToPosition(other.transform.position);

                Destroy(gameObject);
            }
        }
        */
    void OnTriggerEnter2D(Collider2D other) {
        Vector3 bulletPos = this.transform.position;
        if(!(other.gameObject==parentObject))
        {  
            Debug.Log("COLLISION WITH " + other.gameObject.name);
            if(photonView.IsMine && other.gameObject.name == "Drop(Clone)"){
                Debug.Log("-----------------ismine" + other.gameObject.name);
                Vector2 temp = other.transform.position;
                //other.transform.position = parentObject.transform.position;
                PhotonView photonViewOther = PhotonView.Get(other.gameObject);
                photonViewOther.RPC("TransitionToPosition", RpcTarget.All, parentObject.transform.position);
                parentObject.transform.position = temp;
                PhotonNetwork.Destroy(this.gameObject);
                
            
                //parentObject.GetComponent<SimpleTeleport_NetworkVersion>().TransitionToPosition(temp);
            }
            if(photonView.IsMine && other.gameObject.name == "Player_Network(Clone)"){
                Debug.Log("-----------------ismine" + other.gameObject.name);
                Vector3 temp = other.transform.position;
                PhotonView photonViewOther = PhotonView.Get(other.gameObject);
                //other.transform.position = parentObject.transform.position;
                //other.gameObject.GetComponent<SimpleTeleport_NetworkVersion>().TransitionToPosition(parentObject.transform.position);
                other.gameObject.GetComponent<SimpleTeleport_NetworkVersion>().TransitionToPosition(parentObject.transform.position);
                photonViewOther.RPC("TransitionToPosition", RpcTarget.All, parentObject.transform.position);
                //PhotonView photonViewParent = PhotonView.Get(parentObject.gameObject);
                //photonViewParent.RPC("TransitionToPosition", RpcTarget.All, temp);
                ///parentObject.transform.position = bulletPos;
                PhotonView photonViewParent = PhotonView.Get(parentObject.gameObject);
                photonViewParent.RPC("TransitionToPosition3", RpcTarget.All,bulletPos);
                //other.transform.position = temp;
                
                
                
                //other.transform.position = parentObject.transform.position+new Vector3(0,5,0);
                
                
                PhotonNetwork.Destroy(this.gameObject);
                //other.gameObject.GetComponent<SimpleTeleport_NetworkVersion>().TransitionToPosition(parentObject.transform.position);
            
                //parentObject.GetComponent<SimpleTeleport_NetworkVersion>().TransitionToPosition(temp);
            }
        }  
        //if(photonView.IsMine)
            
       // if( other.gameObject.name == "Drop(Clone)" ||  other.gameObject.name == "Player_Network(Clone)")
            
    }
   
}
