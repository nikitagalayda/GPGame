using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity : MonoBehaviour
{

    public float Gforce;
    public float GRadius;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ForceField();
    }

    void ForceField(){
        Collider2D[] colliders = Physics2D.OverlapCircleAll(gameObject.transform.position,GRadius);
        foreach (Collider2D itemCollider2D in colliders)
        {
            GameObject item = itemCollider2D.gameObject;
            Vector3 forceDirection = (gameObject.transform.position - item.transform.position);
            forceDirection[2] = 0;
            item.GetComponent<Rigidbody2D>().AddForce(forceDirection * Gforce * Time.deltaTime,ForceMode2D.Impulse);              
        }
    }
}
