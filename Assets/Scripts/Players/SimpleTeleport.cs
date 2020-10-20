using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;
public class SimpleTeleport : MonoBehaviourPunCallbacks
{
    public float smoothTime = 0.05F;
    public float distanceThreshold = 0.01F;
    // public float bulletSpeed;
    public GameObject bulletPrefab;

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
            // Debug.Log("sending out ray");
            // RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            // GameObject target = hit.collider.gameObject;
            Vector2 currentMouseVector = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            Vector2 shootDirection = (Vector2)Vector3.Normalize(currentMouseVector-(Vector2)transform.position);
            Debug.Log(shootDirection);
            bullet.GetComponent<ProjectileController>().moveToTarget(shootDirection);
            bullet.GetComponent<ProjectileController>().parentObject = this.gameObject;
            // bullet.transform.Translate(shootDirection * bulletSpeed * Time.deltaTime);
            // bullet.GetComponent<Rigidbody2D>().velocity = shootDirection * bulletSpeed * Time.deltaTime;
            // Debug.Log(bullet.GetComponent<Rigidbody2D>().velocity);

            // if (hit.collider != null)
            // {
                
                // if(!inTransition) {
                //     transitionTarget = target.transform.position;
                //     inTransition = true;
                //     target.GetComponent<ObjectController>().TransitionToPosition(transform.position);
                // }

            //     // pseudocode
            //     // if(target == someObjectType) {
            //     // }
            // }
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

    public void TransitionToPosition(Vector2 targetPosition) {
        if(!inTransition) {
            transitionTarget = targetPosition;
            // Debug.Log(transitionTarget);
            inTransition = true;
        }
    }
}
