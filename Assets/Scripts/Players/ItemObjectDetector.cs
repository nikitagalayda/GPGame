using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObjectDetector : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D other){
        this.transform.parent.gameObject.GetComponent<PlayerManager>().OnTriggerEnterItemObjectDetector(other);
    }
}
