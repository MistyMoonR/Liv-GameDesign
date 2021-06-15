using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    public Transform player;
    private Camera camera;

    private Vector3 offset;
    // Start is called before the first frame update
    void Start()
    {
        offset = transform.position - player.position;
        camera = this.GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = player.position + offset;
        transform.rotation = player.rotation;
    }
}
