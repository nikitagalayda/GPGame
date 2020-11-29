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
        while(true) {
            this.transform.position = this.transform.position +new Vector3(0, 0.3f,0);
            yield return new WaitForSeconds(0.1f);
        }
    }
}
