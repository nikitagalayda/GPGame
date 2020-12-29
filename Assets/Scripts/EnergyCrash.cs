using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class EnergyCrash : MonoBehaviour
{
    public bool triger_flag = false;
    public float radius = 20;
    public float removeEnergy = 30;
    //public float bombForce = 100;
    public GameObject explosion;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(triger_flag == true){
            TrigerBombEffect();
        }
    }

    void OnDrawGizmos() {
        //Gizmos.color = Color.yellow;
        //Gizmos.DrawSphere(gameObject.transform.position, radius);
    }

    void TrigerBombEffect(){
        Debug.Log("triger bomb effect");
        //例項化一個爆炸效果
        Instantiate(explosion,gameObject.transform.position,Quaternion.identity);
        //GameObject bombAnime = PhotonNetwork.Instantiate(explosion.name, gameObject.transform.position, Quaternion.identity);
        //獲得以炸彈為中心的一定範圍內的所有物件
        Collider2D[] colliders = Physics2D.OverlapCircleAll(gameObject.transform.position,radius);
        foreach (Collider2D itemCollider2D in colliders)
        {
            GameObject item = itemCollider2D.gameObject;
            var playerManager = item.GetComponent<PlayerManager>();
            if(playerManager != null){
                playerManager.energy -= removeEnergy;
            }   
        }
        triger_flag = false;       
        PhotonView.Destroy(this.gameObject);
    }
}
