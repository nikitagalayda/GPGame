using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;

    


public class PlayerStatus : MonoBehaviourPunCallbacks
{
    enum Status {
        Slow,
        Freeze
    }

    public float[] statusTable = new float[2]{ 0.0f, 0.0f};

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < statusTable.Length; i++)
        {
            if(statusTable[i] > 0)
            {
                statusTable[i] -= Time.deltaTime;
                if(statusTable[i] <= 0)
                {
                    statusTable[i] = 0;
                    switch(i)
                    {
                        case (int)Status.Slow:
                            this.gameObject.GetComponent<PlayerManager>().movePower = 10.0f;
                            break;
                    }
                }
            }
        }
    }


    void OnCollisionEnter2D(Collision2D Collider)
    {
        if (Collider.gameObject.name == "slow" && photonView.IsMine)
        {
            if(statusTable[(int)Status.Slow] == 0)
                this.gameObject.GetComponent<PlayerManager>().movePower = 5.0f;
            statusTable[(int)Status.Slow] += 2.0f;

        }
    }
}
