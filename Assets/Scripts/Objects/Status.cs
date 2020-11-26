using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Status : MonoBehaviour
{
    public string statusName;
    public string getStatusName(){
        return this.statusName;
    }
    public void setStatusName(string inputName){
        this.statusName = inputName;
    }

    public virtual void run(){
        Debug.Log("run default: "+statusName);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
