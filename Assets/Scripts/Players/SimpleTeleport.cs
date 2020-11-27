using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;
public class SimpleTeleport : MonoBehaviourPunCallbacks
{
    public float smoothTime = 0.05F;
    public float distanceThreshold = 0.01F;
    public float shootCooldown = 2.0f;
    // public float bulletSpeed;
    public GameObject bulletPrefab;

    private bool inTransition = false;
    private Vector2 velocity = Vector2.zero;
    private Vector2 transitionTarget = Vector2.zero;
    private float nextShotTimestamp = 0.0f;
    private bool canShoot = true;

    void Start()
    {
        // Debug.Log("Starting Player Script");
    }

    void Update()
    {
        if (true)
        {
            if (!inTransition)
            {
                transform.Translate(new Vector3(0, -1, 0) * Time.deltaTime);
            }
            // Debug.Log("sending out ray");
            // RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            // GameObject target = hit.collider.gameObject;
            if (Input.GetMouseButton(0) && canShoot)
            {
                canShoot = false;
                nextShotTimestamp = Time.time + shootCooldown;
                // if (Time.time >= shootTimeStamp + shootCooldown)
                // {
                // shootTimeStamp = Time.time;
                Vector2 currentMouseVector = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
                Vector2 shootDirection = (Vector2)Vector3.Normalize(currentMouseVector - (Vector2)transform.position);
                Debug.Log(shootDirection);
                bullet.GetComponent<ProjectileController_Network>().moveToTarget(shootDirection);
                bullet.GetComponent<ProjectileController_Network>().parentObject = this.gameObject;
                // }
            }
            if (Time.time >= nextShotTimestamp)
            {
                canShoot = true;
            }

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

        if (Vector2.Distance(current2DPosition, transitionTarget) <= distanceThreshold)
        {
            inTransition = false;
        }

    }

    void FixedUpdate()
    {
        if (inTransition)
        {
            transform.position = Vector2.SmoothDamp(transform.position, transitionTarget, ref velocity, smoothTime);
        }
    }

    public void TransitionToPosition(Vector2 targetPosition)
    {
        if (!inTransition)
        {
            transitionTarget = targetPosition;
            // Debug.Log(transitionTarget);
            inTransition = true;
        }
    }
}
