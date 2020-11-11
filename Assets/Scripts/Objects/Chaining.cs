using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]

public class Chaining : MonoBehaviour
{
    float kp=3f;
    float ki=0f;
    float kd=1f; 
    public GameObject target;
    public Vector3 targetOffset;
    float stableDistance=0.05f;
    public Vector3 selftOffset;
    
    Rigidbody2D playerRigidbody2D;

    private Vector3 anchor;
    private Vector3 selfPosition;
    private float previousDistance;
    private float currentDistance;
    private Vector3 force;
    private float I;
    private float deltaTime;

    void GetTargetAnchor()
    {
        anchor = target.transform.position + targetOffset;
    }

    void GetSelfPosition()
    {
        selfPosition = transform.position + selftOffset;
    }

    void GetDistance()
    {
        currentDistance = Vector3.Distance(anchor,selfPosition);
    }

    void PIDtoForceControl()
    {
        Vector3 baseForce = (anchor-selfPosition)/currentDistance;
        deltaTime = Time.deltaTime;
        float P = currentDistance - stableDistance;
        I = I + P*deltaTime;
        float D = 0;
        if(deltaTime==0){
            D = 0;
        }else
        {
            D = (currentDistance-previousDistance)/deltaTime;
        }
        float F = kp*P+ki*I+kd*D;
        previousDistance = currentDistance;
        force = baseForce*F;
        Vector2 Force2D = force;
        playerRigidbody2D.AddForce(Force2D);
    }

    // Start is called before the first frame update
    void Start()
    {
        playerRigidbody2D = GetComponent<Rigidbody2D>();
        GetTargetAnchor();
        GetSelfPosition();
        GetDistance();
        I = 0;
        deltaTime = 0;
        previousDistance = currentDistance;
    }

    // Update is called once per frame
    void Update()
    {
        GetTargetAnchor();
        GetSelfPosition();
        GetDistance();
        PIDtoForceControl();
        //Debug.Log(currentDistance);
    }
}
