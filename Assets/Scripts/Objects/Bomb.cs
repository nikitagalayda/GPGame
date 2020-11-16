using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public bool triger_flag = false;
    public float radius = 20;
    public float bombForce = 100;
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

        //獲得以炸彈為中心的一定範圍內的所有物件
        Collider2D[] colliders = Physics2D.OverlapCircleAll(gameObject.transform.position,radius);
        foreach (Collider2D itemCollider2D in colliders)
        {
            GameObject item = itemCollider2D.gameObject;
            if(item == gameObject){
                Debug.Log("bomb effect: self " + item.name);
            }else
            {
                Vector3 forceDirection = -(gameObject.transform.position - item.transform.position);
                forceDirection[2] = 0;
                Debug.Log("bomb effect: other " + item.name + ":" +  forceDirection.normalized);
                item.GetComponent<Rigidbody2D>().AddForce(forceDirection * bombForce);
            }
        }


        triger_flag = false;
        Destroy(this.gameObject);
    }
}
