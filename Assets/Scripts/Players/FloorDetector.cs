using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorDetector : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other) {
        this.transform.parent.gameObject.GetComponent<PlayerManager>().OnTriggerFloorDetector(other);
    }
    private void OnTriggerStay2D(Collider2D other) {
        this.transform.parent.gameObject.GetComponent<PlayerManager>().OnTriggerStayFloorDetector(other);
    }
    private void OnTriggerExit2D(Collider2D other) {
        this.transform.parent.gameObject.GetComponent<PlayerManager>().OnTriggerExitFloorDetector(other);
    }    
}
