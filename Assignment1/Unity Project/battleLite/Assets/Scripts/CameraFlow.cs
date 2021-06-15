using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFlow : MonoBehaviour
{

    public Transform target;
    public float distanceUp = 10f;
    public float distanceAway = 10f;
    public float smooth = 2f;
    public float camDepthSmooth = 5f;
    // Start is called before the first frame update
    void Start()
    {
        if ((Input.mouseScrollDelta.y < 0 && Camera.main.fieldOfView >= 3) ||
            Input.mouseScrollDelta.y > 0 && Camera.main.fieldOfView <= 80)
        {

            Camera.main.fieldOfView += Input.mouseScrollDelta.y * camDepthSmooth * Time.deltaTime;
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 disPos = target.position + Vector3.up * distanceUp - target.forward * distanceAway * distanceAway;
        transform.position = Vector3.Lerp(transform.position, disPos, Time.deltaTime * smooth* smooth);
        transform.LookAt(target.position);
    }


}
