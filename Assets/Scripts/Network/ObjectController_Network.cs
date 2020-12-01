using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;
public class ObjectController_Network : MonoBehaviourPunCallbacks
{
    public float smoothTime = 0.05F;
    public float distanceThreshold = 0.01F;

    private bool inTransition = false;
    private Vector2 velocity = Vector2.zero;
    private Vector2 transitionTarget = Vector2.zero;

    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate() {
        //Vector2 current2DPosition = new Vector2(transform.position.x, transform.position.y);

        /**if(Vector2.Distance(current2DPosition, transitionTarget) <= distanceThreshold) {
            inTransition = false;
        }
        **/
        /**if(inTransition) {
            transform.position = Vector2.SmoothDamp(transform.position, transitionTarget, ref velocity, smoothTime);
            Debug.Log(transitionTarget);
        }
        **/
        //Debug.LogError("IM RUNNING");
        Vector2 current2DPosition = new Vector2(transform.position.x, transform.position.y);

        if (inTransition)
        {
            if (Vector2.Distance(current2DPosition, transitionTarget) <= distanceThreshold)
            {
                Debug.LogError("DONE WITH TRANSITION");
                inTransition = false;
            }
            transform.position = Vector2.SmoothDamp(transform.position, transitionTarget, ref velocity, smoothTime);
            Debug.Log(transitionTarget);
        }
    }



    [PunRPC]
    public void TransitionToPosition(Vector3 targetPosition)
    {

        transform.position = targetPosition;
            //Debug.Log(transitionTarget);
            //inTransition = true;
        
    }
    void OnCollisionEnter2D(Collision2D Collider)
    {
        //print("A:" + Collider.gameObject.name); //印出A:碰撞對象的名字
        if (Collider.gameObject.name == "lava" && photonView.IsMine)
        {
            PhotonNetwork.Destroy(this.gameObject);
        }
            
    }
    /*
    public void TransitionToPosition(Vector3 targetPosition)
    {
        transform.position = targetPosition;

    }
    */
}
