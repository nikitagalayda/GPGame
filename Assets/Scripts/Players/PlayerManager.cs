using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Pun.Demo.PunBasics;

public class PlayerManager : MonoBehaviourPunCallbacks, IPunObservable
{
    public float movePower = 10f;
    public float jumpPower = 15f; //Set Gravity Scale in Rigidbody2D Component to 5

    private Rigidbody2D rb;
    private Animator anim;
    Vector3 movement;
    private int direction=1;
    bool isJumping = false;
    bool isInAir = false;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // 為玩家本人的狀態, 將 IsFiring 的狀態更新給其他玩家
            //stream.SendNext(IsFiring);
        }
        else
        {
            // 非為玩家本人的狀態, 單純接收更新的資料
            //this.IsFiring = (bool)stream.ReceiveNext();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
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
    private void OnTriggerEnter2D(Collider2D other) {
        anim.SetBool("isJumping",false); 
        isJumping = false;
    }

    private void OnTriggerStay2D(Collider2D other) {
        isInAir = false;
    }

    private void OnTriggerExit2D(Collider2D other) {
        isInAir = true;
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

}
