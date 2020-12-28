using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;
public class SimpleTeleport_NetworkVersion : MonoBehaviourPunCallbacks, IPunInstantiateMagicCallback
{

    [Tooltip("The local player instance. Use this to know if the local player is represented in the Scene")]
    public static GameObject LocalPlayerInstance;
    public float smoothTime = 0.05F;
    public float distanceThreshold = 1F;
    public float shootCooldown = 2.0f;
    public GameObject PlayerUiPrefab;
    public GameObject[] bulletPrefabs;
    // public float bulletSpeed;
    public GameObject bulletPrefab;
    [Tooltip("diff style of spaceShip sprite")]
    [SerializeField]
    private Sprite[] spaceSrpites;
    private Rigidbody2D rb;
    private bool falling = false;
    private bool inTransition = false;
    private Vector2 velocity = Vector2.zero;
    private Vector2 transitionTarget = Vector2.zero;
    private float nextShotTimestamp = 0.0f;
    private bool canShoot = true;
    private Color characterColor = Color.blue;
    public BoxCollider2D m_Collider;
    public bool imDie = false;
    private GameObject[] Players;
    private int watchingPlayer = 0;
    private CameraWork _cameraWork;
    private int color = -1;
    //public GameObject lineRend;
    void Start()
    {
        //lineRend.SetActive(true);
        //lineRend.GetComponent<LineRenderer>().SetPosition(0, this.transform.position);
        //lineRend.GetComponent<LineRenderer>().SetPosition(1, this.transform.position+new Vector3(10,10,0));

        //initial player UI
        createNameUI();
        
        _cameraWork = this.gameObject.GetComponent<CameraWork>();


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
        //GetComponent<SpriteRenderer>().color = characterColor; 
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
        if (photonView.IsMine && !imDie)
        {
            // move player direction to the cursor
            Vector2 positionOnScreen = Camera.main.WorldToViewportPoint (transform.position);
            Vector2 mouseOnScreen = (Vector2)Camera.main.ScreenToViewportPoint(Input.mousePosition);
            float angle = AngleBetweenTwoPoints(positionOnScreen, mouseOnScreen);
            transform.rotation =  Quaternion.Euler (new Vector3(0f,0f,angle+90));
            // Debug.Log("sending out ray");
            // RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            // GameObject target = hit.collider.gameObject;
            PlayerManager plyManager = this.gameObject.GetComponent<PlayerManager>();
            if (Input.GetMouseButton(0) && canShoot && plyManager.energy >= plyManager.defaultShootingEnergyConsuming)
            {
                canShoot = false;
                plyManager.energy -= plyManager.defaultShootingEnergyConsuming;
                nextShotTimestamp = Time.time + shootCooldown;
                // if (Time.time >= shootTimeStamp + shootCooldown)
                // {
                    // shootTimeStamp = Time.time;
                    Vector2 currentMouseVector = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    object[] myCustomInitData = new object[]{color};
                    
                    GameObject bullet = PhotonNetwork.Instantiate(this.bulletPrefabs[color].name, transform.position, Quaternion.Euler(0, 0,  angle+90), 0, myCustomInitData);
                    Vector2 shootDirection = (Vector2)Vector3.Normalize(currentMouseVector - (Vector2)bullet.transform.position);
                    Vector2 shootDirection2 = (Vector2)Vector3.Normalize(currentMouseVector - (Vector2)transform.position);
                    
                    
                    bullet.GetComponent<ProjectileController_Network>().parentObject = gameObject;
                    bullet.GetComponent<ProjectileController_Network>().moveToTarget(shootDirection);
                // }
            }
            //GameObject manager = GameObject.Find("Game Manager");
            
            

            if(Time.time >= nextShotTimestamp) {
                canShoot = true;
            }


        }
        if(imDie && photonView.IsMine)
        {
            
            if (Input.GetKeyDown("space"))
            {
                _cameraWork.StopFollowing();
                watchingPlayer = (watchingPlayer + 1) % Players.Length;
                if(Players[watchingPlayer]==null)
                    watchingPlayer = (watchingPlayer + 1) % Players.Length;
                _cameraWork = Players[watchingPlayer].GetComponent<CameraWork>();
                _cameraWork.OnStartFollowing();
            }
        }


        

    }

    void FixedUpdate()
    {
        Vector2 current2DPosition = new Vector2(transform.position.x, transform.position.y);

        if (inTransition)
        {
            if (Vector2.Distance(current2DPosition, transitionTarget) <= distanceThreshold)
            {
                inTransition = false;
            }
            transform.position = Vector2.SmoothDamp(transform.position, transitionTarget, ref velocity, smoothTime);
        }

    }

    public void TransitionToPosition2(Vector3 targetPosition)
    {
        //if (!inTransition)
        //{
        this.transform.position = targetPosition;
        //   Debug.Log(transitionTarget);
        //inTransition = true;
        //}
    }

    [PunRPC]
    public void TransitionToPosition(Vector3 targetPosition)
    {
        //if (!inTransition)
        //{
        this.transform.position = targetPosition;
         //   Debug.Log(transitionTarget);
            //inTransition = true;
        //}
        //StartCoroutine(waitToTrans(0f, targetPosition));
        //m_Collider.enabled = false; 
        //StartCoroutine(turnOnCollider(0.15f));
    }
    [PunRPC]
    public void TransitionToPosition3(Vector3 targetPosition)
    {

        StartCoroutine(waitToTrans(0f, targetPosition));
        m_Collider.enabled = false; 
        StartCoroutine(turnOnCollider(0.3f));
    }
    [PunRPC]
    public void MoveLine(Vector3 otherPos)
    {
        Debug.Log("move line");
        //lineRend.GetComponent<LineRenderer>().SetPosition(0, this.transform.position);
        //lineRend.GetComponent<LineRenderer>().SetPosition(1, otherPos);
    }
    IEnumerator waitToTrans(float time, Vector3 newPos)
    {

        //Wait for 4 seconds
        yield return new WaitForSeconds(time);

        //Rotate 40 deg
        transform.position = newPos;
    }    
    public void createNameUI()
    {
        if (PlayerUiPrefab != null)
        {
            GameObject _uiGo = Instantiate(PlayerUiPrefab);
            _uiGo.SendMessage("SetTarget", this, SendMessageOptions.RequireReceiver);
        }
        else
        {
            Debug.LogWarning("<Color=Red><a>Missing</a></Color> PlayerUiPrefab reference on player Prefab.", this);
        }
    }

    void OnCollisionEnter2D(Collision2D Collider)
    {
        //print("A:" + Collider.gameObject.name); //印出A:碰撞對象的名字
        if (Collider.gameObject.name == "death_zone" && photonView.IsMine)
        {
            imDie = true;
            GameManager.Instance.dieText.SetActive(true);
            //GameManager.Instance.hintText.SetActive(true);
            StartCoroutine(delDieText());
            
            Destroy(this.gameObject.GetComponent<PlayerManager>());
            Destroy(this.gameObject.GetComponent<PhotonRigidbody2DView>());
            Destroy(this.gameObject.GetComponent<Rigidbody2D>());
            
            //PhotonNetwork.Destroy(this.gameObject);
            GameManager.Instance.leaveButton.SetActive(true);
            //Players = GameObject.FindGameObjectsWithTag("player");
        }
            
                //GameManager.Instance.LeaveRoom();
        //if( photonView.IsMine && Collider.gameObject.name == "BulletPrefab_Network(Clone)"){
                //this.transform.position = Collider.gameObject.GetComponent<ProjectileController_Network>().initialPos;
                //parentObject.transform.position =  bulletPos;

        //}
    }
    private IEnumerator delDieText()
    {
        yield return new WaitForSeconds(3f);
        GameManager.Instance.dieText.SetActive(false);

    }
    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("trigger WITH " + other.gameObject.name);

        if (other.gameObject.name == "BulletPrefab_Network(Clone)")
        {
            //  Debug.Log("ASDASDASDADASDADADSASD");
            if ((other.gameObject.GetComponent<ProjectileController_Network>().parentObject != this.gameObject))
            {
                //this.transform.position = other.gameObject.GetComponent<ProjectileController_Network>().initialPos;
                Debug.Log("Wooooooooo");
                
            }
            //parentObject.transform.position =  bulletPos;

        }
    }


    private float AngleBetweenTwoPoints(Vector3 a, Vector3 b) {
         return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
    }

    IEnumerator turnOnCollider(float time)
    {

        //Wait for 4 seconds
        yield return new WaitForSeconds(time);

        //Rotate 40 deg
        m_Collider.enabled = true;
    }

    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        object[] instantiationData = info.photonView.InstantiationData;

        int number = (int)instantiationData[0];
        this.transform.FindChild("Spaceship").GetComponent<SpriteRenderer>().sprite = spaceSrpites[number];
        color = number;
    }
    #region IPunObservable implementation







    #endregion

}