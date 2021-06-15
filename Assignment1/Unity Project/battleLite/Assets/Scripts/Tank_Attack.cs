using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank_Attack : MonoBehaviour
{
    public TankType tankType;
    public GameObject shellPrefab;

    public KeyCode fireKey = KeyCode.Space;
    public float shellSpeed = 20;
    public AudioClip shotAudio;


    private Transform firePoint;

    // Start is called before the first frame update
    void Start()
    {
        firePoint = transform.Find("FirePoint");
    }

    // Update is called once per frame
    void Update()
    {
        if (tankType == TankType.player)
        {
            if (Input.GetKeyDown(fireKey))
            {
                Fire();
            }
        }
    }

    public void Fire()
    {
        AudioSource.PlayClipAtPoint(shotAudio, transform.position);
        GameObject go =
            GameObject.Instantiate(shellPrefab, firePoint.position, firePoint.rotation) as GameObject;
        go.GetComponent<Rigidbody>().velocity = go.transform.forward * shellSpeed;
    }
}
