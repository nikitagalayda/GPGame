using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;
public class SimpleTeleport_NetworkVersion : MonoBehaviourPunCallbacks
{

    [Tooltip("The local player instance. Use this to know if the local player is represented in the Scene")]
    public static GameObject LocalPlayerInstance;
    public float smoothTime = 0.05F;
    public float distanceThreshold = 1F;
    public float shootCooldown = 2.0f;
    // public float bulletSpeed;
    public GameObject bulletPrefab;
    private bool falling = false;
    private bool inTransition = false;
    private Vector2 velocity = Vector2.zero;
    private Vector2 transitionTarget = Vector2.zero;
    private float nextShotTimestamp = 0.0f;
    private bool canShoot = true;
    private bool leaving = false;

    void Start()
    {
     
        //GameObject.Find("Main Camera").GetComponent<Camera_Network>().setPlayer(this.gameObject.transform);
        // Debug.Log("Starting Player Script");
    }
    void Awake()
    {
  
        //GetComponent<SpriteRenderer>().color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f); 
        // #Important
        // used in GameManager.cs: we keep track of the localPlayer instance to prevent instantiation when levels are synchronized
        if (photonView.IsMine)
        {
            SimpleTeleport_NetworkVersion.LocalPlayerInstance = this.gameObject;
            
        }
        // #Critical
        // we flag as don't destroy on load so that instance survives level synchronization, thus giving a seamless experience when levels load.
        DontDestroyOnLoad(this.gameObject);
    }
    void Update()
    {
        if (photonView.IsMine)
        {
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
                    GameObject bullet = PhotonNetwork.Instantiate("BulletPrefab_Network", transform.position, Quaternion.identity);
                    Vector2 shootDirection = (Vector2)Vector3.Normalize(currentMouseVector - (Vector2)transform.position);
                    //Debug.Log(shootDirection);
                    bullet.GetComponent<ProjectileController_Network>().moveToTarget(shootDirection);
                    bullet.GetComponent<ProjectileController_Network>().parentObject = this.gameObject;
                // }
            }
            /*
            GameObject manager = GameObject.Find("Game Manager");
            
            bool start = manager.GetComponent<GameManager>().gameStart;
            if (start == true && falling == false){

                GetComponent<Rigidbody2D>().gravityScale = 0.05f;
                falling = true;
            }
            */
            if(Time.time >= nextShotTimestamp) {
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
        Die(transform.position.y);

        /**if (Vector2.Distance(current2DPosition, transitionTarget) <= distanceThreshold)
        {
            inTransition = false;
        }
        **/
        

    }

    void FixedUpdate()
    {
        /**if (inTransition )
        {
            transform.position = Vector2.SmoothDamp(transform.position, transitionTarget, ref velocity, smoothTime);
        }**/
    }
    
    public void TransitionToPosition(Vector2 targetPosition)
    {
        /**if (!inTransition)
        {
            transitionTarget = targetPosition;
            // Debug.Log(transitionTarget);
            inTransition = true;
        }**/
        transform.position = targetPosition;
    }

   private void Die(float currentY)
    {
        if (currentY <= -10f && photonView.IsMine && leaving==false)
        {
            GameManager.Instance.LeaveRoom();
            leaving = true;
        }
    }


}
