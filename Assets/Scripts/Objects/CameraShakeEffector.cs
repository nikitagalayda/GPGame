using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class CameraShakeEffector : MonoBehaviour
{
    public bool triger_flag = false;
    public float radius = 20;
    public float time = 5;
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
        Debug.Log("triger camera shake effect");
        //例項化一個爆炸效果
        Instantiate(explosion,gameObject.transform.position,Quaternion.identity);
        //GameObject bombAnime = PhotonNetwork.Instantiate(explosion.name, gameObject.transform.position, Quaternion.identity);
        //獲得以炸彈為中心的一定範圍內的所有物件
        Collider2D[] colliders = Physics2D.OverlapCircleAll(gameObject.transform.position,radius);
        foreach (Collider2D itemCollider2D in colliders)
        {
            GameObject item = itemCollider2D.gameObject;
            var statusScript = item.GetComponent<PlayerStatus>();
            if(statusScript != null)
            {
                int id = statusScript.GetStatusIdByName("CameraShaking");
                item.GetComponent<PlayerManager>().SetStatusEffect(id,time);
            }           
        }
        triger_flag = false;
        //Animator bombAnimator = bombAnime.GetComponentInChildren<Animator>();
        //PhotonNetwork.Destroy(bombAnime);        
        PhotonView.Destroy(this.gameObject);
    }
}
