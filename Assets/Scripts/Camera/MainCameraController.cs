using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraController : MonoBehaviour
{
    public GameObject[] players;
    public float minCameraSize = 5.0f;
    public float maxCameraSize = 10.0f;
    public float zoomSpeed = 100f;

    private float currentZoom = 5.0f;
    private Vector2 screenBounds;
    private float dampVelocity = 0.0f;
    private float smoothTime = 0.5f;
    private bool inTransition = false;

    void Start()
    {
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
    }

    void Update()
    {
        ZoomCamera();
    }

    private void ZoomCamera()
    {
        if (inTransition)
        {
            float targetSize = Mathf.Clamp(currentZoom, minCameraSize, maxCameraSize);
            float target = Mathf.Lerp(Camera.main.orthographicSize, targetSize, zoomSpeed * Time.deltaTime);
            Camera.main.orthographicSize = target;
            if (targetSize - target <= 0.1f)
            {
                inTransition = false;
            }
        }
        if (!inTransition)
        {
            if (AnyPlayerOutOfBounds(Camera.main, players))
            {
                currentZoom += 1f;
                inTransition = true;
            }
            // else if ((Camera.main.orthographicSize > minCameraSize) && !AnyPlayerOutOfBounds(Camera.main, players))
            else if(ShouldZoomIn())
            {
                currentZoom -= 1f;
                inTransition = true;
            }
        }
    }

    private bool AnyPlayerOutOfBounds(Camera camera, GameObject[] players)
    {
        foreach (GameObject player in players)
        {
            if (ObjectOutOfBounds(camera, player))
            {
                return true;
            }
        }
        return false;
    }

    private bool ObjectOutOfBounds(Camera camera, GameObject obj)
    {
        // TODO: change this to depend on sprite not the bounds?
        Bounds bnds = obj.GetComponent<BoxCollider2D>().bounds;
        Vector3 extents = bnds.extents;
        Vector3 offsetTransform = new Vector3(obj.transform.position.x - extents.x, obj.transform.position.y - extents.y, obj.transform.position.z);
        Vector3 viewPos = Camera.main.WorldToViewportPoint(offsetTransform);
        if (viewPos.x > 1.0f || viewPos.x < 0f || viewPos.y > 1.0f || viewPos.y < 0f)
        {
            return true;
        }

        return false;
    }

    private bool ShouldZoomIn() {
        float maxX = 0f;
        float maxY = 0f;

        foreach (GameObject player in players)
        {
            float normXPos = Camera.main.WorldToViewportPoint(player.transform.position).x;
            float normYPos = Camera.main.WorldToViewportPoint(player.transform.position).y;
            Debug.Log(normXPos);
            if(normXPos > maxX) {
                maxX = normXPos;
            }
            if(normYPos > maxY) {
                maxY = normYPos;
            }
        }

        if(maxX > 0.3f || maxY > 0.3f) {
            return true;
        }

        return false;
    }
}
