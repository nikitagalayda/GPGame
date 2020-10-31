using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraController : MonoBehaviour
{
    public GameObject[] players;
    public float minCameraSize = 5.0f;
    public float maxCameraSize = 10.0f;

    private Vector2 screenBounds;
    private float dampVelocity = 0.0f;
    private float smoothTime = 0.5f;

    void Start()
    {
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
    }

    void Update()
    {
        ZoomCamera();        
    }

    private void ZoomCamera() {
        foreach(GameObject player in players) {
            if(ObjectOutOfBounds(Camera.main, player)) {
                Debug.Log("OUT OF BOUNDS");
                float target = Mathf.Lerp(Camera.main.orthographicSize, Camera.main.orthographicSize + 1f, 1 * Time.deltaTime);
                // transform.position = Mathf.SmoothDamp(Camera.main.orthographicSize, Camera.main.orthographicSize + 1f, ref dampVelocity, smoothTime);
                Camera.main.orthographicSize = Mathf.Clamp(target, minCameraSize, maxCameraSize);
            }
        }
    }

    private bool ObjectOutOfBounds(Camera camera, GameObject obj) {
        // TODO: change this to depend on sprite not the bounds?
        Bounds bnds = obj.GetComponent<BoxCollider2D>().bounds;
        Vector3 extents = bnds.extents;
        Vector3 offsetTransform = new Vector3(obj.transform.position.x - extents.x, obj.transform.position.y - extents.y, obj.transform.position.z);
        Vector3 viewPos = Camera.main.WorldToViewportPoint(offsetTransform);
        if(viewPos.x > 1.0f || viewPos.x < 0f || viewPos.y > 1.0f || viewPos.y < 0f) {
            return true;
        }

        return false;
    }
}
