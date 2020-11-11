using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController_Network : MonoBehaviour
{
    public float projectileSpeed = 100.0f;
    public GameObject parentObject = null;

    private Vector2 target;
    private bool movingToTarget = false;

    void Start()
    {

    }

    void FixedUpdate()
    {
        if (movingToTarget)
        {   
            transform.Translate((target * projectileSpeed + parentObject.GetComponent<Rigidbody2D>().velocity ) * Time.deltaTime);
            //Debug.Log(target);
            //Debug.Log(parentObject.GetComponent<Rigidbody2D>().velocity);
        }
    }

    public void moveToTarget(Vector2 tg)
    {
        target = tg;
        movingToTarget = true;
    }

    void OnCollisionEnter2D(Collision2D other) {
        Debug.Log("COLLISION WITH " + other.gameObject.name);
        Vector2 temp = other.transform.position;
        other.gameObject.GetComponent<ObjectController_Network>().TransitionToPosition(parentObject.transform.position);
        List<GameObject> balloonSet = parentObject.GetComponents<SimpleTeleport_NetworkVersion> ()[0].balloonSet;
        foreach (GameObject item in balloonSet)
        {
            item.transform.position = temp + new Vector2 (0,2);
        }
        
        //GameObject balloon = parentObject.GetComponents<Chaining> ()[0].target;
        //balloon.transform.position = temp;
        parentObject.GetComponent<SimpleTeleport_NetworkVersion>().TransitionToPosition(temp);
        Destroy(this.gameObject);
    }

    // private void OnTriggerEnter2D(Collider2D other) {
    //     Debug.Log("ASDASDJLHASJDHJKAHSD");
    //     Debug.Log("COLLISION WITH " + other.gameObject.name);
    // }
}
