using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuTextController : MonoBehaviour
{
    private int fontSize;
    private Text text;
    private static float t = 0.0f;
    private int currSize;

    void Start()
    {
        text = gameObject.GetComponentInChildren<Text>();

        fontSize = text.fontSize;
        currSize = fontSize;
    }

    // Update is called once per frame
    void Update()
    {
    }

    IEnumerator EnlargeText(int newSize, float lerpTime)
    {
        Debug.Log("START COROUTINE");
        float elapsedTime = 0f;
        int startSize = fontSize;

        do {
            text.fontSize = (int)Mathf.Lerp(startSize, newSize, (elapsedTime/lerpTime));
            elapsedTime += Time.deltaTime;
            // Debug.Log(text.fontSize);
            yield return null;
        } while(elapsedTime < lerpTime);
    }

    public void ChangeTextSize(float factor, float lerpTime) {
        StartCoroutine(EnlargeText((int)(fontSize*factor), lerpTime));
    }
}
