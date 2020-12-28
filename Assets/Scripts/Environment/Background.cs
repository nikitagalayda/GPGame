using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    public GameObject[] backgroundCards;
    public float backgroundSpeed = 5f;

    private LinkedList<GameObject> cardList;
    private Vector3 screenBounds; 

    void Start()
    {
        cardList = new LinkedList<GameObject>(backgroundCards);

        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
    }

    void Update()
    {
        transform.Translate(new Vector3(0, -backgroundSpeed, 0) * Time.deltaTime);
    

    // void LateUpdate()
    // {
        GameObject lastBgCard = cardList.Last.Value;
        GameObject firstBgCard = cardList.First.Value;

        Vector3 cardExtents = lastBgCard.GetComponent<SpriteRenderer>().bounds.extents;

        if((Mathf.Abs(lastBgCard.transform.position.y) - cardExtents.y) >= screenBounds.y) {
            lastBgCard.transform.position = firstBgCard.transform.position + new Vector3(0, cardExtents.y*2, 0);
            cardList.AddFirst(lastBgCard);
            cardList.RemoveLast();
        }
    // }
    }
}
