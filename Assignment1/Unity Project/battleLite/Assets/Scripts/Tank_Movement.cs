using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TankType
{
    player,
    enemy
}

public class Tank_Movement : MonoBehaviour
{
    public TankType tankType;
    public float speed = 5;
    public float angularSpeed = 5;


    private Rigidbody rigidbody;
    private GameObject player;


    // Start is called before the first frame update
    void Start()
    {
        rigidbody = this.GetComponent<Rigidbody>();
        if (tankType == TankType.enemy)
        {
            player = GameObject.Find("Player");
        }
    }

    private float tempMoveTime = 2.5f;

    private float attckCD = 1f;

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.GetInstance().isStart)
            return;
        if (tankType == TankType.player)
        {
            float v = Input.GetAxis("Vertical");
            rigidbody.velocity = transform.forward * v * speed;

            float h = Input.GetAxis("Horizontal");
            rigidbody.angularVelocity = transform.up * h * angularSpeed;
        }

        if (tankType == TankType.enemy)
        {
            if (Vector3.Distance(transform.position, player.transform.position) > 15f)
            {
                //go on patrol
                tempMoveTime -= Time.deltaTime;
                if (tempMoveTime > 0f)
                {
                    rigidbody.velocity = transform.forward * speed;
                }
                else
                {
                    transform.eulerAngles += new Vector3(0, 90, 0);
                    tempMoveTime = 1.5f;
                    rigidbody.velocity = transform.forward * speed;
                }
            }
            else //Attack the player
            {
                //Debug.LogError("StartAttack");
                rigidbody.velocity = Vector3.zero;
                transform.LookAt(player.transform);
                attckCD -= Time.deltaTime;
                if (attckCD <= 0)
                {
                    attckCD = 1.5f;
                    gameObject.GetComponent<Tank_Attack>().Fire();
                }
            }
        }
    }
}
