using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] float camSpeed;

    [SerializeField] Vector3 offset = Vector3.zero;

    Vector2 cameraDimension;

    [SerializeField] Camera cam;

    [SerializeField] Transform target;

    [SerializeField] BoxCollider2D cameraBounds;

    bool follow = true;

    void Awake()
    {
        cam = GetComponent<Camera>();
        cameraDimension.y = cam.orthographicSize;
        cameraDimension.x = cam.orthographicSize * cam.aspect;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 followPosition = new Vector3(target.position.x, 3f, 0f) + offset;

        float minX = cameraBounds.transform.position.x - cameraBounds.size.x / 2 + cameraDimension.x;
        float maxX = cameraBounds.transform.position.x + cameraBounds.size.x / 2 - cameraDimension.x;
        followPosition.x = Mathf.Clamp(followPosition.x, minX, maxX);

        Vector3 currentVelocity = Vector3.zero;
        transform.position = Vector3.SmoothDamp(transform.position, followPosition, ref currentVelocity, Time.deltaTime * camSpeed);
    }
}
