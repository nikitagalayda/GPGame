using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Pun.Demo.PunBasics;

public class PlayerManager : MonoBehaviourPunCallbacks
{
    public InventoryObject inventory;
    public float movePower = 10f;
    public float jumpPower = 15f; //Set Gravity Scale in Rigidbody2D Component to 5
    public float bulletSpawnOffset = 100f;
    private Rigidbody2D rb;
    private Animator anim;
    Vector3 movement;
    private int direction=1;
    bool isJumping = false;
    bool isInAir = false;

    // Start is called before the first frame update
    void Start()
    {
        /*
        CameraWork2D _cameraWork2d = this.gameObject.GetComponent<CameraWork2D>();
    
        if (_cameraWork2d != null)
        {
            if (photonView.IsMine)
            {
                _cameraWork2d.OnStartFollowing();
            }
        }
        else 
        {
            Debug.LogError("playerPrefab- CameraWork component 遺失", 
                this); 
        }
        */
        
        if (photonView.IsMine == false && PhotonNetwork.IsConnected == true)
        {
            return;
        }
        
        rb=GetComponent<Rigidbody2D>();
        anim=GetComponent<Animator>();

    }
    
    private void FixedUpdate() {
        if (photonView.IsMine == false && PhotonNetwork.IsConnected == true)
        {
            return;
        }
        Jump();
        Run();

    }
    private void Update(){
        if (photonView.IsMine == false && PhotonNetwork.IsConnected == true)
        {
            return;
        }
        UseTheFirstItem();
        //DefaultShooting();
    }

    public void OnTriggerFloorDetector(Collider2D other){
        anim.SetBool("isJumping",false); 
        isJumping = false;
    }

    public void OnTriggerStayFloorDetector(Collider2D other) {
        isInAir = false;
    }

    public void OnTriggerExitFloorDetector(Collider2D other) {
        isInAir = true;
    }

    public void OnTriggerEnterItemObjectDetector(Collider2D other) {
        Debug.Log(this.name+" touch: " + other.name);
        var item = other.GetComponent<GroundItem>();
        //Debug.Log(item);
        if(item){
            inventory.AddItem(new Item(item.item), 1);
            Destroy(other.gameObject);
        }
    }

    private void OnApplicationQuit(){
        inventory.Container.Items.Clear();
    }

    void Run(){
        Vector3 moveVelocity= Vector3.zero;
            anim.SetBool("isRunning",false);


        if( Input.GetAxisRaw("Horizontal")<0){
            direction= -1;
            moveVelocity = Vector3.left;

            transform.localScale = new Vector3(direction,1,1);
            anim.SetBool("isRunning",true);

        }
        if( Input.GetAxisRaw("Horizontal")>0){
            direction= 1;
            moveVelocity = Vector3.right;

            transform.localScale = new Vector3(direction,1,1);
            anim.SetBool("isRunning",true);

        }
        transform.position+=moveVelocity*movePower*Time.deltaTime;
    }
    void Jump(){
        if( ( Input.GetButtonDown("Jump") || Input.GetAxisRaw("Vertical")>0 )
        &&!anim.GetBool("isJumping")){
            isJumping=true;
            anim.SetTrigger("doJumping");
            anim.SetBool("isJumping",true);
        }
        if(!isJumping){
            return;
        }

        if(isInAir){
            return;
        }

        rb.velocity = Vector2.zero;

        Vector2 jumpVelocity = new Vector2(0,jumpPower);
        rb.AddForce(jumpVelocity,ForceMode2D.Impulse);

        isJumping=false;
    }

    void UseTheFirstItem(){
        if(Input.GetMouseButtonDown(1)){
            //Debug.Log("GetButtonDown: time = " + Time.time);
            var item = inventory.GetItemByIndex(0);
            if(item != null){
                //Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                inventory.RemoveItem(item,1);                
                //item.ExecuteAllItemBuff(this.gameObject,mousePosition);
                item.ExecuteAllItemBuff(this.gameObject);
            }
        }
    }

    void DefaultShooting(){
        if(Input.GetMouseButtonDown(0)){
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            this.gameObject.GetComponent<ProjectileGenerator>().GenerateTheBullet(mousePosition,this.transform.position,bulletSpawnOffset);
        }        
    }
}
