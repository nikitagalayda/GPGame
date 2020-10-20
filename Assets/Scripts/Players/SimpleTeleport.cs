using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;
public class SimpleTeleport : MonoBehaviourPunCallbacks
{
    public float smoothTime = 0.05F;
    public float distanceThreshold = 0.01F;

    private bool inTransition = false;
    private Vector2 velocity = Vector2.zero;
    private Vector2 transitionTarget = Vector2.zero;

    void Start()
    {
        // Debug.Log("Starting Player Script");
    }

    void Update()
    {
        if (photonView.IsMine)
        {
            if (Input.GetMouseButtonDown(0))
            {
                // Debug.Log("sending out ray");
                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
                GameObject target = hit.collider.gameObject;

                if (hit.collider != null)
                {
                    
                    if(!inTransition) {
                        transitionTarget = target.transform.position;
                        inTransition = true;
                        target.GetComponent<ObjectController>().TransitionToPosition(transform.position);
                    }

                    // pseudocode
                    // if(target == someObjectType) {
                    // }
                }
            }
        }
        

        Vector2 current2DPosition = new Vector2(transform.position.x, transform.position.y);

        if(Vector2.Distance(current2DPosition, transitionTarget) <= distanceThreshold) {
            inTransition = false;
        }
        
    }

    void FixedUpdate() {
        if(inTransition) {
            transform.position = Vector2.SmoothDamp(transform.position, transitionTarget, ref velocity, smoothTime);
        }
    }
}
