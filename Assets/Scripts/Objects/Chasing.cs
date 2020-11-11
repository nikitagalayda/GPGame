using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chasing : MonoBehaviour
{
    public float MaxRotation = 0.01f;
    public float Speed = 30f;
    public float BlindDistance = 60f;
    float kp=0.1f;
    float ki=0.1f;
    float kd=0.1f; 
    public GameObject target;
    public Vector3 targetOffset;
    public Vector3 selftOffset;
    public Vector3 initialVelocity;
    private Rigidbody2D rigidbody2D;

    private Vector3 anchor;
    private Vector3 selfPosition;
    private float previousAngle;
    private float currentAngle;
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

    void GetAngle()
    {
        currentAngle = Vector2.Angle(anchor-selfPosition,transform.up);
    }


    void PIDtoAngleControl()
    {
        /*
        deltaTime = Time.deltaTime;
        float P = currentAngle;
        I = I + P*deltaTime;
        float D = 0;
        if(deltaTime==0){
            D = 0;
        }else
        {
            D = (currentAngle-previousAngle)/deltaTime;
        }
        float F = kp*P+ki*I+kd*D;
        if(F > MaxRotation){
            F = MaxRotation;
        }
        if(F < -MaxRotation){
            F = MaxRotation;
        }
        previousAngle = currentAngle;
        */


        transform.Rotate(0,0,currentAngle);

        //transform.Rotate(0,0,currentAngle);
        Vector3 moveDirection = transform.up;
        rigidbody2D.velocity = moveDirection*Speed;
    }

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        GetTargetAnchor();
        GetSelfPosition();
        GetAngle();
        I = 0;
        deltaTime = 0;
        previousAngle = currentAngle;
        initialVelocity = initialVelocity * Speed;
        rigidbody2D.velocity = initialVelocity;
    }

    // Update is called once per frame
    void Update()
    {
        GetTargetAnchor();
        GetSelfPosition();
        GetAngle();
        PIDtoAngleControl();     
        Debug.Log(rigidbody2D.velocity);   
        Debug.Log(currentAngle);
    }
}
