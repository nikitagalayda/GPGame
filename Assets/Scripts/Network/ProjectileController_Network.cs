using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class ProjectileController_Network : MonoBehaviourPunCallbacks, IPunInstantiateMagicCallback
{
    public float projectileSpeed = 1f;
    public GameObject parentObject = null;
    public Vector3 initialPos;
    private Vector2 target;
    private bool movingToTarget = false;
    private float liveSecond = 0.7f;
    [Tooltip("diff style of bullet sprite")]
    [SerializeField]
    private Sprite[] bulletSrpites;
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
            Debug.Log("shoot dir"+ transform.position);
            transform.position = transform.position + (transform.up * Time.deltaTime * projectileSpeed);
            //this.transform.Translate(target * projectileSpeed * Time.deltaTime);
        }
    }

    public void moveToTarget(Vector2 tg)
    {
        //this.gameObject.GetComponent<Rigidbody2D>().AddForce(transform.transform.right * projectileSpeed * 10);
        //projectileSpeed += parentObject.GetComponent<Rigidbody2D>().velocity.magnitude;
        target = tg;
        movingToTarget = true;
        //Vector2 positionOnScreen = Camera.main.WorldToViewportPoint (transform.position);
        //Vector2 mouseOnScreen = (Vector2)Camera.main.ScreenToViewportPoint(Input.mousePosition);
        //float angle = AngleBetweenTwoPoints(positionOnScreen, mouseOnScreen);
        //transform.rotation =  Quaternion.Euler (new Vector3(0f,0f,angle));
        //transform.rotation =  Quaternion.Euler (new Vector3(tg.y,tg.x,0));
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
            if(photonView.IsMine && other.gameObject.name == "meteorite(Clone)"){
                PhotonView photonViewParent = PhotonView.Get(parentObject.gameObject);
                photonViewParent.RPC("MoveLine", RpcTarget.All,other.transform.position);

                Debug.Log("-----------------ismine" + other.gameObject.name);
                Vector2 temp = other.transform.position;
                Vector3 temp2 = parentObject.transform.position;
                //other.transform.position = parentObject.transform.position;
                PhotonView photonViewOther = PhotonView.Get(other.gameObject);
                photonViewOther.RPC("ItemTransitionToPosition", RpcTarget.All, temp2);
                parentObject.transform.position = new Vector3(temp.x, temp.y, temp2.z);
                //parentObject.GetComponent<SimpleTeleport_NetworkVersion>().TransitionToPosition(temp);
                PhotonNetwork.Destroy(this.gameObject);
                
            
                //parentObject.GetComponent<SimpleTeleport_NetworkVersion>().TransitionToPosition(temp);
            }
            if(photonView.IsMine && other.gameObject.name == "Player(Clone)"){
                PhotonView photonViewParent = PhotonView.Get(parentObject.gameObject);
                photonViewParent.RPC("MoveLine", RpcTarget.All,bulletPos);
                Debug.Log("-----------------ismine" + other.gameObject.name);
                Vector3 temp = other.transform.position;
                PhotonView photonViewOther = PhotonView.Get(other.gameObject);
                //other.transform.position = parentObject.transform.position;
                //other.gameObject.GetComponent<SimpleTeleport_NetworkVersion>().TransitionToPosition(parentObject.transform.position);

                
                
                photonViewOther.RPC("TransitionToPosition", RpcTarget.All, parentObject.transform.position);
                //PhotonView photonViewParent = PhotonView.Get(parentObject.gameObject);
                //photonViewParent.RPC("TransitionToPosition", RpcTarget.All, temp);
                ///parentObject.transform.position = bulletPos;
                
                photonViewParent.RPC("TransitionToPosition3", RpcTarget.All,bulletPos);
                //generateEnergy(parentObject);
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

    private float AngleBetweenTwoPoints(Vector3 a, Vector3 b) {
         return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
    }
    public void generateEnergy(GameObject target)
    {
        int num = 10;
        for(int i = 0; i < num; i++) {
            Vector3 genPos = new Vector3(transform.position.x + Random.Range(-0.5f, 0.5f), transform.position.y + Random.Range(-0.5f, 0.5f), transform.position.z); 
            GameObject energy = PhotonNetwork.Instantiate("energy", genPos, Quaternion.identity);
            energy.GetComponent<GoDestination>().SetDest(target);
            Debug.Log("asdasdasd"+energy.GetComponent<GoDestination>().DestObject.name);
        }
    }
    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        object[] instantiationData = info.photonView.InstantiationData;

        int number = (int)instantiationData[0];
        if(number>=0){
            //this.gameObject.GetComponent<SpriteRenderer>().sprite = bulletSrpites[number];
            var animator = gameObject.GetComponent<Animator>();
            // switch (number)
            // {
            //     case 0:
            //         animator.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("Assets/Assets/Sprites/SFX/Bullet/blue_bullet_AC.controller");
            //         break;
            //     case 1:
            //         animator.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("Assets/Assets/Sprites/SFX/Bullet/green_bullet_AC.controller");
            //         break;
            //     case 2:
            //         animator.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("Assets/Assets/Sprites/SFX/Bullet/orange_bullet_AC.controller");
            //         break;
            //     case 3:
            //         animator.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("Assets/Assets/Sprites/SFX/Bullet/red_bullet_AC.controller");
            //         break;

            // }
            

        }
            
    }
}
