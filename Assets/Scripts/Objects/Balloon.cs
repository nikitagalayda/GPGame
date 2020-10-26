using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Balloon : MonoBehaviour
{
    Rigidbody2D playerRigidbody2D;
    const string VERTICAL = "Vertical";

    //[Header("目前的垂直方向")]
    //public float verticalDirection;

    [Header("垂直向上推力")]
    public float yForce;

    void MovementY()
    {
        //verticalDirection = Input.GetAxis(VERTICAL);
        playerRigidbody2D.AddForce(new Vector2(0, yForce));
    }

    // Start is called before the first frame update
    void Start()
    {
        playerRigidbody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        MovementY();
    }
}
