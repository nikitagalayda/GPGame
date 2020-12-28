using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuTextController : MonoBehaviour
{
    private float fontSize;
    private Text text;
    private static float t = 0.0f;

    void Start()
    {
        text = gameObject.GetComponent<Text>();
        fontSize = text.fontSize;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EnlargeText(int factor) {
        Mathf.Lerp(fontSize, fontSize*factor, t);
    }

    public void ReturnToNormalSize() {

    }
}
