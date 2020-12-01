using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(moveSelf());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator moveSelf() 
    {
        int i = 0;
        float speed = 0.05f;
        while(true) {
            if (i == 100)
            {
                speed = speed * 1.02f;
                i = 0;
            }
            this.transform.position = this.transform.position +new Vector3(0, speed,0);
            yield return new WaitForSeconds(0.01f);
            i++;
        }
    }
}
