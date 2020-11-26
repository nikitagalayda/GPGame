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
    public GameObject PlayerUiPrefab;
    // public float bulletSpeed;
    public GameObject bulletPrefab;

    private Rigidbody2D rb;
    private bool falling = false;
    private bool inTransition = false;
    private Vector2 velocity = Vector2.zero;
    private Vector2 transitionTarget = Vector2.zero;
    private float nextShotTimestamp = 0.0f;
    private bool canShoot = true;
    private Color characterColor = Color.blue;

    void Start()
    {
        //initial player UI
        if (PlayerUiPrefab != null)
        {
            GameObject _uiGo = Instantiate(PlayerUiPrefab);
            _uiGo.SendMessage("SetTarget", this, SendMessageOptions.RequireReceiver);
        }
        else
        {
            Debug.LogWarning("<Color=Red><a>Missing</a></Color> PlayerUiPrefab reference on player Prefab.", this);
        }

        // GameObject.Find("Main Camera").GetComponent<Camera_Network>().setPlayer(this.gameObject.transform);

        CameraWork _cameraWork = this.gameObject.GetComponent<CameraWork>();


        if (_cameraWork != null)
        {
            if (photonView.IsMine)
            {
                _cameraWork.OnStartFollowing();
            }
        }
        else
        {
            Debug.LogError("<Color=Red><a>Missing</a></Color> CameraWork Component on playerPrefab.", this);
        }
        // Debug.Log("Starting Player Script");
    }
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        characterColor = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
        GetComponent<SpriteRenderer>().color = characterColor; 
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
                    
                    bullet.GetComponent<ProjectileController_Network>().parentObject = this.gameObject;
                    bullet.GetComponent<ProjectileController_Network>().moveToTarget(shootDirection);
                // }
            }
            //GameObject manager = GameObject.Find("Game Manager");
            

            if(Time.time >= nextShotTimestamp) {
                canShoot = true;
            }


        }



        

    }

    void FixedUpdate()
    {

    }
    [PunRPC]
    public void TransitionToPosition(Vector3 targetPosition)
    {
        transform.position = (Vector2)targetPosition;
        
    }


    void OnCollisionEnter2D(Collision2D Collider)
    {
        print("A:" + Collider.gameObject.name); //印出A:碰撞對象的名字
        //if(Collider.gameObject.name == "Lava")
            //GameManager.Instance.LeaveRoom();
        //if( photonView.IsMine && Collider.gameObject.name == "BulletPrefab_Network(Clone)"){
                //this.transform.position = Collider.gameObject.GetComponent<ProjectileController_Network>().initialPos;
                //parentObject.transform.position =  bulletPos;

        //}
    }

    void OnTriggerEnter2D(Collider2D other) {
            Debug.Log("ASDASDASDADASDADADSASD");
            Debug.Log("trigger WITH " + other.gameObject.name);
            //if( other.gameObject.name == "BulletPrefab_Network(Clone)"){
              //  Debug.Log("ASDASDASDADASDADADSASD");
                //TransitionToPosition(other.gameObject.GetComponent<ProjectileController_Network>().initialPos);
                //parentObject.transform.position =  bulletPos;

            }
        


    #region IPunObservable implementation







    #endregion

}
