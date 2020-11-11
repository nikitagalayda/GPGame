using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum types
{
    countDown,
    onCollision,
    stop
};
public class BombTriger : MonoBehaviour
{
    public types trigerType;
    public float countDownTime = 0;
    public float passTime = 0; 
    public GameObject target = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            switch (trigerType)
            {
                case types.countDown:
                    //Debug.Log("CountDown");
                    CountDown();
                    if(passTime >= countDownTime){
                        Triger();
                        trigerType = types.stop;
                    }
                    break;
                case types.onCollision:
                    Debug.Log("onCollision");
                    break;
            }
        }
    }
    
    void CountDown(){
        passTime = passTime + Time.deltaTime;
    }

    void Triger(){
        target.GetComponent<Bomb>().triger_flag = true;
    }
}
