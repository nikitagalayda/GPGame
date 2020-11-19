using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class ProjectileController_Network : MonoBehaviourPunCallbacks
{
    public float projectileSpeed = 100.0f;
    public GameObject parentObject = null;

    private Vector2 target;
    private bool movingToTarget = false;

    void Start()
    {

    }

    void FixedUpdate()
    {
        if (movingToTarget)
        {
            transform.Translate(target * projectileSpeed * Time.deltaTime);
        }
    }

    public void moveToTarget(Vector2 tg)
    {
        target = tg;
        movingToTarget = true;
    }

    void OnTriggerEnter2D(Collider2D other) {
        Debug.Log("COLLISION WITH " + other.gameObject.name);
        if(photonView.IsMine && other.gameObject.name == "Drop(Clone)"){
            Vector2 temp = other.transform.position;
            other.gameObject.GetComponent<ObjectController_Network>().TransitionToPosition(parentObject.transform.position);
        
            parentObject.GetComponent<SimpleTeleport_NetworkVersion>().TransitionToPosition(temp);
            Destroy(this.gameObject);
        }
        
    }

    // private void OnTriggerEnter2D(Collider2D other) {
    //     Debug.Log("ASDASDJLHASJDHJKAHSD");
    //     Debug.Log("COLLISION WITH " + other.gameObject.name);
    // }
}
