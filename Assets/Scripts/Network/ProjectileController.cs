using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class ProjectileController : MonoBehaviourPunCallbacks
{
    public float projectileSpeed = 1000.0f;
    public GameObject parentObject = null;
    public Vector2 tg;
    private bool alreadyMove = false;

    void Start(){
        
    }
    void Update(){
        moveToTarget(tg);
    }

    public void moveToTarget(Vector2 tg){
        if(alreadyMove == true){
            return;
        }
        Debug.Log("projectile move: "+tg);
        if(parentObject != null){
            Vector2 parentVelocity = parentObject.GetComponent<Rigidbody2D>().velocity;
            this.GetComponent<Rigidbody2D>().velocity += parentVelocity;            
        }
        this.GetComponent<Rigidbody2D>().velocity += projectileSpeed*tg;
        alreadyMove = true;
    }
}
