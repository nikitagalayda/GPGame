using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class CharacterController : MonoBehaviourPunCallbacks
{
    public float MovementSpeed = 4.5f;
    public float JumpForce = 10f;

    private Rigidbody2D _rigidbody;
    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine)
        {
            float movement = Input.GetAxis("Horizontal");
            transform.position += new Vector3(movement,0,0) * Time.deltaTime * MovementSpeed;

            if(Input.GetButtonDown("Jump") && Mathf.Abs(_rigidbody.velocity.y)<0.01f )
            {
                _rigidbody.AddForce(new Vector2(0, JumpForce), ForceMode2D.Impulse);
            }
            if (_rigidbody.velocity.y<=-2.0f) _rigidbody.gravityScale = 0.0f;
            else _rigidbody.gravityScale = 1.0f;
        }
        
    }
}
