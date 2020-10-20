using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
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
            transform.Translate(target * projectileSpeed * Time.deltaTime);
        }
    }

    public void moveToTarget(Vector2 tg)
    {
        target = tg;
        movingToTarget = true;
    }

    void OnCollisionEnter2D(Collision2D other) {
        Debug.Log("COLLISION WITH " + other.gameObject.name);
        other.gameObject.GetComponent<ObjectController>().TransitionToPosition(parentObject.transform.position);
        parentObject.GetComponent<SimpleTeleport>().TransitionToPosition(other.transform.position);
    }

    // private void OnTriggerEnter2D(Collider2D other) {
    //     Debug.Log("ASDASDJLHASJDHJKAHSD");
    //     Debug.Log("COLLISION WITH " + other.gameObject.name);
    // }
}
